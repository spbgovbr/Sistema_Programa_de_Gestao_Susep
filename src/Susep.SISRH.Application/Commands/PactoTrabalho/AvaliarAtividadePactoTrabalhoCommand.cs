using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class AvaliarAtividadePactoTrabalhoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "pactoTrabalhoAtividadeId")]
        public Guid PactoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "nota")]
        public int Nota { get; set; }

        [DataMember(Name = "justificativa")]
        public string Justificativa { get; set; }

    }
}
