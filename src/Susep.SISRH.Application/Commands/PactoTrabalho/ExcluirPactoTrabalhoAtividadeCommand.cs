using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class ExcluirPactoTrabalhoAtividadeCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "planoTrabalhoAtividadeId")]
        public Guid PactoTrabalhoAtividadeId { get; set; }


    }
}
