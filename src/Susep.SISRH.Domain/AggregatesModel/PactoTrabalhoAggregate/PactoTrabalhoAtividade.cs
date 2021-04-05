using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using Susep.SISRH.Domain.Enums;
using Susep.SISRH.Domain.Exceptions;
using Susep.SISRH.Domain.Helpers;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate
{
    /// <summary>
    /// Representa as atividades delegadas às pessoas dentro de um plano de trabalho
    /// </summary>
    public class PactoTrabalhoAtividade : Entity
    {

        public Guid PactoTrabalhoAtividadeId { get; private set; }
        public Guid PactoTrabalhoId { get; private set; }

        public Guid ItemCatalogoId { get; private set; }
        public Int32 Quantidade { get; private set; }

        public decimal TempoPrevistoPorItem { get; private set; }
        public decimal TempoPrevistoTotal { get; private set; }
        public decimal? TempoRealizado { get; private set; }
        public decimal? TempoHomologado { get; private set; }
        
        public DateTime? DataInicio { get; private set; }
        public DateTime? DataFim { get; private set; }

        public Int32 SituacaoId { get; private set; }
        public String Descricao { get; private set; }
        public String ConsideracoesConclusao { get; private set; }

        public PactoTrabalho PactoTrabalho { get; private set; }
        public ItemCatalogo ItemCatalogo { get; private set; }
        public CatalogoDominio Situacao { get; private set; }

        public List<PactoTrabalhoAtividadeAssunto> Assuntos { get; private set; }

        public List<PactoAtividadePlanoObjeto> Objetos { get; set; }

        public decimal? Nota { get; private set; }

        public string Justificativa { get; private set; }

        public decimal? diferencaRealizadoEPrevistoEmHoras
        {
            get {
                var realizado = TempoRealizado != null ? TempoRealizado.Value : 0;

                switch (this.SituacaoId) {
                    case ((int)SituacaoAtividadePactoTrabalhoEnum.ToDo): 
                        return Decimal.Zero;
                    case ((int)SituacaoAtividadePactoTrabalhoEnum.Doing): 
                        return TempoPrevistoTotal > realizado ? Decimal.Subtract(TempoPrevistoTotal, realizado) : Decimal.Zero;
                    case ((int)SituacaoAtividadePactoTrabalhoEnum.Done): 
                        return realizado > TempoPrevistoTotal ? Decimal.Subtract(realizado, TempoPrevistoTotal) : Decimal.Zero;
                }

                return Decimal.Zero;
            }
        }
        public decimal? diferencaPrevistoParaRealizadoEmDias
        {
            get => diferencaRealizadoEPrevistoEmHoras == null || diferencaRealizadoEPrevistoEmHoras == Decimal.Zero ? 
                Decimal.Zero :
                Decimal.Divide(diferencaRealizadoEPrevistoEmHoras.Value, PactoTrabalho.Pessoa.CargaHoraria);
        }

        public PactoTrabalhoAtividade() { }

        public DateTime CalcularAjusteNoPrazo(DateTime prazoAtual, List<DateTime> diasNaoUteis)
        {
            var diasAlemDoPrevisto = this.diferencaPrevistoParaRealizadoEmDias;
            var diasAdicionados = 0;
            var novaDataFim = prazoAtual;
            while (diasAdicionados < diasAlemDoPrevisto)
            {
                novaDataFim = novaDataFim.AddDays(1);
                if (!diasNaoUteis.Any(f => f == novaDataFim) &&
                    novaDataFim.DayOfWeek != DayOfWeek.Saturday &&
                    novaDataFim.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasAdicionados++;
                }
            }
            return novaDataFim;
        }

        public static PactoTrabalhoAtividade Criar(Guid pactoTrabalhoId, Guid itemCatalogoId, int quantidade, decimal tempoPrevistoPorItem, string descricao)
        {
            //Constrói a atividade do pacto de trabalho
            return new PactoTrabalhoAtividade()
            {
                PactoTrabalhoId = pactoTrabalhoId,
                ItemCatalogoId = itemCatalogoId,
                Quantidade = quantidade,
                TempoPrevistoPorItem = tempoPrevistoPorItem,
                TempoPrevistoTotal = Decimal.Multiply(quantidade, tempoPrevistoPorItem),
                SituacaoId = (int)SituacaoAtividadePactoTrabalhoEnum.ToDo,
                Descricao = descricao,
                Assuntos = new List<PactoTrabalhoAtividadeAssunto>(),
                Objetos = new List<PactoAtividadePlanoObjeto>()
            };
        }

        public static PactoTrabalhoAtividade Criar(PactoTrabalho pactoTrabalho, Guid itemCatalogoId, int quantidade, decimal tempoPrevistoPorItem, string descricao)
        {
            var atividade = Criar(pactoTrabalho.PlanoTrabalhoId, itemCatalogoId, quantidade, tempoPrevistoPorItem, descricao);
            atividade.PactoTrabalho = pactoTrabalho;
            return atividade;
        }


        public void Alterar(Guid itemCatalogoId, int quantidade, decimal tempoPrevistoPorItem, string descricao)
        {
            ItemCatalogoId = itemCatalogoId;
            Quantidade = quantidade;
            TempoPrevistoPorItem = tempoPrevistoPorItem;
            TempoPrevistoTotal = Decimal.Multiply(quantidade, tempoPrevistoPorItem);
            Descricao = descricao;
        }

        public void AtualizarTempoPrevistoTotal(int quantideDias)
        {
            this.TempoPrevistoTotal = Decimal.Multiply(quantideDias, this.TempoPrevistoTotal);
        }

        public void AjustarTemposPrevistoEHomologadoAoTempoRealizado()
        {
            if (this.SituacaoId != (int)SituacaoAtividadePactoTrabalhoEnum.Done)
                throw new SISRHDomainException("Não é possível ajustar os tempos previsto e homologado se a atividade não estiver concluída.");
            
            this.TempoPrevistoPorItem = this.TempoRealizado.Value;

            if (this.TempoHomologado.HasValue)
                this.TempoHomologado = this.TempoRealizado.Value;
        }

        #region Ações do kanban

        /// <summary>
        /// Ação utilizada quando no quadro Kanban uma atividade que já estivesse em execução voltar para a raia TODO
        /// </summary>
        public void DefinirComoPendente()
        {
            this.SituacaoId = (int)SituacaoAtividadePactoTrabalhoEnum.ToDo;
            this.DataInicio = null;
            this.DataFim = null;
            this.TempoRealizado = null;
        }

        /// <summary>
        /// Ação utilizada quando no quadro Kanban uma atividade passar para a raia DOING (vindo da raia TODO ou da raia DONE)
        /// </summary>
        public void DefinirComoEmExecucao()
        {
            this.SituacaoId = (int)SituacaoAtividadePactoTrabalhoEnum.Doing;
            if (!this.DataInicio.HasValue) this.DataInicio = DateTime.Now;
            this.DataFim = null;
            this.TempoRealizado = null;
        }

        /// <summary>
        /// Ação utilizada quando no quadro Kanban uma atividade passar para a raia DONE
        /// </summary>
        public void DefinirComoConcluida(List<DateTime> feriados, int cargaHoraria)
        {
            //Só muda se tiver utilizado o Kanban para registrar o início da execução
            if (this.SituacaoId != (int)SituacaoAtividadePactoTrabalhoEnum.Doing)
                throw new SISRHDomainException("Não é possível alterar para concluída uma atividade que ainda não está em execução");

            this.SituacaoId = (int)SituacaoAtividadePactoTrabalhoEnum.Done;
            this.DataFim = DateTime.Now;
            this.TempoRealizado = Decimal.Round(CalcularTempoRealizado(feriados, cargaHoraria), 1);
        }

        /// <summary>
        /// Calcula o tempo gasto entre a data de início e a data fim
        /// </summary>
        /// <returns></returns>
        private decimal CalcularTempoRealizado(List<DateTime> feriados, int cargaHoraria)
        {
            if (this.ItemCatalogo.FormaCalculoTempoItemCatalogoId == (int)FormaCalculoTempoItemCatalogoEnum.PredefinidoPorDia)
            {
                return this.TempoPrevistoTotal;
            }
            else
            {
                return WorkingDays.DiffTime(this.DataInicio.Value, this.DataFim.Value, feriados, cargaHoraria);
            }
        }

        #endregion

        #region Ações para o usuário que não quer usar o kanban

        /// <summary>
        /// Permite que o usuário informe "manualmente" os marcos de execução de uma atividade
        /// </summary>
        /// <param name="situacaoId"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataConclusao"></param>
        /// <param name="tempoRealizado"></param>
        public void AlterarAndamento(Int32 situacaoId, DateTime? dataInicio, DateTime? dataConclusao, decimal? tempoRealizado, string consideracoes = null, Boolean ignorarValidacoes = false)
        {
            this.SituacaoId = situacaoId;
            this.DataInicio = null;
            this.DataFim = null;
            this.TempoRealizado = null;
            this.ConsideracoesConclusao = consideracoes;

            switch (this.SituacaoId)
            {
                case (int)SituacaoAtividadePactoTrabalhoEnum.Doing:
                    if (!dataInicio.HasValue && !ignorarValidacoes)
                        throw new SISRHDomainException("A data de início é obrigatória");

                    this.DataInicio = dataInicio;
                    break;
                case (int)SituacaoAtividadePactoTrabalhoEnum.Done:
                    if (!dataInicio.HasValue && !ignorarValidacoes)
                        throw new SISRHDomainException("A data de início é obrigatória");
                    if (!dataConclusao.HasValue && !ignorarValidacoes)
                        throw new SISRHDomainException("A data de conclusão é obrigatória");
                    if (!tempoRealizado.HasValue && !ignorarValidacoes)
                        throw new SISRHDomainException("É obrigatório informar o tempo gasto para concluir a atividade");

                    this.DataInicio = dataInicio;
                    this.DataFim = dataConclusao;
                    this.TempoRealizado = tempoRealizado;
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Permite que o usuário informe "manualmente" o tempo que levou para exeutar uma atividade que já concluiu
        /// </summary>
        /// <param name="tempoRealizado"></param>
        public void AlterarTempoRealizado(decimal tempoRealizado)
        {
            //Só permite usar esse método se antes tivesse definido a data de início
            if (this.SituacaoId != (int)SituacaoAtividadePactoTrabalhoEnum.Done &&
                this.ItemCatalogo.FormaCalculoTempoItemCatalogoId != (int)FormaCalculoTempoItemCatalogoEnum.PredefinidoPorDia)
            {
                throw new SISRHDomainException("Não é possível alterar o tempo realizado de uma atividade que ainda não está concluída");
            }

            this.TempoRealizado = tempoRealizado;
        }

        public void Avaliar(int nota, string justificativa)
        {
            if (nota < 5)
            {
                if (String.IsNullOrWhiteSpace(justificativa))
                    throw new SISRHDomainException("Se a nota for inferior a 5, a justificativa é obrigatória.");

                this.TempoHomologado = 0;
            } else
            {
                this.TempoHomologado = this.TempoPrevistoPorItem;
            }

            this.Nota = nota;
            this.Justificativa = justificativa;

        }

        public void AdicionarAssunto(Guid assuntoId)
        {
            Assuntos.Add(PactoTrabalhoAtividadeAssunto.Criar(this.PactoTrabalhoAtividadeId, assuntoId));
        }

        public void RemoverAssunto(Guid assuntoId)
        {
            Assuntos.RemoveAll(a => a.AssuntoId == assuntoId);
        }

        public void AdicionarObjeto(Guid planoTrabalhoObjetoId)
        {
            Objetos.Add(PactoAtividadePlanoObjeto.Criar(this.PactoTrabalhoAtividadeId, planoTrabalhoObjetoId));
        }

        public void RemoverObjeto(Guid planoTrabalhoObjetoId)
        {
            Objetos.RemoveAll(o => o.PlanoTrabalhoObjetoId == planoTrabalhoObjetoId);
        }

    }
}
