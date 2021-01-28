using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate
{
    /// <summary>
    /// Representa as unidades/setores
    /// </summary>
    public class Unidade : Entity
    {

        public Int64 UnidadeId { get; private set; }
        public String Nome { get; private set; }
        public String Sigla { get; private set; }
        public String SiglaCompleta { get; private set; }
        public String UfId { get; private set; }
        public IEnumerable<UnidadeModalidadeExecucao> ModalidadesExecucao { get; private set; }

        public IEnumerable<Catalogo> Catalogos { get; private set; }
        public IEnumerable<PlanoTrabalho> PlanosTrabalho { get; private set; }
        public IEnumerable<PactoTrabalho> PactosTrabalho { get; private set; }
    }
}
