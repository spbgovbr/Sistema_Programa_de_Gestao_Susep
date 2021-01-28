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
    public class AlterarAndamentoPactoTrabalhoAtividadeCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "pactoTrabalhoAtividadeId")]
        public Guid PactoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "dataInicio")]
        public DateTime? DataInicio { get; set; }

        [DataMember(Name = "dataFim")]
        public DateTime? DataFim { get; set; }

        [DataMember(Name = "situacaoId")]
        public Int32 SituacaoId { get; set; }

        [DataMember(Name = "tempoRealizado")]
        public Decimal? TempoRealizado { get; set; }

        [DataMember(Name = "consideracoes")]
        public string Consideracoes { get; set; }

    }
}
