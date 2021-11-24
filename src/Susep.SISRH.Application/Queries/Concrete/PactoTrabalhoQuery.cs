using Dapper;
using Microsoft.Extensions.Configuration;
using SUSEP.Framework.Messages.Concrete.Request;
using SUSEP.Framework.Result.Abstractions;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.Transactions;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.ViewModels;
using Susep.SISRH.Application.Queries.RawSql;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Domain.Enums;

namespace Susep.SISRH.Application.Queries.Concrete
{
    public class PactoTrabalhoQuery : IPactoTrabalhoQuery
    {
        private readonly IConfiguration Configuration;

        public PactoTrabalhoQuery(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public async Task<IApplicationResult<PactoTrabalhoViewModel>> ObterPorChaveAsync(Guid pactoTrabalhoId)
        {
            var result = new ApplicationResult<PactoTrabalhoViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pactoTrabalhoId", pactoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryFirstOrDefaultAsync<PactoTrabalhoViewModel>(PactoTrabalhoRawSqls.ObterPorChave, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<PactoTrabalhoViewModel>> ObterAtualAsync(UsuarioLogadoRequest request)
        {
            var result = new ApplicationResult<PactoTrabalhoViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@situacaoRascunho", (int)SituacaoPactoTrabalhoEnum.Rascunho, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@situacaoEnviadoAceite", (int)SituacaoPactoTrabalhoEnum.EnviadoAceite, DbType.Int32, ParameterDirection.Input); 
            parameters.Add("@situacaoAceito", (int)SituacaoPactoTrabalhoEnum.Aceito, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@situacaoEmExecucao", (int)SituacaoPactoTrabalhoEnum.EmExecucao, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@pessoaId", request.UsuarioLogadoId, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryFirstOrDefaultAsync<PactoTrabalhoViewModel>(PactoTrabalhoRawSqls.ObterAtual, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PactoTrabalhoViewModel>>> ObterPorPlanoAsync(Guid planoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PactoTrabalhoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PactoTrabalhoViewModel>(PactoTrabalhoRawSqls.ObterPactosTrabalhoPorPlano, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PactoTrabalhoHistoricoViewModel>>> ObterHistoricoPorPactoAsync(Guid pactoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PactoTrabalhoHistoricoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pactoTrabalhoId", pactoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PactoTrabalhoHistoricoViewModel>(PactoTrabalhoRawSqls.ObterHistorico, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<DadosPaginadosViewModel<PactoTrabalhoViewModel>>> ObterPorFiltroAsync(PactoTrabalhoFiltroRequest request)
        {
            var result = new ApplicationResult<DadosPaginadosViewModel<PactoTrabalhoViewModel>>();

            var query = PactoTrabalhoRawSqls.ObterPorFiltro;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pessoaLogadaId", request.UsuarioLogadoId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@pessoaId", request.PessoaId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@formaExecucaoId", request.FormaExecucaoId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@situacaoId", request.SituacaoId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@dataInicio", request.DataInicio, DbType.Date, ParameterDirection.Input);
            parameters.Add("@dataFim", request.DataFim, DbType.Date, ParameterDirection.Input);

            parameters.Add("@offset", (request.Page - 1) * request.PageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@pageSize", request.PageSize, DbType.Int32, ParameterDirection.Input);            

            if (!request.IsGestor)
                query = query.Replace("#CONTROLE#", PactoTrabalhoRawSqls.ControleAcesso);
            else
                query = query.Replace("#CONTROLE#", string.Empty);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dadosPaginados = new DadosPaginadosViewModel<PactoTrabalhoViewModel>(request);
                using (var multi = await connection.QueryMultipleAsync(query, parameters))
                {
                    dadosPaginados.Registros = multi.Read<PactoTrabalhoViewModel>().ToList();
                    dadosPaginados.Controle.TotalRegistros = multi.ReadFirst<int>();
                    result.Result = dadosPaginados;
                }

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PactoTrabalhoAtividadeViewModel>>> ObterAtividadesPactoAsync(Guid pactoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PactoTrabalhoAtividadeViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pactoTrabalhoId", pactoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PactoTrabalhoAtividadeViewModel>(PactoTrabalhoRawSqls.ObterAtividades, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PactoTrabalhoSolicitacaoViewModel>>> ObteSolicitacoesPactoAsync(Guid pactoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PactoTrabalhoSolicitacaoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pactoTrabalhoId", pactoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PactoTrabalhoSolicitacaoViewModel>(PactoTrabalhoRawSqls.ObterSolicitacoes, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<PactoTrabalhoAssuntosParaAssociarViewModel>> ObterAssuntosParaAssociarAsync(Guid pactoTrabalhoId)
        {
            var result = new ApplicationResult<PactoTrabalhoAssuntosParaAssociarViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pactoTrabalhoId", pactoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (var multi = await connection.QueryMultipleAsync(PactoTrabalhoRawSqls.ObterAssuntosDoPactoEPlanoTrabalho, parameters))
                {
                    result.Result = new PactoTrabalhoAssuntosParaAssociarViewModel();
                    result.Result.TodosOsAssuntosParaAssociar = multi.Read<PactoTrabalhoAtividadeAssuntoViewModel>().ToList();
                    result.Result.AssuntosAssociados = multi.Read<PactoTrabalhoAtividadeAssuntoViewModel>().ToList();
                }

                connection.Close();
            }

            return result;
        }

    }
}
