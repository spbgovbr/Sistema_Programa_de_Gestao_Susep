import { IUnidade } from "../../unidade/models/unidade.model";
import { IItemCatalogo } from "./item-catalogo.model";

export interface ICatalogo
{
  catalogoId: string;
  unidadeId: string;
  unidadeSigla: string;
  itens: IItemCatalogo[];
  unidade: IUnidade;
}

export interface ICatalogoItemCatalogo {
  catalogoId: string;
  itemCatalogoId: string;
}
