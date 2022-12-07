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
    public class PactoTrabalhoInformacao : Entity
    {

        public Guid PactoTrabalhoInformacaoId { get; private set; }
        public Guid PactoTrabalhoId { get; private set; }

        public DateTime? DataRegistro { get; private set; }
        public String ResponsavelRegistro { get; private set; }
        public String Informacao { get; private set; }

        public PactoTrabalho PactoTrabalho { get; private set; }

        public PactoTrabalhoInformacao() { }


        public static PactoTrabalhoInformacao Criar(string informacao, string responsavelRegistro)
        {
            return new PactoTrabalhoInformacao()
            {
                DataRegistro = DateTime.Now,
                Informacao = informacao,
                ResponsavelRegistro = responsavelRegistro
            };
        }

    }
}
