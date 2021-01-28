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
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "ProporPactoTrabalhoAtividadeCommand")]
    public class ProporPactoTrabalhoAtividadeCommand : UsuarioLogadoRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }


        [DataMember(Name = "itemCatalogoId")]
        public Guid ItemCatalogoId { get; set; }

        [DataMember(Name = "itemCatalogo")]
        public string ItemCatalogo { get; set; }

        [DataMember(Name = "situacaoId")]
        public Int32 SituacaoId { get; set; }

        [DataMember(Name = "situacao")]
        public string Situacao { get; set; }

        [DataMember(Name = "tempoPrevistoPorItem")]
        public Decimal? TempoPrevistoPorItem { get; set; }

        [DataMember(Name = "dataInicio")]
        public DateTime? DataInicio { get; set; }

        [DataMember(Name = "dataFim")]
        public DateTime? DataFim { get; set; }

        [DataMember(Name = "tempoRealizado")]
        public Decimal? TempoRealizado { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }

    }
}
