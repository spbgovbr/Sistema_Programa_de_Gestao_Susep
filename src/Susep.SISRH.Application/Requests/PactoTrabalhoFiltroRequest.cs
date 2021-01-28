using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.Requests
{
    public class PactoTrabalhoFiltroRequest : UsuarioLogadoRequest
    {
        [DataMember(Name = "unidadeId")]
        public Int64? UnidadeId { get; set; }

        [DataMember(Name = "pessoaId")]
        public Int64? PessoaId { get; set; }

        [DataMember(Name = "situacaoId")]
        public Int32? SituacaoId { get; set; }

        [DataMember(Name = "formaExecucaoId")]
        public Int32? FormaExecucaoId { get; set; }

        [DataMember(Name = "dataInicio")]
        public DateTime? DataInicio { get; set; }

        [DataMember(Name = "dataFim")]
        public DateTime? DataFim { get; set; }


        public List<Int64> UnidadesUsuario { get; set; }
    }
}
