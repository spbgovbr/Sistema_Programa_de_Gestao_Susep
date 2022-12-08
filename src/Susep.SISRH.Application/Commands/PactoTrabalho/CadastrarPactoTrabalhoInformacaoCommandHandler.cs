using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Susep.SISRH.Application.Options;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.ViewModels;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using SUSEP.Framework.Utils.Abstractions;
using SUSEP.Framework.Utils.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class CadastrarPactoTrabalhoInformacaoCommandHandler : IRequestHandler<CadastrarPactoTrabalhoInformacaoCommand, IActionResult>
    {
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public CadastrarPactoTrabalhoInformacaoCommandHandler(
            IPactoTrabalhoRepository planoTrabalhoRepository,
            IUnitOfWork unitOfWork)
        {
            PactoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(CadastrarPactoTrabalhoInformacaoCommand request, CancellationToken cancellationToken)
        {
            var result = new ApplicationResult<PactoTrabalhoInformacaoViewModel>(request);
            try
            {
                //Monta os dados do pacto de trabalho
                var pacto = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

                //Registra a informação no pacto
                var informacao = pacto.RegistrarInformacao(request.Informacao, request.UsuarioLogadoId.ToString());

                //Altera o pacto de trabalho no banco de dados
                PactoTrabalhoRepository.Atualizar(pacto);
                UnitOfWork.Commit(false);

                result.Result = new PactoTrabalhoInformacaoViewModel() { 
                    Informacao = informacao.Informacao, 
                    ResponsavelRegistro = informacao.ResponsavelRegistro 
                };

                result.SetHttpStatusToOk("Informação registrada com sucesso.");
            }
            catch (System.Exception ex)
            {
                result.Validations = new List<string>() { ex.Message };
                result.SetHttpStatusToBadRequest();
            }
            return result;
        }        

    }
}
