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
    public class APIExtracaoOrgaoCentralQuery : IAPIExtracaoOrgaoCentralQuery
    {
        private readonly IConfiguration Configuration;

        public APIExtracaoOrgaoCentralQuery(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IApplicationResult<IEnumerable<APIPlanoTrabalhoViewModel>>> ObterPlanosTrabalhoAsync()
        {
            var result = new ApplicationResult<IEnumerable<APIPlanoTrabalhoViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            //parameters.Add("@chave", request.Chave, DbType.String, ParameterDirection.Input);
            try
            {
                using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var results = await connection.QueryMultipleAsync(APIExtracaoOrgaoCentralRawSqls.ObterPlanosTrabalho, parameters))
                    {
                        var planos = results.Read<APIPlanoTrabalhoViewModel>();
                        var atividades = results.Read<APIPlanoTrabalhoAtividadeViewModel>();

                        foreach (var plano in planos)
                        {
                            plano.Atividades = atividades.Where(a => a.PactoTrabalhoId == plano.PactoTrabalhoId).ToList();
                        }

                        result.Result = planos;
                    }

                    connection.Close();
                }
            }
            catch (System.Exception ex)
            {

            }

            return result;
        }


    }
}
