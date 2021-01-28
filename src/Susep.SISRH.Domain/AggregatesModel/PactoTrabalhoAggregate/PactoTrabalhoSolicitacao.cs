using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using Susep.SISRH.Domain.Enums;
using Susep.SISRH.Domain.Exceptions;
using Susep.SISRH.Domain.Helpers;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate
{
    /// <summary>
    /// Representa as atividades delegadas às pessoas dentro de um plano de trabalho
    /// </summary>
    public class PactoTrabalhoSolicitacao : Entity
    {

        public Guid PactoTrabalhoSolicitacaoId { get; private set; }
        public Guid PactoTrabalhoId { get; private set; }

        public Int32 TipoSolicitacaoId { get; private set; }

        public DateTime DataSolicitacao { get; private set; }
        public String Solicitante { get; private set; }
        public String DadosSolicitacao { get; private set; }
        public String ObservacoesSolicitante { get; private set; }

        public Boolean Analisado { get; private set; }

        public DateTime? DataAnalise { get; private set; }
        public String Analista { get; private set; }
        public Boolean? Aprovado { get; private set; }
        public String ObservacoesAnalista { get; private set; }

        public PactoTrabalho PactoTrabalho { get; private set; }
        public CatalogoDominio TipoSolicitacao { get; private set; }

        public PactoTrabalhoSolicitacao() { }

        public static PactoTrabalhoSolicitacao Criar(Guid pactoTrabalhoId, int tipoSolicitacaoId, string solicitante, string dadosSolicitacao, string observacoesSolicitante)
        {
            //Constrói a atividade do pacto de trabalho
            return new PactoTrabalhoSolicitacao()
            {
                PactoTrabalhoId = pactoTrabalhoId,
                TipoSolicitacaoId = tipoSolicitacaoId,
                DataSolicitacao = DateTime.Now,
                Solicitante = solicitante,
                DadosSolicitacao = dadosSolicitacao,
                ObservacoesSolicitante = observacoesSolicitante,
                Analisado = false
            };
        }

        public void Responder(string analista, Boolean aprovado, string observacoesAnalista)
        {
            Analisado = true;
            DataAnalise = DateTime.Now;
            Analista = analista;
            Aprovado = aprovado;
            ObservacoesAnalista = observacoesAnalista;
        }
    }
}
