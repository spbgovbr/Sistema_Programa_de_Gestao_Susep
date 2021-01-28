using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Requests;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class CadastrarPlanoTrabalhoCommand : UsuarioLogadoRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "unidadeId")]
        public Int64 UnidadeId { get; set; }

        [DataMember(Name = "dataInicio")]
        public DateTime DataInicio { get; set; }

        [DataMember(Name = "dataFim")]
        public DateTime DataFim { get; set; }

        [DataMember(Name = "tempoComparecimento")]
        public Int32 TempoComparecimento { get; set; }

        [DataMember(Name = "tempoFaseHabilitacao")]
        public Int32 TempoFaseHabilitacao { get; set; }

        [DataMember(Name = "termoAceite")]
        public string TermoAceite { get; set; }

    }
}
