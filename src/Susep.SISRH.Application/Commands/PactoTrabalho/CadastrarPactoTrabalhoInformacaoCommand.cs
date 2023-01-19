using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Requests;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "CadastrarPactoTrabalhoInformacaoCommand")]
    public class CadastrarPactoTrabalhoInformacaoCommand : UsuarioLogadoRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }


        [DataMember(Name = "informacao")]
        public string Informacao { get; set; }

    }
}
