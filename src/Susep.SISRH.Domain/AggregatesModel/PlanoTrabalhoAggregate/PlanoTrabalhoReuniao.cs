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
    public class PlanoTrabalhoReuniao : Entity
    {

        public Guid PlanoTrabalhoReuniaoId { get; private set; }
        public Guid PlanoTrabalhoId { get; private set; }
        public DateTime Data { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }

        public Guid? PlanoTrabalhoObjetoId { get; private set; }
        public PlanoTrabalhoObjeto PlanoTrabalhoObjeto { get; set; }

        public PlanoTrabalho PlanoTrabalho { get; private set; }


        public PlanoTrabalhoReuniao() { }

        public static PlanoTrabalhoReuniao Criar(Guid planoTrabalhoId, DateTime data, string titulo, string descricao, Guid? planoTrabalhoObjetoId = null)
        {
            //Constrói a atividade do pacto de trabalho
            return new PlanoTrabalhoReuniao()
            {
                PlanoTrabalhoId = planoTrabalhoId,
                //PlanoTrabalhoReuniaoId = Guid.NewGuid(),
                Data = data,
                Titulo = titulo,
                Descricao = descricao,
                PlanoTrabalhoObjetoId = planoTrabalhoObjetoId
            };
        }

        public void Alterar(DateTime data, string titulo, string descricao)
        {
            Data = data;
            Titulo = titulo;
            Descricao = descricao;
        }

    }
}
