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
    public class AlterarPlanoTrabalhoObjetoCommand : BaseActionRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "planoTrabalhoObjetoId")]
        public Guid PlanoTrabalhoObjetoId { get; set; }

        [DataMember(Name = "objetoId")]
        public Guid ObjetoId { get; set; }

        [DataMember(Name = "descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "tipo")]
        public Int32 Tipo { get; set; }

        [DataMember(Name = "chave")]
        public string Chave { get; set; }

        [DataMember(Name = "custos", EmitDefaultValue = false)]
        public IEnumerable<PlanoTrabalhoCustoViewModel> Custos { get; set; }

        [DataMember(Name = "reunioes", EmitDefaultValue = false)]
        public IEnumerable<PlanoTrabalhoReuniaoViewModel> Reunioes { get; set; }

        [DataMember(Name = "assuntos", EmitDefaultValue = false)]
        public IEnumerable<PlanoTrabalhoObjetoAssuntoViewModel> Assuntos { get; set; }

    }
}
