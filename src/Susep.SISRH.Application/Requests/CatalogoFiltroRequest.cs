using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.Requests
{
    public class CatalogoFiltroRequest : UsuarioLogadoRequest
    {

        [DataMember(Name = "unidadeId")]
        public Int32? UnidadeId { get; set; }

    }
}
