using System;
using System.Threading.Tasks;

namespace Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate
{
    public interface ICatalogoRepository
    {
        Task<Catalogo> ObterAsync(Guid catalogoId);

        Task<Catalogo> AdicionarAsync(Catalogo item);

        void Atualizar(Catalogo item);

        void Excluir(Catalogo item);

        Task ExcluirAsync(Guid catalogoId);
    }
}
