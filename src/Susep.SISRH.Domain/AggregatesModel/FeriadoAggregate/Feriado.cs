using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.FeriadoAggregate
{
    /// <summary>
    /// Representa as pessoas 
    /// </summary>
    public class Feriado : Entity
    {

        public Int64 FeriadoId { get; private set; }
        public DateTime Data { get; private set; }
        public Boolean Fixo { get; private set; }
        public String Descricao { get; private set; }
        public String UfId { get; private set; }

    }
}
