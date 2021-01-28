using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.Enums;
using Susep.SISRH.Domain.Exceptions;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate
{
    /// <summary>
    /// Representa as atividades previstas para serem realizadas em um plano de trabalho de uma unidade / setor
    /// </summary>
    public class PlanoTrabalhoMeta : Entity
    {

        public Guid PlanoTrabalhoMetaId { get; private set; }
        public Guid PlanoTrabalhoId { get; private set; }
        public string Meta { get; private set; }
        public string Indicador { get; private set; }
        public string Descricao { get; private set; }


        public PlanoTrabalho PlanoTrabalho { get; private set; }


        public PlanoTrabalhoMeta() { }

        public static PlanoTrabalhoMeta Criar(Guid planoTrabalhoId, string meta, string indicador, string descricao)
        {
            //Constrói a atividade do pacto de trabalho
            return new PlanoTrabalhoMeta()
            {
                PlanoTrabalhoId = planoTrabalhoId,
                //PlanoTrabalhoMetaId = Guid.NewGuid(),
                Meta = meta,
                Indicador = indicador,
                Descricao = descricao
            };
        }

        public void Alterar(string meta, string indicador, string descricao)
        {
            Meta = meta;
            Indicador = indicador;
            Descricao = descricao;
        }

    }
}
