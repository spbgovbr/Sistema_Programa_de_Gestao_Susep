using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Susep.SISRH.Application.Options;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
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

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class CandidatarPlanoTrabalhoAtividadeCommandHandler : IRequestHandler<CandidatarPlanoTrabalhoAtividadeCommand, IActionResult>
    {
        private IPlanoTrabalhoRepository PlanoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }
        private IEmailHelper EmailHelper { get; }
        private IPessoaQuery PessoaQuery { get; }
        private IOptions<PadroesOptions> Configuration { get; }
        private IOptions<EmailOptions> EmailConfiguration { get; }

        public CandidatarPlanoTrabalhoAtividadeCommandHandler(            
            IPlanoTrabalhoRepository planoTrabalhoRepository, 
            IUnitOfWork unitOfWork,
            IEmailHelper emailHelper,
            IPessoaQuery pessoaQuery,
            IOptions<PadroesOptions> configuration,
            IOptions<EmailOptions> emailConfiguration)
        {
            PlanoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
            EmailHelper = emailHelper;
            PessoaQuery = pessoaQuery;
            Configuration = configuration;
            EmailConfiguration = emailConfiguration;
        }

        public async Task<IActionResult> Handle(CandidatarPlanoTrabalhoAtividadeCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);
            try
            {
                //Monta o objeto com os dados do catalogo
                var item = await PlanoTrabalhoRepository.ObterAsync(request.PlanoTrabalhoId);

                //Remove a atividade
                item.RegistrarCandidaturaAtividade(request.PlanoTrabalhoAtividadeId, request.UsuarioLogadoId);

                //Altera o item de catalogo no banco de dados
                PlanoTrabalhoRepository.Atualizar(item);
                UnitOfWork.Commit(false);

                //Envia email
                await EnviarEmail(request.PlanoTrabalhoAtividadeId, request.UsuarioLogadoId);

                result.Result = true;
                result.SetHttpStatusToOk("Candidatura registrada com sucesso.");
            }
            catch (SISRH.Domain.Exceptions.SISRHDomainException ex)
            {
                result.Validations = new List<string>() { ex.Message };
                result.Result = false;
                result.SetHttpStatusToBadRequest();
            }
            return result;
        }

        #region EnviarEmail

        private async Task EnviarEmail(Guid planoTrabalhoId, Int64 pessoaId)
        {
            try
            {

                if (Configuration.Value.Notificacoes == null ||
                    Configuration.Value.Notificacoes.EnviarEmail)
                {
                    //Obtém os dados do usuário logado
                    var dadosPessoa = await PessoaQuery.ObterPorChaveAsync(pessoaId);
                    if (!String.IsNullOrEmpty(dadosPessoa.Result.Email))
                    {
                        //Obtem os destinatários dos emails
                        var destinatarios = new List<string>();
                        destinatarios.Add(dadosPessoa.Result.Email);

                        //Envia os emails
                        EnviarEmail(planoTrabalhoId, Configuration.Value.Notificacoes.EmailPlanoCandidaturaRegistrada, destinatarios.ToArray());
                    }
                }
            }
            catch { }
        }

        private void EnviarEmail(Guid planoTrabalhoId, Email opcaoEmail, string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {                
                var enderecoAcesso = Configuration.Value.EnderecoPublicacaoFront.TrimEnd('/') + "/programagestao/detalhar/" + planoTrabalhoId.ToString();

                var mensagem = new StringBuilder();
                mensagem.Append(opcaoEmail.Mensagem)
                    .AppendLine().AppendLine().Append("<a href =\"").Append(enderecoAcesso).Append("\">Clique aqui</a> para acessar o programa de gestão no sistema.").AppendLine().AppendLine()
                    .AppendLine("Caso o link não funcione, copie o endereço abaixo e abra no navegador da sua preferência:")
                    .AppendLine(enderecoAcesso);

                EmailHelper.Send(
                    EmailConfiguration.Value.EmailRemetente,
                    EmailConfiguration.Value.NomeRemetente,
                    destinatarios,
                    opcaoEmail.Assunto,
                    mensagem.ToString(),
                    true);
            }
        }

        #endregion

    }
}
