using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class AlterarPeriodoPactoTrabalhoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "dataInicio")]
        public DateTime DataInicio { get; set; }

        [DataMember(Name = "dataFim")]
        public DateTime DataFim { get; set; }


    }
}
