using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;
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
    /// Representa um plano de trabalho de uma unidade / setor
    /// </summary>
    public class PlanoTrabalho : Entity
    {

        public Guid PlanoTrabalhoId { get; private set; }
        public Int64 UnidadeId { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public Int32 SituacaoId { get; private set; }
        public Int32 PrazoComparecimento { get; private set; }
        public Int32 TotalServidoresSetor { get; private set; }
        public Int32 TempoFaseHabilitacao { get; private set; }       

        public string TermoAceite { get; private set; }

        public Unidade Unidade { get; private set; }
        public CatalogoDominio Situacao { get; private set; }

        public List<PlanoTrabalhoAtividade> Atividades { get; private set; }
        public List<PlanoTrabalhoMeta> Metas { get; private set; }
        public List<PlanoTrabalhoReuniao> Reunioes { get; private set; }
        public List<PlanoTrabalhoCusto> Custos { get; private set; }
        public List<PlanoTrabalhoHistorico> Historico { get; private set; }
        public List<PlanoTrabalhoObjeto> Objetos { get; private set; }
        public List<PactoTrabalho> PactosTrabalho { get; private set; }


        public PlanoTrabalho() { }

        public static PlanoTrabalho Criar(Int64 unidadeId, DateTime dataInicio, DateTime dataFim, int prazoComparecimento, int tempoFaseHabilitacao, int totalServidoresSetor, String usuarioLogado, string termoAceite)
        {
            //Cria o pacto de trabalho para ser ajustado pelo chefe da unidade
            var model = new PlanoTrabalho()
            {
                PlanoTrabalhoId = Guid.NewGuid(),
                UnidadeId = unidadeId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                PrazoComparecimento = prazoComparecimento,
                TempoFaseHabilitacao = tempoFaseHabilitacao,
                TotalServidoresSetor = totalServidoresSetor,
                SituacaoId = (int)SituacaoPlanoTrabalhoEnum.Rascunho,
                Historico = new List<PlanoTrabalhoHistorico>(),
                TermoAceite = termoAceite
            };

            model.ValidarDatas();

            model.AlterarSituacao((int)SituacaoPlanoTrabalhoEnum.Rascunho, usuarioLogado, null);

            return model;
        }

        public void AlterarPrazo(DateTime dataInicio, DateTime dataFim)
        {
            VerificarPossibilidadeAlteracao();
            DataInicio = dataInicio;
            DataFim = dataFim;
            this.ValidarDatas();
        }

        #region Atividades

        public void AdicionarAtividade(Int32 modalidadeExecucaoId, Int32 quantidadeColaboradores, string descricao, IEnumerable<Guid> itensCatalogo, IEnumerable<Int32> criterios, IEnumerable<Guid> idsAssuntos)
        {
            VerificarPossibilidadeAlteracaoAtividades();
            var atividade = PlanoTrabalhoAtividade.Criar(this.PlanoTrabalhoId, modalidadeExecucaoId, quantidadeColaboradores, descricao, itensCatalogo, criterios);
            if (idsAssuntos != null)
            {
                foreach (var idAssunto in idsAssuntos)
                {
                    atividade.AdicionarAssunto(idAssunto);
                }
            }
            this.Atividades.Add(atividade);
            VerificarQuantidadeServidores();
        }

        public void AlterarAtividade(Guid planoTrabalhoAtividadeId, Int32 modalidadeExecucaoId, Int32 quantidadeColaboradores, string descricao, IEnumerable<Guid> itensCatalogo, IEnumerable<Int32> criterios, IEnumerable<Guid> idsAssuntos)
        {
            VerificarPossibilidadeAlteracaoAtividades();
            var atividade = this.Atividades.FirstOrDefault(r => r.PlanoTrabalhoAtividadeId == planoTrabalhoAtividadeId);
            atividade.Alterar(modalidadeExecucaoId, quantidadeColaboradores, descricao, itensCatalogo, criterios);
            if (idsAssuntos != null)
            {
                var idsAssuntosARemover = atividade.Assuntos.Select(a => a.AssuntoId).Where(id => !idsAssuntos.Contains(id)).ToList();
                var idsAssuntosAIncluir = idsAssuntos.Where(id => !atividade.Assuntos.Select(a => a.AssuntoId).Contains(id)).ToList();
                foreach (var id in idsAssuntosARemover) { atividade.RemoverAssunto(id); }
                foreach (var id in idsAssuntosAIncluir) { atividade.AdicionarAssunto(id); }
            }

            VerificarQuantidadeServidores();
        }

        private void VerificarQuantidadeServidores()
        {
            var totalCadastrado = this.Atividades.Sum(a => a.QuantidadeColaboradores);
            if (totalCadastrado > this.TotalServidoresSetor)
                throw new SISRHDomainException("Não é possível cadastrar atividades para mais servidores do que a quantidade total da unidade");
        }

        public void RemoverAtividade(Guid planoTrabalhoAtividadeId)
        {
            VerificarPossibilidadeAlteracaoAtividades();
            this.Atividades.RemoveAll(r => r.PlanoTrabalhoAtividadeId == planoTrabalhoAtividadeId);
        }

        public void RegistrarCandidaturaAtividade(Guid planoTrabalhoAtividadeId, Int64 pessoaId)
        {            
            var atividade = this.Atividades.FirstOrDefault(r => r.PlanoTrabalhoAtividadeId == planoTrabalhoAtividadeId);
            atividade.RegistrarCandidatura(pessoaId, this.TermoAceite);
        }

        public void AtualizarCandidaturaAtividade(Guid planoTrabalhoAtividadeId, Guid planoTrabalhoAtividadeCandidatoId, Int32 situacaoId, string responsavelOperacao, string descricao)
        {
            var atividade = this.Atividades.FirstOrDefault(r => r.PlanoTrabalhoAtividadeId == planoTrabalhoAtividadeId);
            var candidatura = atividade.AtualizarCandidatura(planoTrabalhoAtividadeCandidatoId, situacaoId, responsavelOperacao, descricao);

            //Se aprovar a candidatura do usuário nesse plano, deve rejeitar as demais
            if (situacaoId == (int)SituacaoCandidaturaPlanoTrabalhoEnum.Aprovada)
            {
                this.Atividades.ForEach(a => a.Candidatos.ForEach(c =>
                {
                    if (c.PessoaId == candidatura.PessoaId && 
                        c.PlanoTrabalhoAtividadeCandidatoId != candidatura.PlanoTrabalhoAtividadeCandidatoId)
                    {
                        a.AtualizarCandidatura(c.PlanoTrabalhoAtividadeCandidatoId, (int)SituacaoCandidaturaPlanoTrabalhoEnum.Rejeitada, responsavelOperacao, "Aprovado em outra vaga");
                    }
                }));
            }
        }


        #endregion

        #region Metas

        public void AdicionarMeta(string meta, string indicador, string descricao)
        {
            VerificarPossibilidadeAlteracao();
            this.Metas.Add(PlanoTrabalhoMeta.Criar(this.PlanoTrabalhoId, meta, indicador, descricao));
        }

        public void AlterarMeta(Guid planoTrabalhoMetaId, string meta, string indicador, string descricao)
        {
            VerificarPossibilidadeAlteracao();
            var model = this.Metas.FirstOrDefault(r => r.PlanoTrabalhoMetaId == planoTrabalhoMetaId);
            model.Alterar(meta, indicador, descricao);
        }

        public void RemoverMeta(Guid planoTrabalhoMetaId)
        {
            VerificarPossibilidadeAlteracao();
            this.Metas.RemoveAll(r => r.PlanoTrabalhoMetaId == planoTrabalhoMetaId);
        }

        #endregion

        #region Reuniões

        public void AdicionarReuniao(DateTime data, string titulo, string descricao)
        {
            VerificarPossibilidadeAlteracao();
            this.Reunioes.Add(PlanoTrabalhoReuniao.Criar(this.PlanoTrabalhoId, data, titulo, descricao));
        }

        public void AlterarReuniao(Guid planoTrabalhoReuniaoId, DateTime data, string titulo, string descricao)
        {
            VerificarPossibilidadeAlteracao();
            var atividade = this.Reunioes.FirstOrDefault(r => r.PlanoTrabalhoReuniaoId == planoTrabalhoReuniaoId);
            atividade.Alterar(data, titulo, descricao);
        }

        public void RemoverReuniao(Guid planoTrabalhoReuniaoId)
        {
            VerificarPossibilidadeAlteracao();
            this.Reunioes.RemoveAll(r => r.PlanoTrabalhoReuniaoId == planoTrabalhoReuniaoId);
        }

        #endregion

        #region Custos

        public void AdicionarCusto(Decimal valor, string descricao)
        {
            VerificarPossibilidadeAlteracao();
            this.Custos.Add(PlanoTrabalhoCusto.Criar(this.PlanoTrabalhoId, valor, descricao));
        }

        public void AlterarCusto(Guid planoTrabalhoCustoId, Decimal valor, string descricao)
        {
            VerificarPossibilidadeAlteracao();
            var custo = this.Custos.FirstOrDefault(r => r.PlanoTrabalhoCustoId == planoTrabalhoCustoId);
            custo.Alterar(valor, descricao);
        }

        public void RemoverCusto(Guid planoTrabalhoCustoId)
        {
            VerificarPossibilidadeAlteracao();
            this.Custos.RemoveAll(r => r.PlanoTrabalhoCustoId == planoTrabalhoCustoId);
        }

        #endregion

        #region Objeto

        public void AdicionarObjeto(Guid objetoId, List<PlanoTrabalhoReuniao> reunioes, List<PlanoTrabalhoCusto> custos, List<PlanoTrabalhoObjetoAssunto> assuntos)
        {
            VerificarPossibilidadeAlteracao();

            // Criar objeto 
            var objeto = PlanoTrabalhoObjeto.Criar(this.PlanoTrabalhoId, objetoId);

            // Popular as listas de assuntos, custos e reuniões do objeto
            assuntos.ForEach(a => objeto.AdicionarAssunto(a.AssuntoId));
            reunioes.ForEach(r => objeto.AdicionarReuniao(this.PlanoTrabalhoId, r.Data, r.Titulo, r.Descricao));
            custos.ForEach(c => objeto.AdicionarCusto(this.PlanoTrabalhoId, c.Valor, c.Descricao));

            // Popular as listas de custos e reuniões do PlanoTrabalho
            objeto.Reunioes.ForEach(r => this.Reunioes.Add(r));
            objeto.Custos.ForEach(c => this.Custos.Add(c));

            // Adicionar o objeto à lista do PlanoTrabalho
            this.Objetos.Add(objeto);

            VerificarQuantidadeServidores();
        }

        public void AlterarObjeto(Guid planoTrabalhoObjetoId, IEnumerable<Guid> idsAssuntos, IEnumerable<PlanoTrabalhoCusto> custosParaIncluir, IEnumerable<Guid> idsCustos, IEnumerable<PlanoTrabalhoReuniao> reunioesParaIncluir, IEnumerable<Guid> idsReunioes)
        {
            VerificarPossibilidadeAlteracao();

            var objeto = this.Objetos.FirstOrDefault(r => r.PlanoTrabalhoObjetoId == planoTrabalhoObjetoId);

            // Remover custos do PlanoTrabalho
            var idsCustosExclusao = objeto.Custos.Select(c => c.PlanoTrabalhoCustoId).Where(id => !idsCustos.Contains(id)).ToList();
            idsCustosExclusao.ForEach(id => this.RemoverCusto(id));

            // Remover reuniões do PlanoTrabalho
            var idsReunioesExclusao = objeto.Reunioes.Select(r => r.PlanoTrabalhoReuniaoId).Where(id => !idsReunioes.Contains(id)).ToList();
            idsReunioesExclusao.ForEach(id => this.RemoverReuniao(id));

            objeto.Alterar(idsAssuntos, custosParaIncluir, idsCustosExclusao, reunioesParaIncluir, idsReunioesExclusao);

            VerificarQuantidadeServidores();
        }


        public void RemoverObjeto(Guid planoTrabalhoObjetoId)
        {
            VerificarPossibilidadeAlteracao();
            this.Objetos.RemoveAll(r => r.PlanoTrabalhoObjetoId == planoTrabalhoObjetoId);
        }

        #endregion

        public void AlterarSituacao(Int32 situacaoId, String responsavelOperacaoId, String observacoes, Boolean deserto = false)
        {
            if (!PodeAlteracaoSituacao(situacaoId, deserto))
                throw new SISRHDomainException("A situação atual do programa de gestão não permite mudar para o estado solicitado");

            if (deserto)
                observacoes = "O Programa de Gestão foi considerado deserto, pois ninguém se candidatou às vagas disponibilizadas";

            this.SituacaoId = situacaoId;
            this.Historico.Add(PlanoTrabalhoHistorico.Criar(this.PlanoTrabalhoId, this.SituacaoId, responsavelOperacaoId, observacoes));
        }

        public void ConcluirHabilitacao(String responsavelOperacaoId, String observacoes, string[] aprovados)
        {
            //Atualiza as candidaturas
            foreach (var atividade in this.Atividades)
            {
                foreach (var candidatura in atividade.Candidatos)
                {
                    var situacaoCandidatura = (int)SituacaoCandidaturaPlanoTrabalhoEnum.Rejeitada;
                    Guid candidatoId;
                    if (aprovados.Any(candidatoAprovadoId =>
                            !String.IsNullOrEmpty(candidatoAprovadoId) &&
                            Guid.TryParse(candidatoAprovadoId, out candidatoId) &&
                            candidatura.PlanoTrabalhoAtividadeCandidatoId == Guid.Parse(candidatoAprovadoId)))
                    {
                        situacaoCandidatura = (int)SituacaoCandidaturaPlanoTrabalhoEnum.Aprovada;
                    }

                    candidatura.AlterarSituacao(situacaoCandidatura, responsavelOperacaoId, observacoes);
                }
            }
            
            //Altera a situação
            this.AlterarSituacao((int)SituacaoPlanoTrabalhoEnum.EmExecucao, responsavelOperacaoId, observacoes);
        }

        private Boolean PodeAlteracaoSituacao(Int32 situacaoId, Boolean deserto)
        {
            if (situacaoId == (int)SituacaoPlanoTrabalhoEnum.Rascunho)
                return true;

            switch (this.SituacaoId)
            {
                case (int)SituacaoPlanoTrabalhoEnum.Rascunho:
                    return situacaoId == (int)SituacaoPlanoTrabalhoEnum.Habilitacao;
                //case (int)SituacaoPlanoTrabalhoEnum.EnviadoAprovacao:
                //    return situacaoId == (int)SituacaoPlanoTrabalhoEnum.Aprovado || 
                //           situacaoId == (int)SituacaoPlanoTrabalhoEnum.Rejeitado;
                //case (int)SituacaoPlanoTrabalhoEnum.Rejeitado:
                //    return situacaoId == (int)SituacaoPlanoTrabalhoEnum.Rascunho;
                //case (int)SituacaoPlanoTrabalhoEnum.AprovadoIndicadores:
                //    return situacaoId == (int)SituacaoPlanoTrabalhoEnum.AprovadoGestaoPessoas ||
                //           situacaoId == (int)SituacaoPlanoTrabalhoEnum.Rejeitado;
                //case (int)SituacaoPlanoTrabalhoEnum.AprovadoGestaoPessoas:
                //    return situacaoId == (int)SituacaoPlanoTrabalhoEnum.Aprovado ||
                //           situacaoId == (int)SituacaoPlanoTrabalhoEnum.Rejeitado;
                //case (int)SituacaoPlanoTrabalhoEnum.Aprovado:
                //    return situacaoId == (int)SituacaoPlanoTrabalhoEnum.Habilitacao;
                case (int)SituacaoPlanoTrabalhoEnum.Habilitacao:
                    return situacaoId == (int)SituacaoPlanoTrabalhoEnum.EmExecucao ||
                           (deserto && situacaoId == (int)SituacaoPlanoTrabalhoEnum.Concluido);
                //case (int)SituacaoPlanoTrabalhoEnum.ProntoParaExecucao:
                //    return situacaoId == (int)SituacaoPlanoTrabalhoEnum.EmExecucao;
                case (int)SituacaoPlanoTrabalhoEnum.EmExecucao:
                    //Só permite alterar o plano para executado se todos os pactos tiverem sido executados
                    if (this.PactosTrabalho.Any(p => p.SituacaoId != (int)SituacaoPactoTrabalhoEnum.Executado))
                        throw new SISRHDomainException("Não é possível encerrar o programa de gestão enquanto houver planos em execução");

                    return situacaoId == (int)SituacaoPlanoTrabalhoEnum.Executado;
                case (int)SituacaoPlanoTrabalhoEnum.Executado:

                    return situacaoId == (int)SituacaoPlanoTrabalhoEnum.Concluido;
            }

            return true;
        }
        private void VerificarPossibilidadeAlteracaoAtividades()
        {
            if (SituacaoId != (int)SituacaoPlanoTrabalhoEnum.Rascunho)
                throw new SISRHDomainException("As atividades de um programa de gestão só podem ser alteradas enquanto estiver na situação rascunho");
        }

        private void VerificarPossibilidadeAlteracao()
        {
            if (SituacaoId == (int)SituacaoPlanoTrabalhoEnum.Executado ||
                SituacaoId == (int)SituacaoPlanoTrabalhoEnum.Concluido)
                throw new SISRHDomainException("O programa de gestão só pode ser alterado enquanto estiver na situação rascunho");
        }

        private void ValidarDatas()
        {

            if (DataInicio < DateTime.Now.Date)
                throw new SISRHDomainException("A data de início do programa de gestão deve ser maior ou igual à data atual");

            if (DataFim <= DataInicio)
                throw new SISRHDomainException("A data de fim do programa de gestão deve ser maior que a data de início");

        }

    }
}
