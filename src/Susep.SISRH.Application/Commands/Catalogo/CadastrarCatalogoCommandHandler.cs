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
    public class CadastrarCatalogoCommandHandler : IRequestHandler<CadastrarCatalogoCommand, IActionResult>
    {
        private ICatalogoQuery CatalogoQuery { get; }        
        private ICatalogoRepository CatalogoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public CadastrarCatalogoCommandHandler(
            ICatalogoQuery catalogoQuery, 
            ICatalogoRepository catalogoRepository, 
            IUnitOfWork unitOfWork)
        {
            CatalogoQuery = catalogoQuery;
            CatalogoRepository = catalogoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(CadastrarCatalogoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<Guid> result = new ApplicationResult<Guid>(request);            

            //Monta o objeto com os dados do catalogo
            var catalogo = Domain.AggregatesModel.CatalogoAggregate.Catalogo.Criar(request.UnidadeId);
            
            //Adiciona o catalogo no banco de dados
            await CatalogoRepository.AdicionarAsync(catalogo);
            UnitOfWork.Commit(false);

            result.Result = catalogo.CatalogoId;
            result.SetHttpStatusToOk("Catálogo cadastrado com sucesso.");
            return result;
        }
    }
}
