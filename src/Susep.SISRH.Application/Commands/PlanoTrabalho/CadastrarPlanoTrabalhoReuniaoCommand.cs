using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class CadastrarPlanoTrabalhoReuniaoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "data")]
        public DateTime Data { get; set; }

        [DataMember(Name = "titulo")]
        public string Titulo { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }


    }
}
