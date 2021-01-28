using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoAPI")]
    public class APIPlanoTrabalhoAtividadeViewModel
    {

        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "id_atividade")]
        public Guid ItemCatalogoId { get; set; }

        [DataMember(Name = "nome_atividade")]
        public string Titulo { get; set; }

        [DataMember(Name = "faixa_complexidade")]
        public string Complexidade { get; set; }

        [DataMember(Name = "parametros_complexidade")]
        public string DefinicaoComplexidade { get; set; }

        [DataMember(Name = "tempo_exec_presencial")]
        public decimal TempoExecucaoPresencial { get; set; }

        [DataMember(Name = "tempo_exec_teletrabalho")]
        public decimal TempoExecucaoRemoto { get; set; }

        [DataMember(Name = "entrega_esperada")]
        public string EntregasEsperadas { get; set; }

        [DataMember(Name = "qtde_entregas")]
        public int QuantidadeEntregas { get; set; }

        [DataMember(Name = "qtde_entregas_efetivas")]
        public int QuantidadeEntregasRealizadas { get; set; }

        [DataMember(Name = "avaliacao")]
        public int Nota { get; set; }

        [DataMember(Name = "justificativa")]
        public string Justificativa { get; set; }


    }
}
