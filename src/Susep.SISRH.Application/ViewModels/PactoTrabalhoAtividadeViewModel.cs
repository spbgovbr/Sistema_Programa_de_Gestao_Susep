using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PactoTrabalhoAtividade")]
    public class PactoTrabalhoAtividadeViewModel
    {

        [DataMember(Name = "pactoTrabalhoAtividadeId")]
        public Guid PactoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "itemCatalogoId")]
        public Guid ItemCatalogoId { get; set; }

        [DataMember(Name = "itemCatalogo")]
        public String ItemCatalogo { get; set; }

        [DataMember(Name = "itemCatalogoComplexidade")]
        public String ItemCatalogoComplexidade { get; set; }


        [DataMember(Name = "execucaoRemota")]
        public Boolean ExecucaoRemota { get; set; }
        


        [DataMember(Name = "formaCalculoTempoItemCatalogoId")]
        public Int32 FormaCalculoTempoItemCatalogoId { get; set; }        

        [DataMember(Name = "quantidade")]
        public Int32 Quantidade { get; set; }

        [DataMember(Name = "tempoPrevistoPorItem")]
        public Decimal? TempoPrevistoPorItem { get; set; }

        [DataMember(Name = "tempoPrevistoTotal")]
        public Decimal? TempoPrevistoTotal { get; set; }

        [DataMember(Name = "tempoRealizado")]
        public Decimal? TempoRealizado { get; set; }

        [DataMember(Name = "tempoHomologado")]
        public Decimal? TempoHomologado { get; set; }        

        [DataMember(Name = "dataInicio")]
        public DateTime? DataInicio { get; set; }

        [DataMember(Name = "dataFim")]
        public DateTime? DataFim { get; set; }

        [DataMember(Name = "situacaoId")]
        public Int32 SituacaoId { get; set; }

        [DataMember(Name = "situacao")]
        public String Situacao { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

        [DataMember(Name = "consideracoes")]
        public String Consideracoes { get; set; }

        [DataMember(Name = "justificativa")]
        public String Justificativa { get; set; }

        [DataMember(Name = "nota")]
        public int? Nota { get; set; }

    }


}
