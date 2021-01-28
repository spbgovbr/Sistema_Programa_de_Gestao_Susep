using SUSEP.Framework.SeedWorks.Domains;
using System;

namespace Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate
{
    /// <summary>
    /// Representa os custos de um plano de trabalho de uma unidade / setor
    /// </summary>
    public class PlanoTrabalhoCusto : Entity
    {

        public Guid PlanoTrabalhoCustoId { get; private set; }
        public Guid PlanoTrabalhoId { get; private set; }
        public Decimal Valor { get; private set; }
        public string Descricao { get; private set; }

        public Guid? PlanoTrabalhoObjetoId { get; private set; }
        public PlanoTrabalhoObjeto PlanoTrabalhoObjeto { get; set; }

        public PlanoTrabalho PlanoTrabalho { get; private set; }


        public PlanoTrabalhoCusto() { }

        public static PlanoTrabalhoCusto Criar(Guid planoTrabalhoId, Decimal valor, string descricao, Guid? planoTrabalhoObjetoId = null)
        {
            //Constroi o custo do plano de trabalho
            return new PlanoTrabalhoCusto()
            {
                PlanoTrabalhoId = planoTrabalhoId,
                //PlanoTrabalhoCustoId = Guid.NewGuid(),
                Valor = valor,
                Descricao = descricao,
                PlanoTrabalhoObjetoId = planoTrabalhoObjetoId
            };
        }

        public void Alterar(Decimal valor, string descricao)
        {
            Valor = valor;
            Descricao = descricao;
        }

    }
}
