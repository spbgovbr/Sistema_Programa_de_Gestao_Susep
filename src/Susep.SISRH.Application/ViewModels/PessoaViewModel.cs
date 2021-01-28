using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "Pessoa")]
    public class PessoaViewModel
    {
        [DataMember(Name = "pessoaId")]
        public Int64 PessoaId { get; set; }

        [DataMember(Name = "nome")]
        public String Nome { get; set; }

        [DataMember(Name = "email")]
        public String Email { get; set; }

        [DataMember(Name = "unidadeId")]
        public Int64 UnidadeId { get; set; }

        [DataMember(Name = "unidade")]
        public String Unidade { get; set; }

        [DataMember(Name = "tipoFuncaoUnidadeId")]
        public Int64 TipoFuncaoUnidadeId { get; set; }        

        [DataMember(Name = "cargaHoraria")]
        public Int64 CargaHoraria { get; set; }

        [DataMember(Name = "tipoFuncaoId")]
        public Int64? TipoFuncaoId { get; set; }

        [DataMember(Name = "chefe")]
        public Boolean? Chefe { get; set; }

        



        [DataMember(Name = "candidaturas")]
        public List<PlanoTrabalhoAtividadeCandidatoViewModel> Candidaturas { get; set; }



    }
}
