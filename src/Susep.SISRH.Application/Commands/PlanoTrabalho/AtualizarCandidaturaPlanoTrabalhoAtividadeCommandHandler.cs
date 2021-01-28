using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class AtualizarCandidaturaPlanoTrabalhoAtividadeCommandHandler : IRequestHandler<AtualizarCandidaturaPlanoTrabalhoAtividadeCommand, IActionResult>
    {
        private IPlanoTrabalhoRepository PlanoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public AtualizarCandidaturaPlanoTrabalhoAtividadeCommandHandler(            
            IPlanoTrabalhoRepository planoTrabalhoRepository, 
            IUnitOfWork unitOfWork)
        {
            PlanoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(AtualizarCandidaturaPlanoTrabalhoAtividadeCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);            

            //Monta o objeto com os dados do catalogo
            var item = await PlanoTrabalhoRepository.ObterAsync(request.PlanoTrabalhoId);

            //Atualiza a candidatura
            item.AtualizarCandidaturaAtividade(request.PlanoTrabalhoAtividadeId, request.PlanoTrabalhoAtividadeCandidatoId, request.SituacaoId, request.UsuarioLogadoId.ToString(), request.Descricao);

            //Altera o item de catalogo no banco de dados
            PlanoTrabalhoRepository.Atualizar(item);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Candidatura registrada com sucesso.");
            return result;
        }
    }
}
