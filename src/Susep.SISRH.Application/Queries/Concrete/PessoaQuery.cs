using Dapper;
using Microsoft.Extensions.Configuration;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.Queries.RawSql;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.Result.Abstractions;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Queries.Concrete
{
    public class PessoaQuery : IPessoaQuery
    {
        private readonly IConfiguration Configuration;
        private readonly IUnidadeQuery UnidadeQuery;

        public PessoaQuery(IConfiguration configuration, IUnidadeQuery unidadeQuery)
        {
            Configuration = configuration;
            UnidadeQuery = unidadeQuery;
        }

        public async Task<IApplicationResult<DashboardViewModel>> ObterDashboardAsync(UsuarioLogadoRequest request)
        {
            var result = new ApplicationResult<DashboardViewModel>()
            {
                Result = new DashboardViewModel()
            };

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pessoaId", request.UsuarioLogadoId, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dashboardData = await connection.QueryMultipleAsync(PessoaRawSqls.ObterDashboard, parameters);

                result.Result.PlanosTrabalho = dashboardData.Read<PlanoTrabalhoViewModel>().ToList();
                result.Result.PactosTrabalho = dashboardData.Read<PactoTrabalhoViewModel>().ToList();
                result.Result.Solicitacoes = dashboardData.Read<PactoTrabalhoSolicitacaoViewModel>().ToList();

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<DadosPaginadosViewModel<PessoaViewModel>>> ObterPorFiltroAsync(PessoaFiltroRequest request)
        {
            var result = new ApplicationResult<DadosPaginadosViewModel<PessoaViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pesNome", request.Nome, DbType.String, ParameterDirection.Input);
            parameters.Add("@unidadeId", request.UnidadeId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@catalogoDominioId", request.CatalogoDominioId, DbType.Int32, ParameterDirection.Input);

            parameters.Add("@offset", (request.Page - 1) * request.PageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@pageSize", request.PageSize, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dadosPaginados = new DadosPaginadosViewModel<PessoaViewModel>(request);


                using (var multi = await connection.QueryMultipleAsync(PessoaRawSqls.ObterPorFiltro, parameters))
                {
                    dadosPaginados.Registros = multi.Read<PessoaViewModel>().ToList();
                    dadosPaginados.Controle.TotalRegistros = multi.ReadFirst<int>();
                    result.Result = dadosPaginados;
                }

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<PessoaViewModel>> ObterPorChaveAsync(Int64 pessoaId)
        {
            var result = new ApplicationResult<PessoaViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pessoaId", pessoaId, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryFirstOrDefaultAsync<PessoaViewModel>(PessoaRawSqls.ObterPorChave, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<DateTime>>> ObterDiasNaoUteisAsync(Int64 pessoaId, DateTime dataInicio, DateTime dataFim)
        {
            var result = new ApplicationResult<IEnumerable<DateTime>>();

            //Obtém os dados da pessoa
            var dadosPessoa = await this.ObterPorChaveAsync(pessoaId);

            //Obtém os feriados pela unidade da pessoa
            var feriados = await UnidadeQuery.ObterFeriadosPorUnidadeAsync(dadosPessoa.Result.UnidadeId, dataInicio, dataFim);

            result.Result = feriados.Result.Select(f => f.Date);

            return result;
        }

        public async Task<IApplicationResult<PessoaViewModel>> ObterDetalhesPorChaveAsync(long pessoaId)
        {
            var result = new ApplicationResult<PessoaViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pessoaId", pessoaId, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var results = await connection.QueryMultipleAsync(PessoaRawSqls.ObterDetalhes, parameters);

                var pessoa = results.Read<PessoaViewModel>().FirstOrDefault();

                pessoa.Candidaturas = results.Read<PlanoTrabalhoAtividadeCandidatoViewModel>().ToList();

                var tarefas = results.Read<PlanoTrabalhoAtividadeItemViewModel>();

                var perfis = results.Read<PlanoTrabalhoAtividadeCriterioViewModel>();

                var historicoCandidaturas = results.Read<PlanoTrabalhoAtividadeCandidatoHistoricoViewModel>();

                foreach (PlanoTrabalhoAtividadeCandidatoViewModel candidatura in pessoa.Candidaturas)
                {
                    candidatura.Tarefas = tarefas.Where(r => r.PlanoTrabalhoAtividadeId == candidatura.PlanoTrabalhoAtividadeId).ToList();
                    candidatura.Perfis = perfis.Where(r => r.PlanoTrabalhoAtividadeId == candidatura.PlanoTrabalhoAtividadeId).ToList();
                    candidatura.Descricao = historicoCandidaturas.Where(r => r.PlanoTrabalhoAtividadeCandidatoId == candidatura.PlanoTrabalhoAtividadeCandidatoId && (r.SituacaoId == (int)SituacaoCandidaturaPlanoTrabalhoEnum.Aprovada || r.SituacaoId == (int)SituacaoCandidaturaPlanoTrabalhoEnum.Rejeitada)).OrderByDescending(r => r.data).FirstOrDefault()?.descricao;
                }

                result.Result = pessoa;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PessoaViewModel>>> ObterComPactoTrabalhoAsync()
        {
            var result = new ApplicationResult<IEnumerable<PessoaViewModel>>();

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PessoaViewModel>(PessoaRawSqls.ObterComPactoTrabalho);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

    }
}
