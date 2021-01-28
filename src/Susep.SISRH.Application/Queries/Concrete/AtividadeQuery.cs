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

namespace Susep.SISRH.Application.Queries.Concrete
{
    public class AtividadeQuery : IAtividadeQuery
    {
        private readonly IConfiguration Configuration;

        public AtividadeQuery(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
    }
}
