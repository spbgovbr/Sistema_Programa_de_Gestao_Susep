using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Concrete;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.Messages.Concrete.Request;
using SUSEP.Framework.Result.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Queries.Abstractions
{
    public interface IDominioQuery
    {
        Task<IApplicationResult<IEnumerable<DominioViewModel>>> ObterDominioPorClassificacaoAsync(ClassificacaoCatalogoDominioEnum classificacao);
        Task<IApplicationResult<IEnumerable<DominioViewModel>>> ObterDominioPorSituacaoCandidaturaPlanoTrabalhoAsync(SituacaoCandidaturaPlanoTrabalhoEnum situacaoCandidaturaPlanoTrabalho);
    }
}
