using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.Catalogo
{
    public class CadastrarCatalogoItemCatalogoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "catalogoId")]
        public Guid CatalogoId { get; set; }

        [DataMember(Name = "itemCatalogoId")]
        public Guid ItemCatalogoId { get; set; }
    }
}
