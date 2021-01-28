using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate
{
    /// <summary>
    /// Representa as atividades previstas para serem realizadas em um plano de trabalho de uma unidade / setor
    /// </summary>
    public class PlanoTrabalhoAtividadeCandidatoHistorico : Entity
    {
        public Guid PlanoTrabalhoAtividadeCandidatoHistoricoId { get; private set; }
        public Guid PlanoTrabalhoAtividadeCandidatoId { get; private set; }
        public Int32 SituacaoId { get; private set; }
        public DateTime Data { get; private set; }
        public String Descricao { get; private set; }
        public String ResponsavelOperacao { get; private set; }        

        public PlanoTrabalhoAtividadeCandidato PlanoTrabalhoAtividadeCandidato { get; private set; }
        public CatalogoDominio Situacao { get; private set; }

        public PlanoTrabalhoAtividadeCandidatoHistorico() { }

        public static PlanoTrabalhoAtividadeCandidatoHistorico Criar(Guid planoTrabalhoAtividadeCandidatoId, Int32 situacaoId, string responsavelOperacao, string descricao = null)
        {
            return new PlanoTrabalhoAtividadeCandidatoHistorico()
            {
                PlanoTrabalhoAtividadeCandidatoId = planoTrabalhoAtividadeCandidatoId,
                SituacaoId = situacaoId,
                ResponsavelOperacao = responsavelOperacao,
                Data = DateTime.Now,
                Descricao = descricao
            };
        }

        
    }
}
