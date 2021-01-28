using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoPessoaModalidade")]
    public class PlanoTrabalhoPessoaModalidadeViewModel
    {

        [DataMember(Name = "pessoaId")]
        public Int64 PessoaId { get; set; }

        [DataMember(Name = "modalidadeExecucaoId")]
        public Int32 modalidadeExecucaoId { get; set; }

        [DataMember(Name = "modalidadeExecucao")]
        public String modalidadeExecucao { get; set; }

        [DataMember(Name = "termoAceite")]
        public String TermoAceite { get; set; }

    }


}
