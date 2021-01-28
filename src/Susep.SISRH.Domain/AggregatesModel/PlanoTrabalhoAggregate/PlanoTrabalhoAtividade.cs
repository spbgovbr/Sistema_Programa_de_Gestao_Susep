using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.Enums;
using Susep.SISRH.Domain.Exceptions;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate
{
    /// <summary>
    /// Representa as atividades previstas para serem realizadas em um plano de trabalho de uma unidade / setor
    /// </summary>
    public class PlanoTrabalhoAtividade : Entity
    {

        public Guid PlanoTrabalhoAtividadeId { get; private set; }
        public Guid PlanoTrabalhoId { get; private set; }
        public Int32 ModalidadeExecucaoId { get; private set; }
        public Int32 QuantidadeColaboradores { get; private set; }
        public String Descricao { get; private set; }

        public List<PlanoTrabalhoAtividadeItem> ItensCatalogo { get; private set; }
        public List<PlanoTrabalhoAtividadeCriterio> Criterios { get; private set; }
        public List<PlanoTrabalhoAtividadeCandidato> Candidatos { get; private set; }

        public List<PlanoTrabalhoAtividadeAssunto> Assuntos { get; private set; }

        public PlanoTrabalho PlanoTrabalho { get; private set; }
        public CatalogoDominio ModalidadeExecucao { get; private set; }

        public PlanoTrabalhoAtividade() { }

        public static PlanoTrabalhoAtividade Criar(Guid planoTrabalhoId, Int32 modalidadeExecucaoId, Int32 quantidadeColaboradores, string descricao, IEnumerable<Guid> itensCatalogo, IEnumerable<Int32> criterios)
        {
            //Constrói a atividade do pacto de trabalho
            var model = new PlanoTrabalhoAtividade()
            {
                //PlanoTrabalhoAtividadeId = Guid.NewGuid(),
                PlanoTrabalhoId = planoTrabalhoId,
                ModalidadeExecucaoId = modalidadeExecucaoId,
                QuantidadeColaboradores = quantidadeColaboradores,
                Descricao = descricao,
                Assuntos = new List<PlanoTrabalhoAtividadeAssunto>()
            };
            
            model.ItensCatalogo = itensCatalogo.Select(i => PlanoTrabalhoAtividadeItem.Criar(model.PlanoTrabalhoAtividadeId, i)).ToList();
            model.Criterios = criterios.Select(i => PlanoTrabalhoAtividadeCriterio.Criar(model.PlanoTrabalhoAtividadeId, i)).ToList();

            return model;
        }

        public void Alterar(Int32 modalidadeExecucaoId, Int32 quantidadeColaboradores, string descricao, IEnumerable<Guid> itensCatalogo, IEnumerable<Int32> criterios)
        {
            ModalidadeExecucaoId = modalidadeExecucaoId;
            QuantidadeColaboradores = quantidadeColaboradores;
            Descricao = descricao;
            ItensCatalogo = itensCatalogo.Select(i => PlanoTrabalhoAtividadeItem.Criar(this.PlanoTrabalhoAtividadeId, i)).ToList();
            Criterios = criterios.Select(i => PlanoTrabalhoAtividadeCriterio.Criar(this.PlanoTrabalhoAtividadeId, i)).ToList();
        }

        public void RegistrarCandidatura(Int64 pessoaId, string termoAceite)
        {
            this.Candidatos.Add(PlanoTrabalhoAtividadeCandidato.Criar(this.PlanoTrabalhoAtividadeId, pessoaId, termoAceite));
        }

        public PlanoTrabalhoAtividadeCandidato AtualizarCandidatura(Guid planoTrabalhoAtividadeCandidatoId, Int32 situacaoId, string responsavelOperacao, string descricao)
        {
            var candidato = this.Candidatos.FirstOrDefault(r => r.PlanoTrabalhoAtividadeCandidatoId == planoTrabalhoAtividadeCandidatoId);
            candidato.AlterarSituacao(situacaoId, responsavelOperacao, descricao);
            return candidato;
        }

        public void AdicionarAssunto(Guid assuntoId)
        {
            Assuntos.Add(PlanoTrabalhoAtividadeAssunto.Criar(this.PlanoTrabalhoAtividadeId, assuntoId));
        }

        public void RemoverAssunto(Guid assuntoId)
        {
            Assuntos.RemoveAll(a => a.AssuntoId == assuntoId);
        }

    }
}
