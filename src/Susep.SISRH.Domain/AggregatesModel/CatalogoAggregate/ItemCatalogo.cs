using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate
{
    /// <summary>
    /// Representa os tipos de atividade
    /// </summary>
    public class ItemCatalogo : Entity
    {

        public Guid ItemCatalogoId { get; private set; }
        public String Titulo { get; private set; }
        public String Descricao { get; private set; }
        public Int32 FormaCalculoTempoItemCatalogoId { get; private set; }
        public Boolean PermiteTrabalhoRemoto { get; private set; }
        public Decimal? TempoExecucaoPresencial { get; private set; }
        public Decimal? TempoExecucaoRemoto { get; private set; }
        public String Complexidade { get; private set; }
        public String DefinicaoComplexidade { get; private set; }
        public String EntregasEsperadas { get; private set; }

        public Boolean TempoExecucaoPreviamenteDefinido
        {
            get
            {
                return FormaCalculoTempoItemCatalogoId == (int)FormaCalculoTempoItemCatalogoEnum.PredefinidoPorDia ||
                       FormaCalculoTempoItemCatalogoId == (int)FormaCalculoTempoItemCatalogoEnum.PredefinidoPorTarefa;
            }
        }

        public String TituloCompletoComTempos
        {
            get
            {
                return String.Format("{0} (pres.:{1}h / rem.:{2}h)",
                    TituloCompleto,
                    TempoExecucaoPresencial,
                    TempoExecucaoRemoto);
            }
        }

        public String TituloCompleto
        {
            get
            {
                if (!String.IsNullOrEmpty(Complexidade))
                    return Titulo + " - " + Complexidade;
                return Titulo;
            }
        }

        public CatalogoDominio FormaCalculoTempoItemCatalogo { get; private set; }
        public List<CatalogoItemCatalogo> Catalogos { get; private set; }
        public List<PlanoTrabalhoAtividadeItem> PlanosTrabalhoAtividadesItens { get; private set; }
        public List<PactoTrabalhoAtividade> PactosTrabalhoAtividades { get; private set; }
        public List<ItemCatalogoAssunto> Assuntos { get; private set; }

        public ItemCatalogo() { }

        public static ItemCatalogo Criar(string titulo, string descricao, int formaCalculoTempoItemCatalogoId, bool permiteTrabalhoRemoto, decimal? tempoExecucaoPresencial, decimal? tempoExecucaoRemoto, string complexidade, string definicaoComplexidade, string entregasEsperadas)
        {
            return new ItemCatalogo()
            {
                ItemCatalogoId = Guid.NewGuid(),
                Titulo = titulo,
                Descricao = descricao,
                FormaCalculoTempoItemCatalogoId = formaCalculoTempoItemCatalogoId,
                PermiteTrabalhoRemoto = permiteTrabalhoRemoto,
                TempoExecucaoPresencial = tempoExecucaoPresencial,
                TempoExecucaoRemoto = tempoExecucaoRemoto,
                Complexidade = complexidade,
                DefinicaoComplexidade = definicaoComplexidade,
                EntregasEsperadas = entregasEsperadas,
                Assuntos = new List<ItemCatalogoAssunto>()
            };
        }


        public void Alterar(string titulo, string descricao, int formaCalculoTempoItemCatalogoId, bool permiteTrabalhoRemoto, decimal? tempoExecucaoPresencial, decimal? tempoExecucaoRemoto, string complexidade, string definicaoComplexidade, string entregasEsperadas)
        {
            Titulo = titulo;
            Descricao = descricao;
            FormaCalculoTempoItemCatalogoId = formaCalculoTempoItemCatalogoId;
            PermiteTrabalhoRemoto = permiteTrabalhoRemoto;
            TempoExecucaoPresencial = tempoExecucaoPresencial;
            TempoExecucaoRemoto = tempoExecucaoRemoto;
            Complexidade = complexidade;
            DefinicaoComplexidade = definicaoComplexidade;
            EntregasEsperadas = entregasEsperadas;
        }

        public void AdicionarAssunto(Guid assuntoId)
        {
            Assuntos.Add(ItemCatalogoAssunto.Criar(this.ItemCatalogoId, assuntoId));
        }

        public void RemoverAssunto(Guid assuntoId)
        {
            Assuntos.RemoveAll(a => a.AssuntoId == assuntoId);
        }
    }
}
