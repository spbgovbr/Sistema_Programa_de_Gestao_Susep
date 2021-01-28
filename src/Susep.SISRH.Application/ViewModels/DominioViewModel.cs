using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/corretores/viewmodels/", Name = "dominios")]
    public class DominioViewModel
    {

        [DataMember(Name = "id")]
        public Int32 Id { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

    }
}
