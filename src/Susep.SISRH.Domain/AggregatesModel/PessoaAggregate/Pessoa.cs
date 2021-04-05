using Susep.SISRH.Domain.AggregatesModel.FeriadoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.PessoaAggregate
{
    /// <summary>
    /// Representa as pessoas 
    /// </summary>
    public class Pessoa : Entity
    {

        public Int64 PessoaId { get; private set; }
        public String Nome { get; private set; }
        public String Email { get; private set; }
        public String Cpf { get; private set; }
        public String MatriculaSiape { get; private set; }
        public Int64 UnidadeId { get; private set; }
        public Int64? TipoFuncaoId { get; private set; }
        public Int32? CargaHorariaDb { get; private set; }

        public Int32 CargaHoraria
        {
            get => CargaHorariaDb.HasValue && CargaHorariaDb.Value > 0 ?
                   CargaHorariaDb.Value : 8;            
        }

        public Unidade Unidade { get; private set; }

        public IEnumerable<PessoaModalidadeExecucao> ModalidadesExecucao { get; private set; }
        public IEnumerable<PactoTrabalho> PactosTrabalho { get; private set; }
        public List<PlanoTrabalhoAtividadeCandidato> Candidaturas { get; private set; }

    }
}
