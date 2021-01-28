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
    public class PlanoTrabalhoAtividadeItem : Entity
    {
        public Guid PlanoTrabalhoAtividadeItemId { get; private set; }
        public Guid PlanoTrabalhoAtividadeId { get; private set; }
        public Guid ItemCatalogoId { get; private set; }

        public PlanoTrabalhoAtividade PlanoTrabalhoAtividade { get; private set; }
        public ItemCatalogo ItemCatalogo { get; private set; }

        public PlanoTrabalhoAtividadeItem() { }

        public static PlanoTrabalhoAtividadeItem Criar(Guid planoTrabalhoAtividadeId, Guid itemCatalogoId)
        {
            return new PlanoTrabalhoAtividadeItem()
            {
                //PlanoTrabalhoAtividadeItemId = Guid.NewGuid(),
                PlanoTrabalhoAtividadeId = planoTrabalhoAtividadeId,
                ItemCatalogoId = itemCatalogoId
            };
        }

        
    }
}
