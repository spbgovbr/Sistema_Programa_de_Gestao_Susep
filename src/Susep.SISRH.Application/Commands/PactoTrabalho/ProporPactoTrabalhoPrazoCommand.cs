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
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "ProporPactoTrabalhoPrazoCommand")]
    public class ProporPactoTrabalhoPrazoCommand : UsuarioLogadoRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "dataFim")]
        public DateTime? DataFim { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }

    }
}
