using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.Agendamento
{
    public class ExcluirAgendamentoCommand : UsuarioLogadoRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "agendamentoId")]
        public Guid AgendamentoId { get; set; }

    }
}
