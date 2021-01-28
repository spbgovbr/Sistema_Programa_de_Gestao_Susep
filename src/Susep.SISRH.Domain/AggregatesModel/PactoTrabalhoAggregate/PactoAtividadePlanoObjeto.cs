using SUSEP.Framework.SeedWorks.Domains;
using System;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate
{
    /// <summary>
    /// Representa a associação entre objetos de um PlanoTrabalho e atividades de um PactoTrabalho
    /// </summary>
    public class PactoAtividadePlanoObjeto : Entity
    {

        public Guid PactoAtividadePlanoObjetoId { get; private set; }
        public Guid PactoTrabalhoAtividadeId { get; private set; }
        public Guid PlanoTrabalhoObjetoId { get; private set; }

        public PactoTrabalhoAtividade PactoTrabalhoAtividade { get; private set; }
        public PlanoTrabalhoObjeto PlanoTrabalhoObjeto { get; private set; }

        public PactoAtividadePlanoObjeto() { }

        public static PactoAtividadePlanoObjeto Criar(Guid pactoTrabalhoAtividadeId, Guid planoTrabalhoObjetoId)
        {
            //Constrói a atividade do pacto de trabalho
            return new PactoAtividadePlanoObjeto()
            {
                PactoTrabalhoAtividadeId = pactoTrabalhoAtividadeId,
                PlanoTrabalhoObjetoId = planoTrabalhoObjetoId
            };
        }
    }
}
