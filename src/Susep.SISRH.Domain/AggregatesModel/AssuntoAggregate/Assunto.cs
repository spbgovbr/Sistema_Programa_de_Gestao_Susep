using SUSEP.Framework.SeedWorks.Domains;
using System;

namespace Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate
{
    /// <summary>
    /// Representa os assuntos
    /// </summary>
    public class Assunto : Entity
    {
        public Guid AssuntoId { get; private set; }
        public String Valor { get; private set; }
        public Guid? AssuntoPaiId { get; private set; }
        public Assunto AssuntoPai { get; private set; }
        public bool Ativo { get; private set; }

        public static Assunto Criar(String valor, Guid? assuntoPaiId, bool ativo)
        {
            return new Assunto()
            {
                AssuntoId = Guid.NewGuid(),
                Valor = valor,
                AssuntoPaiId = assuntoPaiId,
                Ativo = ativo
            };
        }

        public void Alterar(String valor, Guid? assuntoPaiId, bool ativo)
        {
            this.Valor = valor;
            this.AssuntoPaiId = assuntoPaiId;
            this.Ativo = ativo;
        }
    }
}
