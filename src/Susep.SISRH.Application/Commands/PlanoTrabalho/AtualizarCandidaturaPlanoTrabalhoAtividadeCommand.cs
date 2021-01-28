using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class AtualizarCandidaturaPlanoTrabalhoAtividadeCommand : UsuarioLogadoRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "planoTrabalhoAtividadeId")]
        public Guid PlanoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "planoTrabalhoAtividadeCandidatoId")]
        public Guid PlanoTrabalhoAtividadeCandidatoId { get; set; }

        [DataMember(Name = "situacaoId")]
        public Int32 SituacaoId { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }

    }
}
