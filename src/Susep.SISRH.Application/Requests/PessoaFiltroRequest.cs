using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.Requests
{
    public class PessoaFiltroRequest : UsuarioLogadoRequest
    {

        [DataMember(Name = "nome")]
        public String Nome { get; set; }

        [DataMember(Name = "unidadeId")]
        public Int32? UnidadeId { get; set; }

        [DataMember(Name = "catalogoDominioId")]
        public Int32? CatalogoDominioId { get; set; }
      
    }
}
