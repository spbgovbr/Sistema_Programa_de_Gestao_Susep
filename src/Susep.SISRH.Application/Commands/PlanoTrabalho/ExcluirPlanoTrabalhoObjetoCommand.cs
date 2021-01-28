using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class ExcluirPlanoTrabalhoObjetoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "planoTrabalhoObjetoId")]
        public Guid PlanoTrabalhoObjetoId { get; set; }


    }
}
