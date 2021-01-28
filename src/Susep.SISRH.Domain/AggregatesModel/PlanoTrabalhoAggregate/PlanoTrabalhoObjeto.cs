using Susep.SISRH.Domain.AggregatesModel.ObjetoAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate
{
    /// <summary>
    /// Relaciona os objetos a um plano de trabalho
    /// </summary>
    public class PlanoTrabalhoObjeto : Entity
    {

        public Guid PlanoTrabalhoObjetoId { get; private set; }
        public Guid PlanoTrabalhoId { get; private set; }
        public Guid ObjetoId { get; private set; }
        public PlanoTrabalho PlanoTrabalho { get; private set; }
        public Objeto Objeto { get; private set; }

        public List<PlanoTrabalhoReuniao> Reunioes { get; private set; }
        public List<PlanoTrabalhoCusto> Custos { get; private set; }
        public List<PlanoTrabalhoObjetoAssunto> Assuntos { get; private set; }

        public PlanoTrabalhoObjeto() { }

        public static PlanoTrabalhoObjeto Criar(Guid planoTrabalhoId, Guid objetoId)
        {
            return new PlanoTrabalhoObjeto()
            {
                //PlanoTrabalhoAtividadeId = Guid.NewGuid(),
                PlanoTrabalhoId = planoTrabalhoId,
                ObjetoId = objetoId,
                Reunioes = new List<PlanoTrabalhoReuniao>(),
                Custos = new List<PlanoTrabalhoCusto>(),
                Assuntos = new List<PlanoTrabalhoObjetoAssunto>()
            };
        }

        public void Alterar(IEnumerable<Guid> idsAssuntos, IEnumerable<PlanoTrabalhoCusto> custosParaIncluir, IEnumerable<Guid> idsCustosExclusao, IEnumerable<PlanoTrabalhoReuniao> reunioesParaIncluir, IEnumerable<Guid> idsReunioesExclusao)
        {
            var idsAssuntosBanco = this.Assuntos.Select(a => a.AssuntoId);
            var idsAssuntosExclusao = idsAssuntosBanco.Where(id => !idsAssuntos.Contains(id)).ToList();
            idsAssuntosExclusao.ForEach(id => this.RemoverAssunto(id));
            var idsAssuntosInclusao = idsAssuntos.Where(id => !idsAssuntosBanco.Contains(id)).ToList();
            idsAssuntosInclusao.ForEach(a => this.AdicionarAssunto(a));

            idsCustosExclusao.ToList().ForEach(id => this.RemoverCusto(id));
            custosParaIncluir.ToList().ForEach(c => this.AdicionarCusto(this.PlanoTrabalhoId, c.Valor, c.Descricao));

            idsReunioesExclusao.ToList().ForEach(id => this.RemoverReuniao(id));
            reunioesParaIncluir.ToList().ForEach(r => this.AdicionarReuniao(this.PlanoTrabalhoId, r.Data, r.Titulo, r.Descricao));
        }

        public void AdicionarCusto(Guid planoTrabalhoId, decimal valor, string descricao)
        {
            var custo = PlanoTrabalhoCusto.Criar(planoTrabalhoId, valor, descricao, this.PlanoTrabalhoObjetoId);
            this.Custos.Add(custo);
        }

        public void RemoverCusto(Guid planoTrabalhoCustoId)
        {
            var custo = this.Custos.Where(c => c.PlanoTrabalhoCustoId == planoTrabalhoCustoId);
            this.Custos.RemoveAll(c => c.PlanoTrabalhoCustoId == planoTrabalhoCustoId);
        }

        public void AdicionarReuniao(Guid planoTrabalhoId, DateTime data, string titulo, string descricao)
        {
            var reuniao = PlanoTrabalhoReuniao.Criar(planoTrabalhoId, data, titulo, descricao, this.PlanoTrabalhoObjetoId);
            this.Reunioes.Add(reuniao);
        }

        public void RemoverReuniao(Guid planoTrabalhoReuniaoId)
        {
            this.Reunioes.RemoveAll(c => c.PlanoTrabalhoReuniaoId == planoTrabalhoReuniaoId);
        }

        public void AdicionarAssunto(Guid assuntoId)
        {
            this.Assuntos.Add(PlanoTrabalhoObjetoAssunto.Criar(this.PlanoTrabalhoObjetoId, assuntoId));
        }

        public void RemoverAssunto(Guid assuntoId)
        {
            this.Assuntos.RemoveAll(a => a.AssuntoId == assuntoId);
        }
    }
}
