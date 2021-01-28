using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "APIPlanoTrabalho")]
    public class APIPlanoTrabalhoViewModel
    {

        [DataMember(Name = "cod_unidade")]
        public int CodigoUnidadeSIORG { get; set; }

        [DataMember(Name = "id_plano_trabalho")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "matricula_siape")]
        public int MatriculaSIAPE { get; set; }

        [DataMember(Name = "cpf")]
        public string CPF { get; set; }

        [DataMember(Name = "nome_participante")]
        public string pessoa { get; set; }

        [DataMember(Name = "cod_unidade_exercicio")]
        public int CodigoUnidadeSIORGExercicio { get; set; }

        [DataMember(Name = "nome_unidade_exercicio")]
        public string NomeUnidadeSIORGExercicio { get; set; }

        [DataMember(Name = "local_execucao")]
        public int FormaExecucaoId { get; set; }

        [DataMember(Name = "carga_horaria_semanal")]
        public int CargaHorariaSemanal { get; set; }

        [DataMember(Name = "data_inicio")]
        public DateTime DataInicio { get; set; }

        [DataMember(Name = "data_fim")]
        public DateTime DataFim { get; set; }

        [DataMember(Name = "carga_horaria_total")]
        public decimal CargaHorariaTotal { get; set; }



        [DataMember(Name = "atividades")]
        public List<APIPlanoTrabalhoAtividadeViewModel> Atividades { get; set; }
        
    }
}
