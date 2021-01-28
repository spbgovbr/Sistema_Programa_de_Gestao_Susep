using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.Catalogo
{
    public class AlterarCatalogoCommandHandler : IRequestHandler<AlterarCatalogoCommand, IActionResult>
    {
        private ICatalogoQuery CatalogoQuery { get; }        
        private ICatalogoRepository CatalogoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public AlterarCatalogoCommandHandler(
            ICatalogoQuery catalogoQuery, 
            ICatalogoRepository catalogoRepository, 
            IUnitOfWork unitOfWork)
        {
            CatalogoQuery = catalogoQuery;
            CatalogoRepository = catalogoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(AlterarCatalogoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);            

            //Monta o objeto com os dados do catalogo
            var catalogo = await CatalogoRepository.ObterAsync(request.CatalogoId);

            //Adiciona o catalogo no banco de dados
            CatalogoRepository.Atualizar(catalogo);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Catalogo alterado com sucesso.");
            return result;
        }
    }
}
