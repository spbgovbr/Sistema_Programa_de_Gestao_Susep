using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Domain.AggregatesModel.ObjetoAggregate
{
    public interface IObjetoRepository
    {
        Task<Objeto> ObterAsync(Guid assuntoId);
        Task<Objeto> AdicionarAsync(Objeto item);
        void Atualizar(Objeto item);

    }
}
