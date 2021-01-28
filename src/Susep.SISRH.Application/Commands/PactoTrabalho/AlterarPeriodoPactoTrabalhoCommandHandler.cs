using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class AlterarPeriodoPactoTrabalhoCommandHandler : IRequestHandler<AlterarPeriodoPactoTrabalhoCommand, IActionResult>
    {
        private IPessoaQuery PessoaQuery { get; }
        private IPessoaRepository PessoaRepository { get; }
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public AlterarPeriodoPactoTrabalhoCommandHandler(
            IPessoaQuery pessoaQuery,
            IPessoaRepository pessoaRepository,
            IPactoTrabalhoRepository planoTrabalhoRepository, 
            IUnitOfWork unitOfWork)
        {
            PessoaQuery = pessoaQuery;
            PessoaRepository = pessoaRepository;
            PactoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(AlterarPeriodoPactoTrabalhoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            //Monta o objeto com os dados do catalogo
            var item = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

            //Obtém os dias não úteis da pessoa
            var diasNaoUteis = await PessoaQuery.ObterDiasNaoUteisAsync(item.PessoaId, item.DataInicio, item.DataFim);
            item.DiasNaoUteis = diasNaoUteis.Result.ToList();

            //Verifica se a pessoa tem outros pactos cadastrados para o período
            var dadosPessoa = await PessoaRepository.ObterAsync(item.PessoaId);

            if (dadosPessoa.PactosTrabalho.Any(p => 
                    p.PactoTrabalhoId != item.PactoTrabalhoId && 
                    p.DataFim >= request.DataInicio && 
                    p.DataInicio <= request.DataFim))
            {
                result.Validations = new List<string> { "A pessoa já tem um plano de trabalho cadastrado para o período" };
                return result;
            }

            //Altera a situação
            item.AlterarPeriodo(request.DataInicio, request.DataFim);

            //Altera o item de catalogo no banco de dados
            PactoTrabalhoRepository.Atualizar(item);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Plano de trabalho alterado com sucesso.");
            return result;
        }
    }
}
