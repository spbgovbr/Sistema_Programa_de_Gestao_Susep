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
    public class CatalogoQuery : ICatalogoQuery
    {
        private readonly IConfiguration Configuration;

        public CatalogoQuery(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IApplicationResult<CatalogoViewModel>> ObterPorChaveAsync(Guid catalogoid)
        {
            var result = new ApplicationResult<CatalogoViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@catalogoid", catalogoid, DbType.Guid, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<CatalogoViewModel, UnidadeViewModel, CatalogoViewModel>(
                    CatalogoRawSqls.ObterPorChave,
                    (catalogo, unidade) =>
                    {
                        catalogo.Unidade = unidade;
                        return catalogo;
                    },
                    splitOn: "unidadeId",
                    param: parameters
                );
                result.Result = dados.FirstOrDefault();                

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<DadosPaginadosViewModel<CatalogoViewModel>>> ObterPorFiltroAsync(CatalogoFiltroRequest request)
        {
            var result = new ApplicationResult<DadosPaginadosViewModel<CatalogoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@unidadeId", request.UnidadeId, DbType.Int32, ParameterDirection.Input);

            parameters.Add("@offset", (request.Page - 1) * request.PageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@pageSize", request.PageSize, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dadosPaginados = new DadosPaginadosViewModel<CatalogoViewModel>(request);
                using (var multi = await connection.QueryMultipleAsync(CatalogoRawSqls.ObterPorFiltro, parameters))
                {
                    dadosPaginados.Registros = multi.Read<CatalogoViewModel, UnidadeViewModel, CatalogoViewModel>((catalogo, unidade) =>
                    {
                        catalogo.Unidade = unidade;
                        return catalogo;
                    }, splitOn: "unidadeId").ToList();
                    dadosPaginados.Controle.TotalRegistros = multi.ReadFirst<int>();
                    result.Result = dadosPaginados;
                }                

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<CatalogoViewModel>> ObterPorUnidadeAsync(Int32 unidadeId)
        {
            var result = new ApplicationResult<CatalogoViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@unidadeId", unidadeId, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<CatalogoViewModel>(CatalogoRawSqls.ObterPorUnidade, parameters);

                var itens = await connection.QueryAsync<ItemCatalogoViewModel>(ItemCatalogoRawSqls.ObterPorUnidade, parameters);

                var retorno = dados.FirstOrDefault();
                
                retorno.Itens= itens;

                result.Result = retorno;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<ItemCatalogoViewModel>>> ObterItensPorUnidadeAsync(Int32 unidadeId)
        {
            var result = new ApplicationResult<IEnumerable<ItemCatalogoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@unidadeId", unidadeId, DbType.Int32, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var itemCatalogoDictionary = new Dictionary<Guid, ItemCatalogoViewModel>();

                var dados = await connection.QueryAsync<ItemCatalogoViewModel, AssuntoViewModel, ItemCatalogoViewModel>(
                    CatalogoRawSqls.ObterItensPorUnidade, 
                    (itemCatalogo, assunto) =>
                    {
                        ItemCatalogoViewModel itemCatalogoEntry;

                        if (!itemCatalogoDictionary.TryGetValue(itemCatalogo.ItemCatalogoId, out itemCatalogoEntry))
                        {
                            itemCatalogoEntry = itemCatalogo;
                            itemCatalogoEntry.Assuntos = new List<AssuntoViewModel>();
                            itemCatalogoDictionary.Add(itemCatalogoEntry.ItemCatalogoId, itemCatalogoEntry);
                        }
                        
                        if (assunto != null)
                        {
                            itemCatalogoEntry.Assuntos.AsList().Add(assunto);
                        }

                        return itemCatalogoEntry;

                    },
                    splitOn: "assuntoId",
                    param: parameters);

                result.Result = dados.Distinct().ToList();

                connection.Close();
            }

            return result;
        }
    }
}
