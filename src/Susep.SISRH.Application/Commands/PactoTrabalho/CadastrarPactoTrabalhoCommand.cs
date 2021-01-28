using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Requests;
using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class CadastrarPactoTrabalhoCommand : UsuarioLogadoRequest, IRequest<IActionResult>
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid? PactoTrabalhoId { get; set; }

        [DataMember(Name = "unidadeId")]
        public Int64 UnidadeId { get; set; }

        [DataMember(Name = "pessoaId")]
        public Int64 PessoaId { get; set; }

        [DataMember(Name = "formaExecucaoId")]
        public Int32 FormaExecucaoId { get; set; }

        [DataMember(Name = "dataInicio")]
        public DateTime DataInicio { get; set; }

        [DataMember(Name = "dataFim")]
        public DateTime DataFim { get; set; }

        [DataMember(Name = "termoAceite")]
        public string TermoAceite { get; set; }

    }
}
