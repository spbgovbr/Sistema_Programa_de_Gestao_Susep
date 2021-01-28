using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class AlterarPlanoTrabalhoMetaCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "planoTrabalhoMetaId")]
        public Guid PlanoTrabalhoMetaId { get; set; }

        [DataMember(Name = "meta")]
        public string Meta { get; set; }

        [DataMember(Name = "indicador")]
        public string Indicador { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }


    }
}
