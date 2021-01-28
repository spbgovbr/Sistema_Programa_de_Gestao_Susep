using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.Assunto
{
    public class AlterarAssuntoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "assuntoId")]
        public Guid AssuntoId { get; set; }

        [DataMember(Name = "valor")]
        public String Valor { get; set; }

        [DataMember(Name = "assuntoPaiId")]
        public Guid? AssuntoPaiId { get; set; }

        [DataMember(Name = "ativo")]
        public bool Ativo { get; set; }

    }
}
