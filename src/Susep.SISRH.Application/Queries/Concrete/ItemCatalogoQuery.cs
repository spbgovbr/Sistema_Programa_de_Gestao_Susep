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
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.Queries.RawSql;

namespace Susep.SISRH.Application.Queries.Concrete
{
    public class ItemCatalogoQuery : IItemCatalogoQuery
    {
        private readonly IConfiguration Configuration;

        public ItemCatalogoQuery(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IApplicationResult<ItemCatalogoViewModel>> ObterPorChaveAsync(Guid itemCatalogoid)
        {
            var result = new ApplicationResult<ItemCatalogoViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@itemCatalogoid", itemCatalogoid, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                ItemCatalogoViewModel itemCatalogo;

                using (var multi = await connection.QueryMultipleAsync(ItemCatalogoRawSqls.ObterPorChave, parameters))
                {
                    itemCatalogo = multi.Read<ItemCatalogoViewModel>().First();
                    itemCatalogo.Assuntos = multi.Read<AssuntoViewModel>().ToList();
                    result.Result = itemCatalogo;
                }

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<DadosPaginadosViewModel<ItemCatalogoViewModel>>> ObterPorFiltroAsync(ItemCatalogoFiltroRequest request)
        {
            var result = new ApplicationResult<DadosPaginadosViewModel<ItemCatalogoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@titulo", request.Titulo, DbType.String, ParameterDirection.Input);
            parameters.Add("@formaCalculoTempoId", request.FormaCalculoTempoId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@permiteTrabalhoRemoto", request.PermiteTrabalhoRemoto, DbType.Boolean, ParameterDirection.Input);

            parameters.Add("@offset", (request.Page - 1) * request.PageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@pageSize", request.PageSize, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dadosPaginados = new DadosPaginadosViewModel<ItemCatalogoViewModel>(request);
                using (var multi = await connection.QueryMultipleAsync(ItemCatalogoRawSqls.ObterPorFiltro, parameters))
                {
                    dadosPaginados.Registros = multi.Read<ItemCatalogoViewModel>().ToList();
                    dadosPaginados.Controle.TotalRegistros = multi.ReadFirst<int>();
                    result.Result = dadosPaginados;
                }   

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<ItemCatalogoViewModel>>> ObterTodos(ItemCatalogoFiltroRequest request)
        {
            var result = new ApplicationResult<IEnumerable<ItemCatalogoViewModel>>();
          
            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<ItemCatalogoViewModel>(ItemCatalogoRawSqls.ObterTodos);
                
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

    }
}
