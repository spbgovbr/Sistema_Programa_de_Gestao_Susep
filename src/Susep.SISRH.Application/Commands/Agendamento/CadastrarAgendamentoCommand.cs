using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Requests;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.Agendamento
{
    public class CadastrarAgendamentoCommand : UsuarioLogadoRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "pessoaId")]
        public Int32 PessoaId { get; set; }

        [DataMember(Name = "data")]
        public DateTime Data { get; set; }

    }
}
