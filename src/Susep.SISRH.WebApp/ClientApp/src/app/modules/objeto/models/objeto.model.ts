import { Guid } from "src/app/shared/helpers/guid.helper";

export interface IObjeto
{
  objetoId?: Guid;
  chave: string;
  descricao: string;
  tipo: number;
  ativo: boolean;
}

