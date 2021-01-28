using Dapper;
using Microsoft.Extensions.Configuration;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.Queries.RawSql;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
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
    public class PlanoTrabalhoQuery : IPlanoTrabalhoQuery
    {
        private readonly IConfiguration Configuration;

        public PlanoTrabalhoQuery(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IApplicationResult<PlanoTrabalhoViewModel>> ObterPorChaveAsync(Guid planoTrabalhoId)
        {
            var result = new ApplicationResult<PlanoTrabalhoViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryFirstOrDefaultAsync<PlanoTrabalhoViewModel>(PlanoTrabalhoRawSqls.ObterPorChave, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<PlanoTrabalhoViewModel>> ObterTermoAceitePorChaveAsync(Guid planoTrabalhoId)
        {
            var result = new ApplicationResult<PlanoTrabalhoViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryFirstOrDefaultAsync<PlanoTrabalhoViewModel>(PlanoTrabalhoRawSqls.ObterTermoAceite, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }
        
        public async Task<IApplicationResult<PlanoTrabalhoViewModel>> ObterAtualAsync(UsuarioLogadoRequest request)
        {
            var result = new ApplicationResult<PlanoTrabalhoViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@situacao", (int)SituacaoPlanoTrabalhoEnum.EmExecucao, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@pessoaId", request.UsuarioLogadoId, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryFirstOrDefaultAsync<PlanoTrabalhoViewModel>(PlanoTrabalhoRawSqls.ObterPorSituacao, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<PlanoTrabalhoViewModel>> ObterEmHabiliacaoAsync(UsuarioLogadoRequest request)
        {
            var result = new ApplicationResult<PlanoTrabalhoViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@situacao", (int)SituacaoPlanoTrabalhoEnum.Habilitacao, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@pessoaId", request.UsuarioLogadoId, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryFirstOrDefaultAsync<PlanoTrabalhoViewModel>(PlanoTrabalhoRawSqls.ObterPorSituacao, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PlanoTrabalhoAtividadeViewModel>>> ObterAtividadesPlanoAsync(Guid planoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PlanoTrabalhoAtividadeViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var results = await connection.QueryMultipleAsync(PlanoTrabalhoRawSqls.ObterAtividadesPlano, parameters);

                var atividades = results.Read<PlanoTrabalhoAtividadeViewModel>();
                var itens = results.Read<PlanoTrabalhoAtividadeItemViewModel>();
                var criterios = results.Read<PlanoTrabalhoAtividadeCriterioViewModel>();
                var assuntos = results.Read<PlanoTrabalhoAtividadeAssuntoViewModel>();
                foreach (var atividade in atividades)
                {
                    atividade.Itens = itens.Where(a => a.PlanoTrabalhoAtividadeId == atividade.PlanoTrabalhoAtividadeId).ToList();
                    atividade.Criterios = criterios.Where(a => a.PlanoTrabalhoAtividadeId == atividade.PlanoTrabalhoAtividadeId).ToList();
                    atividade.Assuntos = assuntos.Where(a => a.PlanoTrabalhoAtividadeId == atividade.PlanoTrabalhoAtividadeId).ToList();
                }
                result.Result = atividades;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PlanoTrabalhoMetaViewModel>>> ObterMetasPlanoAsync(Guid planoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PlanoTrabalhoMetaViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PlanoTrabalhoMetaViewModel>(PlanoTrabalhoRawSqls.ObterMetasPlano, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PlanoTrabalhoReuniaoViewModel>>> ObterReunioesPlanoAsync(Guid planoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PlanoTrabalhoReuniaoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PlanoTrabalhoReuniaoViewModel>(PlanoTrabalhoRawSqls.ObterReunioesPlano, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PlanoTrabalhoCustoViewModel>>> ObterCustosPlanoAsync(Guid planoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PlanoTrabalhoCustoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PlanoTrabalhoCustoViewModel>(PlanoTrabalhoRawSqls.ObterCustosPlano, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PlanoTrabalhoHistoricoViewModel>>> ObterHistoricoPorPlanoAsync(Guid planoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PlanoTrabalhoHistoricoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PlanoTrabalhoHistoricoViewModel>(PlanoTrabalhoRawSqls.ObterHistoricoPlano, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }


        public async Task<IApplicationResult<DadosPaginadosViewModel<PlanoTrabalhoViewModel>>> ObterPorFiltroAsync(PlanoTrabalhoFiltroRequest request)
        {
            var result = new ApplicationResult<DadosPaginadosViewModel<PlanoTrabalhoViewModel>>();

            var query = PlanoTrabalhoRawSqls.ObterPorFiltro;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@situacaoId", request.SituacaoId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@dataInicio", request.DataInicio, DbType.Date, ParameterDirection.Input);
            parameters.Add("@dataFim", request.DataFim, DbType.Date, ParameterDirection.Input);

            parameters.Add("@offset", (request.Page - 1) * request.PageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@pageSize", request.PageSize, DbType.Int32, ParameterDirection.Input);

            var unidades = new List<Int64>();
            if (request.UnidadeId.HasValue) 
                unidades.Add(request.UnidadeId.Value);
            else if (request.UnidadesUsuario != null)
            {
                foreach (var unidadeId in request.UnidadesUsuario)
                    unidades.Add(unidadeId);
            }

            if (unidades.Any())
                query = query.Replace("#UNIDADES#", " AND p.unidadeId in (" + String.Join(',', unidades) + " )");
            else
                query = query.Replace("#UNIDADES#", string.Empty);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dadosPaginados = new DadosPaginadosViewModel<PlanoTrabalhoViewModel>(request);
                using (var multi = await connection.QueryMultipleAsync(query, parameters))
                {
                    dadosPaginados.Registros = multi.Read<PlanoTrabalhoViewModel>().ToList();
                    dadosPaginados.Controle.TotalRegistros = multi.ReadFirst<int>();
                    result.Result = dadosPaginados;
                }

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PlanoTrabalhoAtividadeCandidatoViewModel>>> ObterCandidatosPorAtividadeAsync(Guid planoTrabalhoAtividadeId)
        {
            var result = new ApplicationResult<IEnumerable<PlanoTrabalhoAtividadeCandidatoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoAtividadeId", planoTrabalhoAtividadeId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PlanoTrabalhoAtividadeCandidatoViewModel>(PlanoTrabalhoRawSqls.ObterCandidatosAtividade, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PlanoTrabalhoAtividadeCandidatoViewModel>>> ObterCandidatosAsync(Guid planoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PlanoTrabalhoAtividadeCandidatoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PlanoTrabalhoAtividadeCandidatoViewModel>(PlanoTrabalhoRawSqls.ObterCandidatosPlano, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PlanoTrabalhoPessoaModalidadeViewModel>>> ObterPessoasModalidadesAsync(Guid planoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PlanoTrabalhoPessoaModalidadeViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@situacaoAprovada", (int)SituacaoCandidaturaPlanoTrabalhoEnum.Aprovada, DbType.Int32, ParameterDirection.Input);
            
            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PlanoTrabalhoPessoaModalidadeViewModel>(PlanoTrabalhoRawSqls.ObterModalidadePorPessoaPlano, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PlanoTrabalhoObjetoViewModel>>> ObterObjetosPlanoAsync(Guid planoTrabalhoId)
        {
            var result = new ApplicationResult<IEnumerable<PlanoTrabalhoObjetoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryMultipleAsync(PlanoTrabalhoRawSqls.ObterObjetosPlano, parameters);
                var planoTrabalhoObjeto = dados.Read<PlanoTrabalhoObjetoViewModel>();
                var assuntos = dados.Read<PlanoTrabalhoObjetoAssuntoViewModel>().ToList();
                foreach (var objeto in planoTrabalhoObjeto)
                {
                    objeto.Assuntos = assuntos.Where(a => a.PlanoTrabalhoObjetoId == objeto.PlanoTrabalhoObjetoId).ToList();
                }

                result.Result = planoTrabalhoObjeto;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<PlanoTrabalhoObjetoViewModel>> ObterObjetoPlanoByIdAsync(Guid planoTrabalhoObjetoId)
        {
            var result = new ApplicationResult<PlanoTrabalhoObjetoViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoObjetoId", planoTrabalhoObjetoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryMultipleAsync(PlanoTrabalhoRawSqls.ObterObjetoPlanoById, parameters);
                var planoTrabalhoObjeto = dados.ReadFirst<PlanoTrabalhoObjetoViewModel>();
                planoTrabalhoObjeto.Assuntos = dados.Read<PlanoTrabalhoObjetoAssuntoViewModel>().ToList();
                planoTrabalhoObjeto.Custos = dados.Read<PlanoTrabalhoCustoViewModel>().ToList();
                planoTrabalhoObjeto.Reunioes = dados.Read<PlanoTrabalhoReuniaoViewModel>().ToList();

                result.Result = planoTrabalhoObjeto;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PlanoTrabalhoObjetoPactoAtividadeViewModel>>> ObterObjetosPlanoAssociadosOuNaoAAtividadesDoPactoAsync(Guid planoTrabalhoId, Guid pactoTrabalhoAtividadeId)
        {
            var result = new ApplicationResult<IEnumerable<PlanoTrabalhoObjetoPactoAtividadeViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@planoTrabalhoId", planoTrabalhoId, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@pactoTrabalhoAtividadeId", pactoTrabalhoAtividadeId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryMultipleAsync(PlanoTrabalhoRawSqls.ObterObjetosPlanoAssociadosOuNaoAAtividadesDoPacto, parameters);
                var objetosPlano = dados.Read<PlanoTrabalhoObjetoPactoAtividadeViewModel>().ToList();
                var assuntos = dados.Read<PlanoTrabalhoObjetoAssuntoViewModel>().ToList();
                var idObjetoComAtividade = dados.Read<Guid>().ToList();
                objetosPlano.ForEach(o =>
                {
                    o.Associado = idObjetoComAtividade.Contains(o.PlanoTrabalhoObjetoId);
                    o.Assuntos = assuntos.Where(a => a.PlanoTrabalhoObjetoId == o.PlanoTrabalhoObjetoId).ToList();
                });

                result.Result = objetosPlano;

                connection.Close();
            }

            return result;
        }
    }
}
