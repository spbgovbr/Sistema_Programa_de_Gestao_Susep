using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.Requests
{
    public class UsuarioLogadoRequest : BaseActionRequest
    {
        //private String _usuarioLogado = null;
        //[DataMember(Name = "usuarioLogado")]
        //public String UsuarioLogado {
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_usuarioLogado))
        //        {
        //            if (this.JwtSecurityToken != null)
        //            {
        //                var login = this.JwtSecurityToken.Claims.FirstOrDefault(c => c.Type.ToUpper() == "LOGIN");
        //                if (login != null)
        //                    _usuarioLogado = login.Value;
        //            }
        //        }

        //        return _usuarioLogado;
        //    }
        //}

        private Int64? _usuarioLogadoId = null;
        [DataMember(Name = "usuarioLogadoId")]
        public Int64 UsuarioLogadoId {
            get
            {
                if (!_usuarioLogadoId.HasValue)
                {
                    if (this.JwtSecurityToken != null)
                    {
                        var sisRhId = this.JwtSecurityToken.Claims.FirstOrDefault(c => c.Type.ToUpper() == "SUB"); 
                        if (sisRhId != null)
                            _usuarioLogadoId = Int64.Parse(sisRhId.Value);
                    }
                }

                return _usuarioLogadoId.Value;
            }
        }

    }
}
