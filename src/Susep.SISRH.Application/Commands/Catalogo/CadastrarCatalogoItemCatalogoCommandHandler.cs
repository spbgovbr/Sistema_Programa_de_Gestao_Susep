using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.Catalogo
{
    public class CadastrarCatalogoItemCatalogoCommandHandler : IRequestHandler<CadastrarCatalogoItemCatalogoCommand, IActionResult>
    {
        private ICatalogoQuery CatalogoQuery { get; }
        private ICatalogoRepository CatalogoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public CadastrarCatalogoItemCatalogoCommandHandler(
            ICatalogoQuery catalogoQuery,
            ICatalogoRepository catalogoRepository,
            IUnitOfWork unitOfWork)
        {
            CatalogoQuery = catalogoQuery;
            CatalogoRepository = catalogoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(CadastrarCatalogoItemCatalogoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            var catalogo = await CatalogoRepository.ObterAsync(request.CatalogoId);

            catalogo.AdicionarItem(request.ItemCatalogoId);

            //Adiciona o catalogo no banco de dados
            CatalogoRepository.Atualizar(catalogo);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Item cadastrado com sucesso.");
            return result;
        }
    }
}
