using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate
{
    public class PactoTrabalhoHistorico : Entity
    {
        public Guid PactoTrabalhoHistoricoId { get; private set; }
        public Guid PactoTrabalhoId { get; private set; }
        public Int32 SituacaoId { get; private set; }
        public String Observacoes { get; private set; }
        public String ResponsavelOperacao { get; private set; }
        public DateTime DataOperacao { get; private set; }


        public PactoTrabalho PactoTrabalho { get; private set; }
        public CatalogoDominio Situacao { get; private set; }


        public PactoTrabalhoHistorico() { }        

        public static PactoTrabalhoHistorico Criar(Guid pactoTrabalhoId, int situacaoId, string responsavelOperacao, string observacoes)            
        {
            return new PactoTrabalhoHistorico()
            {
                PactoTrabalhoId = pactoTrabalhoId,
                SituacaoId = situacaoId,
                ResponsavelOperacao = responsavelOperacao,
                Observacoes = observacoes,
                DataOperacao = DateTime.Now
            };
        }
    }
}
