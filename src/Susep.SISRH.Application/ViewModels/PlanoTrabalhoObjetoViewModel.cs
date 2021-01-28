using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoObjeto")]
    public class PlanoTrabalhoObjetoViewModel
    {

        [DataMember(Name = "planoTrabalhoObjetoId")]
        public Guid PlanoTrabalhoObjetoId { get; set; }

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "objetoId")]
        public Guid ObjetoId { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "chave")]
        public string Chave { get; set; }

        [DataMember(Name = "tipo")]
        public Int32 Tipo { get; set; }

        [DataMember(Name = "associadoAtividadePlano")]
        public bool AssociadoAtividadePlano { get; set; }

        [DataMember(Name = "custos")]
        public IEnumerable<PlanoTrabalhoCustoViewModel> Custos { get; set; }

        [DataMember(Name = "assuntos")]
        public IEnumerable<PlanoTrabalhoObjetoAssuntoViewModel> Assuntos { get; set; }

        [DataMember(Name = "reunioes")]
        public IEnumerable<PlanoTrabalhoReuniaoViewModel> Reunioes { get; set; }

    }

    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoObjeto")]
    public class PlanoTrabalhoObjetoAssuntoViewModel
    {
        [DataMember(Name = "planoTrabalhoObjetoAssuntoId")]
        public Guid PlanoTrabalhoObjetoAssuntoId { get; set; }

        [DataMember(Name = "planoTrabalhoObjetoId")]
        public Guid PlanoTrabalhoObjetoId { get; set; }

        [DataMember(Name = "assuntoId")]
        public Guid AssuntoId { get; set; }

        [DataMember(Name = "valor")]
        public string Valor { get; set; }

        [DataMember(Name = "hierarquia")]
        public string Hierarquia { get; set; }
    }

    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoObjeto")]
    public class PlanoTrabalhoObjetoPactoAtividadeViewModel
    {
        [DataMember(Name = "planoTrabalhoObjetoId")]
        public Guid PlanoTrabalhoObjetoId { get; set; }

        [DataMember(Name = "objetoId")]
        public Guid ObjetoId { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "associado")]
        public bool Associado { get; set; }

        [DataMember(Name = "assuntos")]
        public List<PlanoTrabalhoObjetoAssuntoViewModel> Assuntos { get; set; }

        
    }


}
