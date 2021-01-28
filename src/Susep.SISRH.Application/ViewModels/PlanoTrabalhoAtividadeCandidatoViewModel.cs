using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoAtividadeCandidato")]
    public class PlanoTrabalhoAtividadeCandidatoViewModel
    {

        [DataMember(Name = "planoTrabalhoAtividadeCandidatoId")]
        public Guid PlanoTrabalhoAtividadeCandidatoId { get; set; }

        [DataMember(Name = "planoTrabalhoAtividadeId")]
        public Guid PlanoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "pessoaId")]
        public Int64 PessoaId { get; set; }

        [DataMember(Name = "nome")]
        public String Nome { get; set; }

        [DataMember(Name = "situacaoId")]
        public Int32 SituacaoId { get; set; }

        [DataMember(Name = "situacao")]
        public String Situacao { get; set; }

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "modalidadeId")]
        public Int32 modalidadeId { get; set; }

        [DataMember(Name = "modalidade")]
        public String modalidade { get; set; }

        [DataMember(Name = "unidadeId")]
        public Int32 unidadeId { get; set; }

        [DataMember(Name = "unidade")]
        public String unidade { get; set; }

        [DataMember(Name = "tarefas")]
        public List<PlanoTrabalhoAtividadeItemViewModel> Tarefas { get; set; }

        [DataMember(Name = "perfis")]
        public List<PlanoTrabalhoAtividadeCriterioViewModel> Perfis { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

    }


}
