using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.ViewModels;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class AlterarPlanoTrabalhoObjetoCommandHandler : IRequestHandler<AlterarPlanoTrabalhoObjetoCommand, IActionResult>
    {
        private IPlanoTrabalhoRepository PlanoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public AlterarPlanoTrabalhoObjetoCommandHandler(            
            IPlanoTrabalhoRepository planoTrabalhoRepository, 
            IUnitOfWork unitOfWork)
        {
            PlanoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(AlterarPlanoTrabalhoObjetoCommand request, CancellationToken cancellationToken)
        {

            request.Assuntos = request.Assuntos != null ? request.Assuntos : new List<PlanoTrabalhoObjetoAssuntoViewModel>();
            request.Custos = request.Custos != null ? request.Custos : new List<PlanoTrabalhoCustoViewModel>();
            request.Reunioes = request.Reunioes != null ? request.Reunioes : new List<PlanoTrabalhoReuniaoViewModel>();

            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            var assuntosId = request.Assuntos.Select(a => a.AssuntoId);

            var custosParaAdicionar = request.Custos
                .Where(c => c.PlanoTrabalhoCustoId == null)
                .Select(c => PlanoTrabalhoCusto.Criar(c.PlanoTrabalhoId, c.Valor, c.Descricao, request.PlanoTrabalhoObjetoId));
            var idsCustos = request.Custos
                .Where(c => c.PlanoTrabalhoCustoId != null)
                .Select(c => c.PlanoTrabalhoCustoId.Value);

            var reunioesParaAdicionar = request.Reunioes
                .Where(c => c.PlanoTrabalhoReuniaoId == null)
                .Select(c => PlanoTrabalhoReuniao.Criar(c.PlanoTrabalhoId, c.Data, c.Titulo, c.Descricao, request.PlanoTrabalhoObjetoId));
            var idsReunioes = request.Reunioes
                .Where(c => c.PlanoTrabalhoReuniaoId != null)
                .Select(c => c.PlanoTrabalhoReuniaoId.Value);

            //Recupera o PlanoTrabalho do banco
            var item = await PlanoTrabalhoRepository.ObterAsync(request.PlanoTrabalhoId);

            //Altera o objeto
            item.AlterarObjeto(request.PlanoTrabalhoObjetoId, assuntosId, custosParaAdicionar, idsCustos, reunioesParaAdicionar, idsReunioes);

            //Altera o objeto no banco de dados
            PlanoTrabalhoRepository.Atualizar(item);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Plano de trabalho alterado com sucesso.");
            return result;
        }
    }
}
