using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class ResponderSolitacaoPactoTrabalhoAtividadeCommand : UsuarioLogadoRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "pactoTrabalhoSolicitacaoId")]
        public Guid PactoTrabalhoSolicitacaoId { get; set; }

        [DataMember(Name = "aprovado")]
        public Boolean Aprovado { get; set; }

        [DataMember(Name = "ajustarPrazo")]
        public Boolean AjustarPrazo { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }

    }
}
