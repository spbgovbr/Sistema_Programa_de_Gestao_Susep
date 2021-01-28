using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.ViewModels;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.ItemCatalogo
{
    public class AlterarItemCatalogoCommandHandler : IRequestHandler<AlterarItemCatalogoCommand, IActionResult>
    {
        private IItemCatalogoQuery ItemCatalogoQuery { get; }        
        private IItemCatalogoRepository ItemCatalogoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public AlterarItemCatalogoCommandHandler(
            IItemCatalogoQuery catalogoQuery, 
            IItemCatalogoRepository itemCatalogoRepository, 
            IUnitOfWork unitOfWork)
        {
            ItemCatalogoQuery = catalogoQuery;
            ItemCatalogoRepository = itemCatalogoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(AlterarItemCatalogoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            request.Assuntos = request.Assuntos != null ? request.Assuntos : new List<AssuntoEdicaoViewModel>();

            //Monta o objeto com os dados do catalogo
            var item = await ItemCatalogoRepository.ObterAsync(request.ItemCatalogoId);

            //Altera os dados
            item.Alterar(request.Titulo,
                request.Descricao,
                request.FormaCalculoTempoItemCatalogoId,
                request.PermiteTrabalhoRemoto,
                request.TempoExecucaoPresencial,
                request.TempoExecucaoRemoto,
                request.Complexidade,
                request.DefinicaoComplexidade,
                request.EntregasEsperadas);

            //Atualiza a lista de assuntos
            atualizarListaAssuntos(item, request.Assuntos);

            //Altera o item de catalogo no banco de dados
            ItemCatalogoRepository.Atualizar(item);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("ItemCatalogo alterado com sucesso.");
            return result;
        }

        private void atualizarListaAssuntos(Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate.ItemCatalogo itemCatalogo, IEnumerable<AssuntoEdicaoViewModel> assuntosViewModel)
        {
            var idsViewModel = assuntosViewModel.Select(a => a.AssuntoId);
            var idsModelo = itemCatalogo.Assuntos.Select(a => a.AssuntoId);
            var assuntosParaAssociar = idsViewModel.Where(id => !idsModelo.Contains(id)).ToList();
            var assuntosParaRemover = idsModelo.Where(id => !idsViewModel.Contains(id)).ToList();
            foreach (var id in assuntosParaRemover)
            {
                itemCatalogo.RemoverAssunto(id);
            }
            foreach (var id in assuntosParaAssociar)
            {
                itemCatalogo.AdicionarAssunto(id);
            }
        }
    }
}
