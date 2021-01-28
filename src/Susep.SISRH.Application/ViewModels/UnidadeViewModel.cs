using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "unidade")]
    public class UnidadeViewModel
    {

        [DataMember(Name = "unidadeId")]
        public Int64 UnidadeId { get; set; }

        [DataMember(Name = "sigla")]
        public String Sigla { get; set; }

        [DataMember(Name = "nome")]
        public String Nome { get; set; }

        [DataMember(Name = "unidadeIdPai")]
        public Int64? UnidadeIdPai { get; set; }

        [DataMember(Name = "tipoUnidadeId")]
        public Int64 TipoUnidadeId { get; set; }

        [DataMember(Name = "situacaoUnidadeId")]
        public Int64 SituacaoUnidadeId { get; set; }

        [DataMember(Name = "ufId")]
        public String UfId { get; set; }

        [DataMember(Name = "nivel")]
        public Int64 Nivel { get; set; }

        [DataMember(Name = "tipoFuncaoUnidadeId")]
        public Int64 TipoFuncaoUnidadeId { get; set; }

        [DataMember(Name = "siglaCompleta")]
        public String SiglaCompleta { get; set; }

        [DataMember(Name = "email")]
        public String Email { get; set; }

        [DataMember(Name = "quantidadeServidores")]
        public Int32 QuantidadeServidores { get; set; }
    }
}
