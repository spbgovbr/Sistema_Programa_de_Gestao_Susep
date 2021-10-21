using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Application.Options
{
    public class PadroesOptions
    {

        public int TempoComparecimento { get; set; }
        public string TermoAceite { get; set; }

        public string EnderecoPublicacaoFront { get; set; }

        public Notificacao Notificacoes { get; set; }

    }

    public class Notificacao
    {

        public Boolean EnviarEmail { get; set; }
        public string AberturaFaseHabilitacao { get; set; }


        public Email EmailPlanoParaAprovacao { get; set; }
        public Email EmailPlanoAprovado { get; set; }
        public Email EmailPlanoRejeitado { get; set; }
        public Email EmailPlanoCandidaturaRegistrada { get; set; }
        public Email EmailPlanoEmHabilitacao { get; set; }
        public Email EmailPlanoCandidaturaAprovada { get; set; }
        public Email EmailPlanoCandidaturaRejeitada { get; set; }
        public Email EmailPactoSituacaoAlterada { get; set; }
        public Email EmailPactoSolicitacaoAlteracaoPrazo { get; set; }
        public Email EmailPactoSolicitacaoNovaAtividade { get; set; }
        public Email EmailPactoSolicitacaoExclusaoAtividade { get; set; }
        public Email EmailPactoSolicitacaoJustificativaEstouroPrazo { get; set; }
        public Email EmailPactoSolicitacaoAnalisada { get; set; }
        public Email EmailPactoAtividadeAvaliada { get; set; }

    }

    public class Email
    {
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
    }


}
