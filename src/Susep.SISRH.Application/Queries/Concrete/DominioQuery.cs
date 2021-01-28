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
using Susep.SISRH.Domain.Enums;
using Susep.SISRH.Application.Queries.RawSql;

namespace Susep.SISRH.Application.Queries.Concrete
{
    public class DominioQuery : IDominioQuery
    {
        private readonly IConfiguration Configuration;

        public DominioQuery(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public async Task<IApplicationResult<IEnumerable<DominioViewModel>>> ObterDominioPorClassificacaoAsync(ClassificacaoCatalogoDominioEnum classificacao)
        {
            IApplicationResult<IEnumerable<DominioViewModel>> result = new ApplicationResult<IEnumerable<DominioViewModel>>();

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@classificacao", classificacao.ToString(), DbType.String, ParameterDirection.Input);

                var dados = await connection.QueryAsync<DominioViewModel>(DominioRawSqls.ObterDominios, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<DominioViewModel>>> ObterDominioPorSituacaoCandidaturaPlanoTrabalhoAsync(SituacaoCandidaturaPlanoTrabalhoEnum situacaoCandidaturaPlanoTrabalho)
        {
            IApplicationResult<IEnumerable<DominioViewModel>> result = new ApplicationResult<IEnumerable<DominioViewModel>>();

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", (Int32) situacaoCandidaturaPlanoTrabalho, DbType.Int32, ParameterDirection.Input);

                var dados = await connection.QueryAsync<DominioViewModel>(DominioRawSqls.ObterPorChave, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }


    }
}
