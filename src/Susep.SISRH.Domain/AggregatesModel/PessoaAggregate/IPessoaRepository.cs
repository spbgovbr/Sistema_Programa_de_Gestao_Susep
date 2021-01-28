using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Domain.AggregatesModel.PessoaAggregate
{
    public interface IPessoaRepository
    {
        Task<Pessoa> ObterAsync(Int64 pessoaId);

        Task<Pessoa> ObterPorCriteriosAsync(String email, String cpf);
    }
}
