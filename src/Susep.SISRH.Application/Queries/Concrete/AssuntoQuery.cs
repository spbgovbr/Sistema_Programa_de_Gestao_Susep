using Dapper;
using Microsoft.EntityFrameworkCore.Internal;
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
    public class AssuntoQuery : IAssuntoQuery
    {
        private readonly IConfiguration Configuration;

        public AssuntoQuery(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IApplicationResult<DadosPaginadosViewModel<AssuntoViewModel>>> ObterPorFiltroAsync(AssuntoFiltroRequest request)
        {
            var result = new ApplicationResult<DadosPaginadosViewModel<AssuntoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@valor", request.Valor, DbType.String, ParameterDirection.Input);

            parameters.Add("@offset", (request.Page - 1) * request.PageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@pageSize", request.PageSize, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dadosPaginados = new DadosPaginadosViewModel<AssuntoViewModel>(request);


                using (var multi = await connection.QueryMultipleAsync(AssuntoRawSqls.ObterPorFiltro, parameters))
                {
                    dadosPaginados.Registros = multi.Read<AssuntoViewModel>().ToList();
                    dadosPaginados.Controle.TotalRegistros = multi.ReadFirst<int>();
                    result.Result = dadosPaginados;
                }

                connection.Close();
            }

            return result;
        }
        public async Task<IApplicationResult<IEnumerable<AssuntoViewModel>>> ObterAtivosAsync()
        {
            var result = new ApplicationResult<IEnumerable<AssuntoViewModel>>();

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                result.Result = await connection.QueryAsync<AssuntoViewModel>(AssuntoRawSqls.ObterAtivos);

                connection.Close();
            }

            return result;
        }


        public async Task<IApplicationResult<AssuntoEdicaoViewModel>> ObterPorIdAsync(Guid id)
        {
            var result = new ApplicationResult<AssuntoEdicaoViewModel>(null);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                AssuntoEdicaoViewModel assunto;

                using (var dados = await connection.QueryMultipleAsync(AssuntoRawSqls.ObterPorId, parameters))
                {
                    var assuntos = dados.Read<AssuntoEdicaoViewModel>().ToList();
                    assunto = assuntos.Where(a => a.AssuntoId == id).First();
                    if (assunto.AssuntoPaiId != null)
                    {
                        var pai = assuntos.Where(a => a.AssuntoId == assunto.AssuntoPaiId).FirstOrDefault();
                        assunto.Pai = pai;
                    }
                }
                    
                result.Result = assunto;

                connection.Close();
            }

            return result;
        }


        public async Task<IApplicationResult<IEnumerable<AssuntoViewModel>>> ObterPorTextoAsync(string texto)
        {
            var result = new ApplicationResult<IEnumerable<AssuntoViewModel>>(null);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@texto", texto, DbType.String, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<AssuntoViewModel>(AssuntoRawSqls.ObterPorTexto, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<Guid>>> ObterIdsDeTodosOsPaisAsync(Guid assuntoId)
        {
            var result = new ApplicationResult<IEnumerable<Guid>>(null);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@assuntoId", assuntoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<Guid>(AssuntoRawSqls.ObterIdsDeTodosOsPais, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<Guid>>> ObterIdsDeTodosOsFilhosAsync(Guid assuntoId)
        {
            var result = new ApplicationResult<IEnumerable<Guid>>(null);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@assuntoId", assuntoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<Guid>(AssuntoRawSqls.ObterIdsDeTodosOsFilhos, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<bool>> ValorDuplicadoAsync(string valor, Guid? assuntoId)
        {
            var result = new ApplicationResult<bool>(null);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@valor", valor, DbType.String, ParameterDirection.Input);
            parameters.Add("@assuntoId", assuntoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<int>(AssuntoRawSqls.ObterPorValor, parameters);
                result.Result = dados.FirstOrDefault() > 0;

                connection.Close();
            }

            return result;
        }

    }
}
