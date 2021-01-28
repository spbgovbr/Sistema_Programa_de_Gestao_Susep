using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Domain.Enums
{
    public enum SituacaoPlanoTrabalhoEnum
    {
        Rascunho = 301,
        EnviadoAprovacao = 302,
        Rejeitado = 303,
        AprovadoIndicadores = 304,
        AprovadoGestaoPessoas = 305,
        Aprovado = 306,
        Habilitacao = 307,
        ProntoParaExecucao = 308,
        EmExecucao = 309,
        Executado = 310,
        Concluido = 311
    }
}
