using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate
{
    public interface IAssuntoRepository
    {
        Task<Assunto> ObterAsync(Guid assuntoId);
        Task<Assunto> AdicionarAsync(Assunto item);
        void Atualizar(Assunto item);

    }
}
