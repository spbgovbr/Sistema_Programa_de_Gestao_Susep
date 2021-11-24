using Castle.Core.Configuration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Susep.SISRH.Application.Options;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using Susep.SISRH.Domain.Exceptions;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class CadastrarPlanoTrabalhoCommandHandler : IRequestHandler<CadastrarPlanoTrabalhoCommand, IActionResult>
    {
        private IPlanoTrabalhoRepository PlanoTrabalhoRepository { get; }
        private IUnidadeQuery UnidadeQuery { get; }
        private IUnitOfWork UnitOfWork { get; }
        private IOptions<PadroesOptions> Configuration { get; }

        public CadastrarPlanoTrabalhoCommandHandler(
            IPlanoTrabalhoRepository planoTrabalhoRepository,
            IUnidadeQuery unidadeQuery,
            IUnitOfWork unitOfWork,
            IOptions<PadroesOptions> configuration)
        {
            PlanoTrabalhoRepository = planoTrabalhoRepository;
            UnidadeQuery = unidadeQuery;
            UnitOfWork = unitOfWork;
            Configuration = configuration;
        }

        public async Task<IActionResult> Handle(CadastrarPlanoTrabalhoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<Guid> result = new ApplicationResult<Guid>(request);

            try
            {
                var unidade = await UnidadeQuery.ObterQuantidadeServidoresPorChaveAsync(request.UnidadeId);

                var tempoComparecimento = request.TempoComparecimento;
                var termosAceite = request.TermoAceite;

                if (Configuration.Value.TempoComparecimento > 0)
                    tempoComparecimento = Configuration.Value.TempoComparecimento;

                if (!String.IsNullOrEmpty(Configuration.Value.TermoAceite))
                    termosAceite = Configuration.Value.TermoAceite;

                //Monta o objeto com os dados do item de catalogo
                Domain.AggregatesModel.PlanoTrabalhoAggregate.PlanoTrabalho item =
                    Domain.AggregatesModel.PlanoTrabalhoAggregate.PlanoTrabalho.Criar(
                        request.UnidadeId,
                        request.DataInicio,
                        request.DataFim,
                        tempoComparecimento,
                        request.TempoFaseHabilitacao,
                        unidade.Result.QuantidadeServidores,
                        request.UsuarioLogadoId.ToString(),
                        termosAceite);

                //Adiciona o catalogo no banco de dados
                await PlanoTrabalhoRepository.AdicionarAsync(item);
                UnitOfWork.Commit(false);

                result.Result = item.PlanoTrabalhoId;
                result.SetHttpStatusToOk("Plano de trabalho cadastrado com sucesso.");
            }
            catch (SISRHDomainException ex)
            {
                result.Validations = new List<string>() { ex.Message };
                result.SetHttpStatusToBadRequest(ex.Message);
            }

            return result;
        }
    }
}
