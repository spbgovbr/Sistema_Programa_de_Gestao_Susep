import { Guid } from "src/app/shared/helpers/guid.helper";

export interface IItemCatalogo
{
  itemCatalogoId: string;
  titulo: string;
  descricao: string; 
  formaCalculoTempoItemCatalogoId: number;
  formaCalculoTempoItemCatalogo: string;
  permiteTrabalhoRemoto: boolean;
  tempoExecucaoPreviamenteDefinido: boolean;
  tempoExecucaoPresencial?: number;
  ganhoProdutividade?: number;
  tempoExecucaoRemoto?: number;
  complexidade?: string;
  definicaoComplexidade?: string;
  entregasEsperadas?: string;
  assuntos: IItemCatalogoAssunto[];
  temPactoCadastrado?: boolean;
  temUnidadeAssociada?: boolean;
}

export interface IItemCatalogoAssunto {

    assuntoId?: Guid;
    valor?: string;
    hierarquia?: string;

}
