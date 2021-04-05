using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using Susep.SISRH.Domain.AggregatesModel.ObjetoAggregate;
using Susep.SISRH.Domain.Exceptions;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.Assunto
{
    public class CadastrarObjetoCommandHandler : IRequestHandler<CadastrarObjetoCommand, IActionResult>
    {
        private IObjetoRepository ObjetoRepository { get; }
        private IObjetoQuery ObjetoQuery { get; }
        private IUnitOfWork UnitOfWork { get; }

        public CadastrarObjetoCommandHandler(
            IObjetoRepository objetoRepository, 
            IObjetoQuery objetoQuery,
            IUnitOfWork unitOfWork)
        {
            ObjetoRepository = objetoRepository;
            ObjetoQuery = objetoQuery;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(CadastrarObjetoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<Guid> result = new ApplicationResult<Guid>(request);            

            //Monta o objeto com os dados do assunto
            var objeto = Objeto.Criar(
                request.Chave.Replace("###", ""),
                request.Descricao,
                request.Tipo);

            try
            {
                await validarObjeto(objeto);
            } catch (SISRHDomainException e)
            {
                result.SetHttpStatusToBadRequest(e.Message);
                return result;
            }

            //Adiciona o assunto no banco de dados
            await ObjetoRepository.AdicionarAsync(objeto);
            UnitOfWork.Commit(false);

            result.Result = objeto.ObjetoId;
            result.SetHttpStatusToOk("Objeto cadastrado com sucesso.");
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
                
                var chaveDuplicada = await ObjetoQuery.ChaveDuplicadaAsync(objeto.Chave, null);
                if (chaveDuplicada.Result)
                {
                    throw new SISRHDomainException("Objeto com chave duplicada.");
                }
                
            }
        }

    }
}
