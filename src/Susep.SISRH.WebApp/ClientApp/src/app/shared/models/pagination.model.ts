export interface IControlePaginacao
{
  totalRegistros: number;
  tamanhoPagina: number;
  paginaAtual: number;
  totalRegistrosPaginaAtual: number;
  totalPaginas?: number;
}

export interface IDadosPaginados<TRegistros> {
  controle: IControlePaginacao;
  registros: TRegistros[];
}
