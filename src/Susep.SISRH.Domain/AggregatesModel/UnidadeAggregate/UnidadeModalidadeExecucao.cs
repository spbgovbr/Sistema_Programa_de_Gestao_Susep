using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate
{
    /// <summary>
    /// Representa as formas de trabalho que podem ser executadas pelas unidades/setores
    /// </summary>
    public class UnidadeModalidadeExecucao : Entity
    {

        public Guid UnidadeModalidadeExecucaoId { get; private set; }

        public Int64 UnidadeId { get; private set; }
        public Int32 ModalidadeExecucaoId { get; private set; }

        public Unidade Unidade { get; private set; }
        public CatalogoDominio ModalidadeExecucao { get; private set; }

    }
}
