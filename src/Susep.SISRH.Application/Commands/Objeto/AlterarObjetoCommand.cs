using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.Assunto
{
    public class AlterarObjetoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "objetoId")]
        public Guid ObjetoId { get; set; }

        [DataMember(Name = "chave")]
        public String Chave { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

        [DataMember(Name = "tipo")]
        public Int32 Tipo { get; set; }

        [DataMember(Name = "ativo")]
        public bool Ativo { get; set; }

    }
}
