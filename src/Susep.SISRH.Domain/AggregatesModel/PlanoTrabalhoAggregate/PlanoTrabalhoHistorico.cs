using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate
{
    /// <summary>
    /// Representa o histórico de um plano de trabalho de uma unidade / setor
    /// </summary>
    public class PlanoTrabalhoHistorico : Entity
    {

        public Guid PlanoTrabalhoHistoricoId { get; private set; }
        public Guid PlanoTrabalhoId { get; private set; }
        public Int32 SituacaoId { get; private set; }
        public String Observacoes { get; private set; }
        public String ResponsavelOperacao { get; private set; }
        public DateTime DataOperacao { get; private set; }


        public PlanoTrabalho PlanoTrabalho { get; private set; }
        public CatalogoDominio Situacao { get; private set; }

        public PlanoTrabalhoHistorico() { }

        public static PlanoTrabalhoHistorico Criar(Guid planoTrabalhoId, int situacaoId, string responsavelOperacao, string observacoes)
        {
            return new PlanoTrabalhoHistorico()
            {
                //PlanoTrabalhoHistoricoId = Guid.NewGuid(),
                PlanoTrabalhoId = planoTrabalhoId,
                DataOperacao = DateTime.Now,
                SituacaoId = situacaoId,
                ResponsavelOperacao = responsavelOperacao,
                Observacoes = observacoes
            };
        }

    }
}
