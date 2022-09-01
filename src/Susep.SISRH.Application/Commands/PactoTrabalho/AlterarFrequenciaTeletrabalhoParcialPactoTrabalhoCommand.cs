using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class AlterarFrequenciaTeletrabalhoParcialPactoTrabalhoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "tipoFrequenciaTeletrabalhoParcialId")]
        public Int32 TipoFrequenciaTeletrabalhoParcialId { get; set; }

        [DataMember(Name = "quantidadeDiasFrequenciaTeletrabalhoParcial")]
        public Int32 QuantidadeDiasFrequenciaTeletrabalhoParcial { get; set; }


    }
}
