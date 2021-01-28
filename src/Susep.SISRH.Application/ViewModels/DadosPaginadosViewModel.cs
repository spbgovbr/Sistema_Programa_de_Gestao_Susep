using SUSEP.Framework.Messages.Concrete.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "DadosPaginados")]
    public class DadosPaginadosViewModel<TipoLista>
    {

        [DataMember(Name = "controle")]
        public ControlePaginacaoViewModel Controle { get; set; }

        [DataMember(Name = "registros")]
        public IEnumerable<TipoLista> Registros {
            get { return _registros; }
            set
            {
                _registros = value;
                Controle.TotalRegistrosPaginaAtual = value.Count();
            }
        }
        private IEnumerable<TipoLista> _registros;

        public DadosPaginadosViewModel(BaseActionRequest request)
        {
            Controle = new ControlePaginacaoViewModel()
            {
                PaginaAtual = request.Page.Value,
                TamanhoPagina = request.PageSize.Value
            };
        }
    }

    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "ControlePaginacao")]
    public class ControlePaginacaoViewModel
    {

        [DataMember(Name = "totalRegistros")]
        public Int32 TotalRegistros { get; set; }

        [DataMember(Name = "tamanhoPagina")]
        public Int32 TamanhoPagina { get; set; }

        [DataMember(Name = "paginaAtual")]
        public Int32 PaginaAtual { get; set; }

        [DataMember(Name = "totalRegistrosPaginaAtual")]
        public Int32 TotalRegistrosPaginaAtual { get; set; }

        [DataMember(Name = "totalPaginas")]
        public Int32 TotalPaginas
        {
            get
            {
                var difTotalPaginas = Decimal.Divide(TotalRegistros, TamanhoPagina);
                var totalPaginas = Math.Floor(Decimal.Divide(TotalRegistros, TamanhoPagina));
                if (totalPaginas != difTotalPaginas)
                    totalPaginas++;
                return Decimal.ToInt32(totalPaginas);
            }
        }
        

    }

}
