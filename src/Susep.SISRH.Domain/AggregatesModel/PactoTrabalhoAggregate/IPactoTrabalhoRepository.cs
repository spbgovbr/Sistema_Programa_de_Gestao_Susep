using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate
{
    public interface IPactoTrabalhoRepository
    {
        Task<PactoTrabalho> ObterAsync(Guid pactoTrabalhoId);

        Task<PactoTrabalho> AdicionarAsync(PactoTrabalho item);

        void Atualizar(PactoTrabalho item);

        void Excluir(PactoTrabalho item);
    }
}
