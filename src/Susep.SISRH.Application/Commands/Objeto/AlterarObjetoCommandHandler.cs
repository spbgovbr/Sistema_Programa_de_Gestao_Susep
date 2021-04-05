using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using Susep.SISRH.Application.Queries.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Susep.SISRH.Domain.AggregatesModel.ObjetoAggregate;
using Susep.SISRH.Domain.Exceptions;

namespace Susep.SISRH.Application.Commands.Assunto
{
    public class AlterarObjetoCommandHandler : IRequestHandler<AlterarObjetoCommand, IActionResult>
    {
        private IObjetoRepository ObjetoRepository { get; }
        private IObjetoQuery ObjetoQuery { get; }
        private IUnitOfWork UnitOfWork { get; }

        public AlterarObjetoCommandHandler(
            IObjetoRepository objetoRepository, 
            IObjetoQuery objetoQuery,
            IUnitOfWork unitOfWork)
        {
            ObjetoRepository = objetoRepository;
            ObjetoQuery = objetoQuery;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(AlterarObjetoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<Guid> result = new ApplicationResult<Guid>(request);

            //Recupera o objeto
            var objeto = await ObjetoRepository.ObterAsync(request.ObjetoId);

            //Monta o objeto
            objeto.Alterar(request.Chave.Replace("###", ""), request.Descricao, request.Tipo, request.Ativo);

            try
            {
                await validarObjeto(objeto);
            }
            catch (SISRHDomainException e)
            {
                result.SetHttpStatusToBadRequest(e.Message);
                return result;
            }

            //Altera o objeto no banco de dados
            ObjetoRepository.Atualizar(objeto);
            UnitOfWork.Commit(false);

            result.Result = objeto.ObjetoId;
            result.SetHttpStatusToOk("Objeto alterado com sucesso.");
            return result;
        }

        private async Task validarObjeto(Objeto objeto)
        {
            await objetoComChaveDuplicada(objeto);
        }

        private async Task objetoComChaveDuplicada(Objeto objeto)
        {
            if (objeto != null)
            {
                
                var chaveDuplicada = await ObjetoQuery.ChaveDuplicadaAsync(objeto.Chave, objeto.ObjetoId);
                if (chaveDuplicada.Result)
                {
                    throw new SISRHDomainException("Objeto com chave duplicada.");
                }
                
            }
        }
    }
}
