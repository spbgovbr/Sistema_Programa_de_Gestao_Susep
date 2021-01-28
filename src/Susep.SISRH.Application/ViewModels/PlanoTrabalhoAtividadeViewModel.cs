using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoAtividade")]
    public class PlanoTrabalhoAtividadeViewModel
    {

        [DataMember(Name = "planoTrabalhoAtividadeId")]
        public Guid PlanoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "modalidadeExecucaoId")]
        public Int32 ModalidadeExecucaoId { get; set; }

        [DataMember(Name = "modalidadeExecucao")]
        public String ModalidadeExecucao { get; set; }

        [DataMember(Name = "quantidadeColaboradores")]
        public Int32 QuantidadeColaboradores { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

        [DataMember(Name = "itensCatalogo")]
        public List<PlanoTrabalhoAtividadeItemViewModel> Itens { get; set; }

        [DataMember(Name = "criterios")]
        public List<PlanoTrabalhoAtividadeCriterioViewModel> Criterios { get; set; }

        [DataMember(Name = "assuntos")]
        public List<PlanoTrabalhoAtividadeAssuntoViewModel> Assuntos { get; set; }

    }


}
