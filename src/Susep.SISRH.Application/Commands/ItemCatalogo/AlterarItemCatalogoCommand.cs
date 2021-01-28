using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.ItemCatalogo
{
    public class AlterarItemCatalogoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "itemCatalogoId")]
        public Guid ItemCatalogoId { get; set; }

        [DataMember(Name = "titulo")]
        public String Titulo { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

        [DataMember(Name = "formaCalculoTempoItemCatalogoId")]
        public Int32 FormaCalculoTempoItemCatalogoId { get; set; }

        [DataMember(Name = "permiteTrabalhoRemoto")]
        public Boolean PermiteTrabalhoRemoto { get; set; }

        [DataMember(Name = "tempoExecucaoPresencial")]
        public Decimal? TempoExecucaoPresencial { get; set; }

        [DataMember(Name = "tempoExecucaoRemoto")]
        public Decimal? TempoExecucaoRemoto { get; set; }

        [DataMember(Name = "complexidade")]
        public String Complexidade { get; set; }

        [DataMember(Name = "definicaoComplexidade")]
        public String DefinicaoComplexidade { get; set; }

        [DataMember(Name = "entregasEsperadas")]
        public String EntregasEsperadas { get; set; }
        [DataMember(Name = "assuntos", EmitDefaultValue = false)]
        public IEnumerable<AssuntoEdicaoViewModel> Assuntos { get; set; }

    }
}
