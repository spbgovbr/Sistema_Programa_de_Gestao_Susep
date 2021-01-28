using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate
{
    /// <summary>
    /// Representa as atividades previstas para serem realizadas em um plano de trabalho de uma unidade / setor
    /// </summary>
    public class PlanoTrabalhoAtividadeCandidato : Entity
    {
        public Guid PlanoTrabalhoAtividadeCandidatoId { get; private set; }
        public Guid PlanoTrabalhoAtividadeId { get; private set; }
        public Int64 PessoaId { get; private set; }
        public Int32 SituacaoId { get; private set; }
        public string TermoAceite { get; set; }

        public PlanoTrabalhoAtividade PlanoTrabalhoAtividade { get; private set; }
        public Pessoa Pessoa { get; private set; }
        public CatalogoDominio Situacao { get; private set; }
        public List<PlanoTrabalhoAtividadeCandidatoHistorico> Historico { get; private set; }

        public PlanoTrabalhoAtividadeCandidato() { }

        public static PlanoTrabalhoAtividadeCandidato Criar(Guid planoTrabalhoAtividadeId, Int64 pessoaId, string termoAceite)
        {
            return new PlanoTrabalhoAtividadeCandidato()
            {
                //PlanoTrabalhoAtividadeCandidatoId = Guid.NewGuid(),
                PlanoTrabalhoAtividadeId = planoTrabalhoAtividadeId,
                SituacaoId = (int)SituacaoCandidaturaPlanoTrabalhoEnum.Solicitada,
                PessoaId = pessoaId,
                TermoAceite = termoAceite
            };
        }

        public void AlterarSituacao(Int32 situacaoId, string responsavelOperacao, string descricao)
        {
            this.SituacaoId = situacaoId;
            this.Historico.Add(PlanoTrabalhoAtividadeCandidatoHistorico.Criar(PlanoTrabalhoAtividadeCandidatoId, this.SituacaoId, responsavelOperacao, descricao));
        }

    }
}
