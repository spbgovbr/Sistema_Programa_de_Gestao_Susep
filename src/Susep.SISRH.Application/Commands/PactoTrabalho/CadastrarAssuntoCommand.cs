using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.Assunto
{
    public class CadastrarAssuntoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "valor")]
        public String Valor { get; set; }

        [DataMember(Name = "assuntoPaiId")]
        public Guid? AssuntoPaiId { get; set; }

    }
}
