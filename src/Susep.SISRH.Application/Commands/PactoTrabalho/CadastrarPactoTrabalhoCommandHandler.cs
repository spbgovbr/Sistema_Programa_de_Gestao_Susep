using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using Susep.SISRH.Domain.Exceptions;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class CadastrarPactoTrabalhoCommandHandler : IRequestHandler<CadastrarPactoTrabalhoCommand, IActionResult>
    {
        private IPessoaRepository PessoaRepository { get; }
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IPessoaQuery PessoaQuery { get; }
        private IUnitOfWork UnitOfWork { get; }

        public CadastrarPactoTrabalhoCommandHandler(
            IPessoaRepository pessoaRepository,
            IPactoTrabalhoRepository catalogoRepository,
            IPessoaQuery pessoaQuery,
            IUnitOfWork unitOfWork)
        {
            PessoaRepository = pessoaRepository;
            PactoTrabalhoRepository = catalogoRepository;
            PessoaQuery = pessoaQuery;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(CadastrarPactoTrabalhoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<Guid> result = new ApplicationResult<Guid>(request);

            try
            {
                var dadosPessoa = await PessoaRepository.ObterAsync(request.PessoaId);

                if (dadosPessoa.PactosTrabalho.Any(p => p.DataFim >= request.DataInicio.Date && p.DataInicio <= request.DataFim.Date))
                {
                    result.Validations = new List<string> { "A pessoa já tem um plano de trabalho cadastrado para o período" };
                    return result;
                }

                //Obtém os dias que não são úteis (feriados, férias, etc)
                var diasNaoUteis = await PessoaQuery.ObterDiasNaoUteisAsync(dadosPessoa.PessoaId, request.DataInicio, request.DataFim);

                //Monta o objeto com os dados do item de catalogo
                Domain.AggregatesModel.PactoTrabalhoAggregate.PactoTrabalho item =
                    Domain.AggregatesModel.PactoTrabalhoAggregate.PactoTrabalho.Criar(
                        request.PlanoTrabalhoId,
                        request.UnidadeId,
                        request.PessoaId,
                        dadosPessoa.CargaHoraria,
                        request.FormaExecucaoId,
                        request.DataInicio,
                        request.DataFim,
                        request.UsuarioLogadoId.ToString(),
                        diasNaoUteis.Result.ToList(),
                        request.TermoAceite);

                //Verifica se está criando ou copiando um pacto de trabalho
                if (request.PactoTrabalhoId.HasValue)
                {
                    //Obtém os dados do pacto que está copiando
                    var pactoCopiar = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId.Value);

                    //Adiciona ao pacto novo as atividades do que está sendo copiado
                    foreach (var atividade in pactoCopiar.Atividades)
                    {
                        var modalidadeExecucaoId = atividade.ModalidadeExecucaoId.HasValue ? atividade.ModalidadeExecucaoId.Value : pactoCopiar.ModalidadeExecucaoId;
                        var assuntosId = atividade.Assuntos.Select(a => a.AssuntoId);
                        var objetosId = atividade.Objetos.Select(o => o.PlanoTrabalhoObjetoId);
                        item.AdicionarAtividade(atividade.ItemCatalogo, atividade.Quantidade, modalidadeExecucaoId, atividade.TempoPrevistoPorItem, atividade.Descricao, assuntosId, objetosId);
                    }
                }

                //Adiciona o catalogo no banco de dados
                await PactoTrabalhoRepository.AdicionarAsync(item);
                UnitOfWork.Commit(false);

                result.Result = item.PactoTrabalhoId;
                result.SetHttpStatusToOk("Plano de trabalho cadastrado com sucesso.");
            }
            catch (SISRH.Domain.Exceptions.SISRHDomainException ex)
            {
                result.Validations = new List<string>() { ex.Message };
                result.SetHttpStatusToBadRequest();
            }
            return result;
        }
    }
}
