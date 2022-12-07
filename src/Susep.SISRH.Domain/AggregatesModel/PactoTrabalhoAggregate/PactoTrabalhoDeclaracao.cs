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
    public class PactoTrabalhoDeclaracao : Entity
    {

        public Guid PactoTrabalhoDeclaracaoId { get; private set; }
        public Guid PactoTrabalhoId { get; private set; }

        public Int32 DeclaracaoId { get; private set; }

        public DateTime DataExibicao { get; private set; }
        public Boolean Aceita { get; private set; }
        public String ResponsavelRegistro { get; private set; }
        public DateTime DataRegistro { get; private set; }


        public CatalogoDominio Declaracao { get; private set; }
        public PactoTrabalho PactoTrabalho { get; private set; }


        public PactoTrabalhoDeclaracao() { }

        public static PactoTrabalhoDeclaracao Criar(int declaracaoId, string responsavelRegistro)
        {
            //Constrói a atividade do pacto de trabalho
            return new PactoTrabalhoDeclaracao()
            {
                DataExibicao = DateTime.Now,
                DeclaracaoId = declaracaoId,
                ResponsavelRegistro = responsavelRegistro,
                DataRegistro = DateTime.Now
            };
        }

        public void RegistrarAceite(string responsavelRegistro)
        {
            Aceita = true;
            ResponsavelRegistro = responsavelRegistro;
            DataRegistro = DateTime.Now;
        }

    }
}
