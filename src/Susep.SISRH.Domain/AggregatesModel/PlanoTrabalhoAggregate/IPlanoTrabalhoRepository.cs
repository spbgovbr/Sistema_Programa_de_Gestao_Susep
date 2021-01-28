using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate
{
    public interface IPlanoTrabalhoRepository
    {
        Task<PlanoTrabalho> ObterAsync(Guid planoTrabalhoId);

        Task<PlanoTrabalho> AdicionarAsync(PlanoTrabalho item);

        void Atualizar(PlanoTrabalho item);
    }
}
