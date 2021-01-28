using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate
{
    public interface IUnidadeRepository
    {
        Task<Unidade> ObterAsync(Guid unidadeId);

    }
}
