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
    public class PlanoTrabalhoAtividadeCriterio : Entity
    {
        public Guid PlanoTrabalhoAtividadeCriterioId { get; private set; }
        public Guid PlanoTrabalhoAtividadeId { get; private set; }
        public Int32 CriterioId { get; private set; }

        public PlanoTrabalhoAtividade PlanoTrabalhoAtividade { get; private set; }
        public CatalogoDominio Criterio { get; private set; }

        public PlanoTrabalhoAtividadeCriterio() { }

        public static PlanoTrabalhoAtividadeCriterio Criar(Guid planoTrabalhoAtividadeId, Int32 criterioId)
        {
            return new PlanoTrabalhoAtividadeCriterio()
            {
                //PlanoTrabalhoAtividadeItemId = Guid.NewGuid(),
                PlanoTrabalhoAtividadeId = planoTrabalhoAtividadeId,
                CriterioId = criterioId
            };
        }

        
    }
}
