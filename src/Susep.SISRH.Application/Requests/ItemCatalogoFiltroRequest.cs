using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.Requests
{
    public class ItemCatalogoFiltroRequest : UsuarioLogadoRequest
    {
        [DataMember(Name = "titulo")]
        public String Titulo { get; set; }

        [DataMember(Name = "formaCalculoTempoId")]
        public Int32? FormaCalculoTempoId { get; set; }

        [DataMember(Name = "permiteTrabalhoRemoto")]
        public Boolean? PermiteTrabalhoRemoto { get; set; }
    }
}
