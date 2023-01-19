using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Requests;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class RegistrarRealizacaoDeclaracaoPactoTrabalhoCommand : UsuarioLogadoRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "declaracaoId")]
        public Int32 DeclaracaoId { get; set; }


    }
}
