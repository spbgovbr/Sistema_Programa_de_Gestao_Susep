using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.PessoaAggregate
{
    /// <summary>
    /// Representa as formas de trabalho que podem ser executadas pela pessoa
    /// </summary>
    public class PessoaModalidadeExecucao : Entity
    {

        public Guid PessoaModalidadeExecucaoId { get; private set; }

        public Int64 PessoaId { get; private set; }
        public Int32 ModalidadeExecucaoId { get; private set; }

        public Pessoa Pessoa { get; private set; }
        public CatalogoDominio ModalidadeExecucao { get; private set; }

    }
}
