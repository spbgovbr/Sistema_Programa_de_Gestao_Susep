using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class CadastrarPlanoTrabalhoAtividadeCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "modalidadeExecucaoId")]
        public Int32 ModalidadeExecucaoId { get; set; }

        [DataMember(Name = "quantidadeColaboradores")]
        public Int32 QuantidadeColaboradores { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "itensCatalogo")]
        public IEnumerable<ItemCatalogoViewModel> ItensCatalogo { get; set; }

        [DataMember(Name = "criterios")]
        public IEnumerable<PlanoTrabalhoAtividadeCriterioViewModel> Criterios { get; set; }

        [DataMember(Name = "idsAssuntos")]
        public IEnumerable<Guid> IdsAssuntos { get; set; }

    }
}
