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
    public class AlterarSituacaoPactoTrabalhoAtividadeCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "pactoTrabalhoAtividadeId")]
        public Guid PactoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "situacaoId")]
        public Int32 SituacaoId { get; set; }


    }
}
