using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PactoTrabalhoAtividadeAssunto")]
    public class PactoTrabalhoAtividadeAssuntoViewModel
    {
        [DataMember(Name = "pactoTrabalhoAtividadeId")]
        public Guid? PactoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "itemCatalogoId")]
        public Guid ItemCatalogoId { get; set; }

        [DataMember(Name = "assuntoId")]
        public Guid AssuntoId { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }
    }

    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PactoTrabalhoAtividadeAssunto")]
    public class PactoTrabalhoAssuntosParaAssociarViewModel
    {
        [DataMember(Name = "todosOsAssuntosParaAssociar")]
        public IEnumerable<PactoTrabalhoAtividadeAssuntoViewModel> TodosOsAssuntosParaAssociar { get; set; }

        [DataMember(Name = "assuntosAssociados")]
        public IEnumerable<PactoTrabalhoAtividadeAssuntoViewModel> AssuntosAssociados { get; set; }
    }
}
