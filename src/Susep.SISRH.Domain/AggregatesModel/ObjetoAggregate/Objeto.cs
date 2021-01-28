using SUSEP.Framework.SeedWorks.Domains;
using System;

namespace Susep.SISRH.Domain.AggregatesModel.ObjetoAggregate
{
    /// <summary>
    /// Representa os objetos
    /// </summary>
    public class Objeto : Entity
    {
        public Guid ObjetoId { get; private set; }
        public String Chave { get; private set; }
        public String Descricao { get; private set; }
        public Int32 Tipo { get; private set; }
        public bool Ativo { get; private set; }

        public static Objeto Criar(String chave, String descricao, Int32 tipo)
        {
            return new Objeto()
            {
                ObjetoId = Guid.NewGuid(),
                Chave = tratarChave(chave),
                Descricao = descricao,
                Tipo = tipo,
                Ativo = true
            };
        }

        public void Alterar(String chave, String descricao, Int32 tipo, bool ativo)
        {
            Chave = tratarChave(chave);
            Descricao = descricao;
            Tipo = tipo;
            Ativo = ativo;
        }

        private static String tratarChave(String chave)
        {
            if (chave != null)
            {
                if (chave.Trim().Length == 0)
                    return null;

                return chave.Trim().ToUpper();
            }

            return chave;
        }
    }
}
