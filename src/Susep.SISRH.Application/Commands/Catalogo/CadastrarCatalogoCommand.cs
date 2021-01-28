using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.Catalogo
{
    public class CadastrarCatalogoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "unidadeId")]
        public Int64 UnidadeId { get; set; }


    }
}
