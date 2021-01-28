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
    public class ObjetoQuery : IObjetoQuery
    {
        private readonly IConfiguration Configuration;

        public ObjetoQuery(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IApplicationResult<DadosPaginadosViewModel<ObjetoViewModel>>> ObterPorFiltroAsync(ObjetoFiltroRequest request)
        {
            var result = new ApplicationResult<DadosPaginadosViewModel<ObjetoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@chave", request.Chave, DbType.String, ParameterDirection.Input);
            parameters.Add("@descricao", request.Descricao, DbType.String, ParameterDirection.Input);
            parameters.Add("@tipo", request.Tipo, DbType.Int64, ParameterDirection.Input);

            parameters.Add("@offset", (request.Page - 1) * request.PageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@pageSize", request.PageSize, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dadosPaginados = new DadosPaginadosViewModel<ObjetoViewModel>(request);


                using (var multi = await connection.QueryMultipleAsync(ObjetoRawSqls.ObterPorFiltro, parameters))
                {
                    dadosPaginados.Registros = multi.Read<ObjetoViewModel>().ToList();
                    dadosPaginados.Controle.TotalRegistros = multi.ReadFirst<int>();
                    result.Result = dadosPaginados;
                }

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<ObjetoViewModel>> ObterPorIdAsync(Guid id)
        {
            var result = new ApplicationResult<ObjetoViewModel>(null);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                result.Result = await connection.QueryFirstOrDefaultAsync<ObjetoViewModel>(ObjetoRawSqls.ObterPorId, parameters);
                
                connection.Close();
            }

            return result; 
        }

        public async Task<IApplicationResult<bool>> ChaveDuplicadaAsync(string chave, Guid? objetoId)
        {
            var result = new ApplicationResult<bool>(null);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@chave", chave, DbType.String, ParameterDirection.Input);
            parameters.Add("@objetoId", objetoId, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

//                var count = await connection.QueryFirstOrDefaultAsync(ObjetoRawSqls.CountChaveDuplicada, parameters);

//                Console.WriteLine("COUNT", count);

                result.Result = await connection.QueryFirstOrDefaultAsync<Int32>(ObjetoRawSqls.CountChaveDuplicada, parameters) > 0;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<ObjetoViewModel>>> ObterPorTextoAsync(string texto)
        {
            var result = new ApplicationResult<IEnumerable<ObjetoViewModel>>(null);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@texto", texto, DbType.String, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<ObjetoViewModel>(ObjetoRawSqls.ObterPorTexto, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }
    }
}
