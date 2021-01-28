using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.ViewModels;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class CadastrarPlanoTrabalhoObjetoCommandHandler : IRequestHandler<CadastrarPlanoTrabalhoObjetoCommand, IActionResult>
    {
        private IPlanoTrabalhoRepository PlanoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public CadastrarPlanoTrabalhoObjetoCommandHandler(
            IPlanoTrabalhoRepository planoTrabalhoRepository,
            IUnitOfWork unitOfWork)
        {
            PlanoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(CadastrarPlanoTrabalhoObjetoCommand request, CancellationToken cancellationToken)
        {
            request.Assuntos = request.Assuntos != null ? request.Assuntos : new List<PlanoTrabalhoObjetoAssuntoViewModel>();
            request.Custos = request.Custos != null ? request.Custos : new List<PlanoTrabalhoCustoViewModel>();
            request.Reunioes = request.Reunioes != null ? request.Reunioes : new List<PlanoTrabalhoReuniaoViewModel>();

            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            //Monta o objeto com os dados do catalogo
            var item = await PlanoTrabalhoRepository.ObterAsync(request.PlanoTrabalhoId);

            var custos = request.Custos.Select(c => PlanoTrabalhoCusto.Criar(c.PlanoTrabalhoId, c.Valor, c.Descricao)).ToList();
            var reunioes = request.Reunioes.Select(r => PlanoTrabalhoReuniao.Criar(r.PlanoTrabalhoId, r.Data, r.Titulo, r.Descricao)).ToList();
            var assuntos = request.Assuntos.Select(a => PlanoTrabalhoObjetoAssunto.Criar(a.PlanoTrabalhoObjetoId, a.AssuntoId)).ToList();

            //Adiciona o objeto
            item.AdicionarObjeto(request.ObjetoId, reunioes, custos, assuntos);

            //Altera o item de catalogo no banco de dados
            PlanoTrabalhoRepository.Atualizar(item);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Plano de trabalho alterado com sucesso.");
            return result;
        }
    }
}
