using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;

namespace Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate
{
    /// <summary>
    /// Relaciona os PlanoTrabalhoObjeto a Assunto
    /// </summary>
    public class PlanoTrabalhoObjetoAssunto : Entity
    {

        public Guid PlanoTrabalhoObjetoAssuntoId { get; private set; }
        public Guid PlanoTrabalhoObjetoId { get; private set; }
        public Guid AssuntoId { get; private set; }
        public PlanoTrabalhoObjeto PlanoTrabalhoObjeto { get; private set; }
        public Assunto Assunto { get; private set; }

        public PlanoTrabalhoObjetoAssunto() { }

        public static PlanoTrabalhoObjetoAssunto Criar(Guid planoTrabalhoObjetoId, Guid assuntoId)
        {
            return new PlanoTrabalhoObjetoAssunto()
            {
                PlanoTrabalhoObjetoId = planoTrabalhoObjetoId,
                AssuntoId = assuntoId
            };
        }

    }
}
