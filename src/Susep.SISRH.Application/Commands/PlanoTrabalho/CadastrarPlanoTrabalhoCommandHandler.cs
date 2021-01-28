using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class CadastrarPlanoTrabalhoCommandHandler : IRequestHandler<CadastrarPlanoTrabalhoCommand, IActionResult>
    {     
        private IPlanoTrabalhoRepository PlanoTrabalhoRepository { get; }
        private IUnidadeQuery UnidadeQuery { get; }
        private IUnitOfWork UnitOfWork { get; }

        public CadastrarPlanoTrabalhoCommandHandler(
            IPlanoTrabalhoRepository catalogoRepository, 
            IUnidadeQuery unidadeQuery,
            IUnitOfWork unitOfWork)
        {
            PlanoTrabalhoRepository = catalogoRepository;
            UnidadeQuery = unidadeQuery;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(CadastrarPlanoTrabalhoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<Guid> result = new ApplicationResult<Guid>(request);

            var unidade = await UnidadeQuery.ObterPorChaveAsync(request.UnidadeId);

            //Monta o objeto com os dados do item de catalogo
            Domain.AggregatesModel.PlanoTrabalhoAggregate.PlanoTrabalho item =
                Domain.AggregatesModel.PlanoTrabalhoAggregate.PlanoTrabalho.Criar(
                    request.UnidadeId,
                    request.DataInicio,
                    request.DataFim,
                    request.TempoComparecimento,
                    request.TempoFaseHabilitacao,
                    unidade.Result.QuantidadeServidores,
                    request.UsuarioLogadoId.ToString(),
                    request.TermoAceite);

            //Adiciona o catalogo no banco de dados
            await PlanoTrabalhoRepository.AdicionarAsync(item);
            UnitOfWork.Commit(false);

            result.Result = item.PlanoTrabalhoId;
            result.SetHttpStatusToOk("Plano de trabalho cadastrado com sucesso.");
            return result;
        }
    }
}
