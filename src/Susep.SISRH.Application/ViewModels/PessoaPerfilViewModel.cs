using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PessoaPerfil")]
    public class PessoaPerfilViewModel
    {
        [DataMember(Name = "pessoaId")]
        public Int64 PessoaId { get; set; }

        [DataMember(Name = "nome")]
        public String Nome { get; set; }

        [DataMember(Name = "unidadeId")]
        public Int64 UnidadeId { get; set; }

        [DataMember(Name = "perfis")]
        public List<PessoaPerfilAcessoViewModel> Perfis { get; set; }
    }

    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PerfilAcesso")]
    public class PessoaPerfilAcessoViewModel
    {
        [DataMember(Name = "perfil")]
        public Int32 Perfil { get; set; }

        [DataMember(Name = "unidades")]
        public List<Int64> Unidades { get; set; }

        public PessoaPerfilAcessoViewModel()
        {
            Unidades = new List<long>();
        }

    }
}
