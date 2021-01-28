using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel
{
    public class CatalogoDominio
    {
        private CatalogoDominio() { }

        public int CatalogoDominioId { get; private set; }
        public string Classificacao { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }

        public IList<UnidadeModalidadeExecucao> UnidadesModalidadesExecucao { get; private set; }
        public IList<PessoaModalidadeExecucao> PessoasModalidadesExecucao { get; private set; }
        public IList<ItemCatalogo> ItensCatalogo { get; private set; }
        public IList<PlanoTrabalho> PlanosTrabalho { get; private set; }
        public List<PlanoTrabalhoAtividade> PlanosTrabalhoAtividades { get; private set; }
        public IList<PlanoTrabalhoHistorico> HistoricoPlanosTrabalho { get; private set; }
        public IList<PactoTrabalho> PactosTrabalho { get; private set; }
        public IList<PactoTrabalhoHistorico> HistoricoPactosTrabalho { get; private set; }
        public IList<PactoTrabalhoAtividade> PactosTrabalhoAtividades { get; private set; }
        public IList<PactoTrabalhoSolicitacao> PactosTrabalhoSolicitacoes { get; private set; }
        public IList<PlanoTrabalhoAtividadeCandidato> PlanoTrabalhoAtividadeCandidatos { get; private set; }
        public IList<PlanoTrabalhoAtividadeCandidatoHistorico> PlanoTrabalhoAtividadeCandidatoHistoricos { get; private set; }
        public IList<PlanoTrabalhoAtividadeCriterio> CriteriosAtividadesPlanos { get; private set; }

    }
}
