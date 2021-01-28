using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;

namespace Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate
{
    /// <summary>
    /// Representa o relacionamento entre PlanoTrabalhoAtividade e Assunto
    /// </summary>
    public class PlanoTrabalhoAtividadeAssunto : Entity
    {

        public Guid PlanoTrabalhoAtividadeAssuntoId { get; private set; }
        public Guid PlanoTrabalhoAtividadeId { get; private set; }
        public Guid AssuntoId { get; private set; }

        public Assunto Assunto { get; private set; }


        public static PlanoTrabalhoAtividadeAssunto Criar(Guid planoTrabalhoAtividadeId, Guid assuntoId)
        {
            return new PlanoTrabalhoAtividadeAssunto()
            {
                PlanoTrabalhoAtividadeId = planoTrabalhoAtividadeId,
                AssuntoId = assuntoId
            };
        }

    }
}
