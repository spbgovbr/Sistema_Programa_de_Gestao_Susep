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
    public class AlterarPactoTrabalhoAtividadeCommand : BaseActionRequest, IRequest<IActionResult>
    {
        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "pactoTrabalhoAtividadeId")]
        public Guid PactoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "itemCatalogoId")]
        public Guid ItemCatalogoId { get; set; }

        [DataMember(Name = "quantidade")]
        public Int32 Quantidade { get; set; }

        [DataMember(Name = "tempoPrevistoPorItem")]
        public Decimal? TempoPrevistoPorItem { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "assuntosId", EmitDefaultValue = false)]
        public IEnumerable<Guid> AssuntosId { get; set; }

        [DataMember(Name = "objetosId", EmitDefaultValue = false)]
        public IEnumerable<Guid> ObjetosId { get; set; }

    }
}
