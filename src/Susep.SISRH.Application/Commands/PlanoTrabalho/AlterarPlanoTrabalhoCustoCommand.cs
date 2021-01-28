using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class AlterarPlanoTrabalhoCustoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "planoTrabalhoCustoId")]
        public Guid PlanoTrabalhoCustoId { get; set; }

        [DataMember(Name = "valor")]
        public string Valor { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }


    }
}
