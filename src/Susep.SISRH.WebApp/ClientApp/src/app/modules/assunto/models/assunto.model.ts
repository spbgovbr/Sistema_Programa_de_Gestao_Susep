import { Guid } from "src/app/shared/helpers/guid.helper";

export interface IAssunto
{
  assuntoId?: Guid;
  valor?: string;
  assuntoPaiId?: Guid;
  hierarquia?: string;
  nivel?: number;
  ativo?: boolean;
}
