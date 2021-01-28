using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/corretores/viewmodels/", Name = "dashboard")]
    public class DashboardViewModel
    {

        [DataMember(Name = "planosTrabalho")]
        public List<PlanoTrabalhoViewModel> PlanosTrabalho { get; set; }

        [DataMember(Name = "pactosTrabalho")]
        public List<PactoTrabalhoViewModel> PactosTrabalho { get; set; }

        [DataMember(Name = "solicitacoes")]
        public List<PactoTrabalhoSolicitacaoViewModel> Solicitacoes { get; set; }

        

    }
}
