using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.ItemCatalogo
{
    public class CadastrarItemCatalogoCommandHandler : IRequestHandler<CadastrarItemCatalogoCommand, IActionResult>
    {
        private IItemCatalogoQuery ItemCatalogoQuery { get; }        
        private IItemCatalogoRepository ItemCatalogoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public CadastrarItemCatalogoCommandHandler(
            IItemCatalogoQuery catalogoQuery, 
            IItemCatalogoRepository catalogoRepository, 
            IUnitOfWork unitOfWork)
        {
            ItemCatalogoQuery = catalogoQuery;
            ItemCatalogoRepository = catalogoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(CadastrarItemCatalogoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<Guid> result = new ApplicationResult<Guid>(request);

            request.Assuntos = request.Assuntos != null ? request.Assuntos : new System.Collections.Generic.List<AssuntoEdicaoViewModel>();

            //Monta o objeto com os dados do item de catalogo
            var itemCatalogo = Domain.AggregatesModel.CatalogoAggregate.ItemCatalogo.Criar(
                request.Titulo, 
                request.Descricao,
                request.FormaCalculoTempoItemCatalogoId,
                request.PermiteTrabalhoRemoto,
                request.TempoExecucaoPresencial,
                request.TempoExecucaoRemoto,
                request.Complexidade,
                request.DefinicaoComplexidade,
                request.EntregasEsperadas);

            foreach (var assunto in request.Assuntos)
            {
                itemCatalogo.AdicionarAssunto(assunto.AssuntoId);
            }

            //Adiciona o catalogo no banco de dados
            await ItemCatalogoRepository.AdicionarAsync(itemCatalogo);
            UnitOfWork.Commit(false);

            result.Result = itemCatalogo.ItemCatalogoId;
            result.SetHttpStatusToOk("ItemCatalogo cadastrado com sucesso.");
            return result;
        }
    }
}
