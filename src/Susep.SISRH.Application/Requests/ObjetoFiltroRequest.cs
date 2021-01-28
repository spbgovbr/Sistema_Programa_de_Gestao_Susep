using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.Requests
{
    public class ObjetoFiltroRequest : UsuarioLogadoRequest
    {

        [DataMember(Name = "chave")]
        public String Chave { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

        [DataMember(Name = "tipo")]
        public Int64? Tipo { get; set; }

    }
}
