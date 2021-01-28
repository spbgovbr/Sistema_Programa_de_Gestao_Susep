using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;

namespace Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate
{
    /// <summary>
    /// Representa o relacionamento entre PactoTrabalhoAtividade e Assunto
    /// </summary>
    public class PactoTrabalhoAtividadeAssunto : Entity
    {

        public Guid PactoTrabalhoAtividadeAssuntoId { get; private set; }
        public Guid PactoTrabalhoAtividadeId { get; private set; }
        public Guid AssuntoId { get; private set; }

        public Assunto Assunto { get; private set; }


        public static PactoTrabalhoAtividadeAssunto Criar(Guid pactoTrabalhoAtividadeId, Guid assuntoId)
        {
            return new PactoTrabalhoAtividadeAssunto()
            {
                PactoTrabalhoAtividadeId = pactoTrabalhoAtividadeId,
                AssuntoId = assuntoId
            };
        }

    }
}
