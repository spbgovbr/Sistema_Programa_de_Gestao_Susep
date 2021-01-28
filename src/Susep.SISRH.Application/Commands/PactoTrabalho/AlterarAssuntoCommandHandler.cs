using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using Susep.SISRH.Application.Queries.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Susep.SISRH.Domain.Exceptions;
using System.Linq;

namespace Susep.SISRH.Application.Commands.Assunto
{
    public class AlterarAssuntoCommandHandler : IRequestHandler<AlterarAssuntoCommand, IActionResult>
    {
        private IAssuntoRepository AssuntoRepository { get; }
        private IAssuntoQuery AssuntoQuery { get; }
        private IUnitOfWork UnitOfWork { get; }

        public AlterarAssuntoCommandHandler(
            IAssuntoRepository assuntoRepository, 
            IAssuntoQuery assuntoQuery,
            IUnitOfWork unitOfWork)
        {
            AssuntoRepository = assuntoRepository;
            AssuntoQuery = assuntoQuery;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(AlterarAssuntoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<Guid> result = new ApplicationResult<Guid>(request);

            //Recupera o objeto assunto
            var assunto = await AssuntoRepository.ObterAsync(request.AssuntoId);

            var alterouPai = assunto.AssuntoPaiId != request.AssuntoPaiId;

            //Monta o objeto com os dados do assunto
            assunto.Alterar(request.Valor, request.AssuntoPaiId, request.Ativo);

            try
            {
                await validarAssunto(assunto, alterouPai);
            }
            catch (SISRHDomainException e)
            {
                result.SetHttpStatusToBadRequest(e.Message);
                return result;
            }

            //Altera o assunto no banco de dados
            AssuntoRepository.Atualizar(assunto);
            UnitOfWork.Commit(false);

            result.Result = assunto.AssuntoId;
            result.SetHttpStatusToOk("Assunto alterado com sucesso.");
            return result;
        }

        private async Task validarAssunto(Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate.Assunto assunto, bool alterouPai)
        {
            await assuntoComValorDuplicado(assunto);
            if (alterouPai)
            {
                assuntoPaiIgualAoEditado(assunto);
                await novoPaiEhAncestralDoAssuntoEmEdicao(assunto);
            }
        }

        private async Task assuntoComValorDuplicado(Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate.Assunto assunto)
        {
            if (assunto != null)
            {
                var valorDuplicado = await AssuntoQuery.ValorDuplicadoAsync(assunto.Valor, assunto.AssuntoId);
                if (valorDuplicado.Result)
                {
                    throw new SISRHDomainException("Assunto com valor duplicado.");
                }
            }
        }

        private void assuntoPaiIgualAoEditado(Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate.Assunto assunto)
        {
            if (assunto != null && assunto.AssuntoPaiId != null && assunto.AssuntoPaiId == assunto.AssuntoId)
            {
                throw new SISRHDomainException("O assunto pai escolhido não pode ser o assunto em edição.");
            }
        }

        private async Task novoPaiEhAncestralDoAssuntoEmEdicao(Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate.Assunto assuntoEmEdicao)
        {
            if (assuntoEmEdicao != null && assuntoEmEdicao.AssuntoPaiId != null)
            {
                var ids = await AssuntoQuery.ObterIdsDeTodosOsPaisAsync(assuntoEmEdicao.AssuntoPaiId.Value);
                if (ids.Result.Where(id => id == assuntoEmEdicao.AssuntoId).Count() > 0)
                {
                    throw new SISRHDomainException("O assunto pai escolhido não pode ser ancestral do assunto em edição.");
                }
            }
        }

    }
}
