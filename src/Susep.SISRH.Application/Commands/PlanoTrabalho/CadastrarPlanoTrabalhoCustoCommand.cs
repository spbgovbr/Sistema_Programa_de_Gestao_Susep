using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class CadastrarPlanoTrabalhoCustoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "valor")]
        public Decimal Valor { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }


    }
}
