using MediatR;
using Microsoft.AspNetCore.Mvc;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.ItemCatalogo
{
    public class ExcluirItemCatalogoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "itemCatalogoId")]
        public Guid ItemCatalogoId { get; set; }


    }
}
