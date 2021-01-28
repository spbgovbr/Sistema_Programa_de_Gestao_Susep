using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using Susep.SISRH.Domain.Exceptions;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.Assunto
{
    public class CadastrarAssuntoCommandHandler : IRequestHandler<CadastrarAssuntoCommand, IActionResult>
    {
        private IAssuntoRepository AssuntoRepository { get; }
        private IAssuntoQuery AssuntoQuery { get; }
        private IUnitOfWork UnitOfWork { get; }

        public CadastrarAssuntoCommandHandler(
            IAssuntoRepository assuntoRepository, 
            IAssuntoQuery assuntoQuery,
            IUnitOfWork unitOfWork)
        {
            AssuntoRepository = assuntoRepository;
            AssuntoQuery = assuntoQuery;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(CadastrarAssuntoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<Guid> result = new ApplicationResult<Guid>(request);            

            //Monta o objeto com os dados do assunto
            var assunto = Domain.AggregatesModel.AssuntoAggregate.Assunto.Criar(
                request.Valor,
                request.AssuntoPaiId,
                true);

            try
            {
                await validarAssunto(assunto);
            } catch (SISRHDomainException e)
            {
                result.SetHttpStatusToBadRequest(e.Message);
                return result;
            }

            //Adiciona o assunto no banco de dados
            await AssuntoRepository.AdicionarAsync(assunto);
            UnitOfWork.Commit(false);

            result.Result = assunto.AssuntoId;
            result.SetHttpStatusToOk("Assunto cadastrado com sucesso.");
            return result;
        }

        private async Task validarAssunto(Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate.Assunto assunto)
        {
            await assuntoComValorDuplicado(assunto);
        }

        private async Task assuntoComValorDuplicado(Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate.Assunto assunto)
        {
            if (assunto != null)
            {
                var valorDuplicado = await AssuntoQuery.ValorDuplicadoAsync(assunto.Valor, null);
                if (valorDuplicado.Result)
                {
                    throw new SISRHDomainException("Assunto com valor duplicado.");
                }
            }
        }

    }
}
