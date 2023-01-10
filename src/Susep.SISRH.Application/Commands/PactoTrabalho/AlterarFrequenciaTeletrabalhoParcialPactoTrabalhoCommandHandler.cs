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
    public class AlterarFrequenciaTeletrabalhoParcialPactoTrabalhoCommandHandler : IRequestHandler<AlterarFrequenciaTeletrabalhoParcialPactoTrabalhoCommand, IActionResult>
    {
        private IPessoaQuery PessoaQuery { get; }
        private IPessoaRepository PessoaRepository { get; }
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public AlterarFrequenciaTeletrabalhoParcialPactoTrabalhoCommandHandler(
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

        public async Task<IActionResult> Handle(AlterarFrequenciaTeletrabalhoParcialPactoTrabalhoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            try
            {
                //Monta o objeto com os dados do catalogo
                var item = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

                //Altera a situação
                item.AlterarFrequenciaTeletrabalhoParcial(request.TipoFrequenciaTeletrabalhoParcialId, request.QuantidadeDiasFrequenciaTeletrabalhoParcial);

                //Altera o item de catalogo no banco de dados
                PactoTrabalhoRepository.Atualizar(item);
                UnitOfWork.Commit(false);

                result.Result = true;
                result.SetHttpStatusToOk("Plano de trabalho alterado com sucesso.");
            }
            catch (SISRH.Domain.Exceptions.SISRHDomainException ex)
            {
                result.Validations = new List<string>() { ex.Message };
                result.Result = false;
                result.SetHttpStatusToBadRequest();
            }
            return result;
        }
    }
}
