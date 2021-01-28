import { Guid } from "src/app/shared/helpers/guid.helper";

export interface IAssunto
{
  assuntoId?: Guid;
  valor?: string;
  hierarquia?: string;
  nivel?: number;
  ativo?: boolean;
}

export interface IAssuntoCadastro {
  valor: string;
  assuntoPaiId?: Guid;
}

export interface IAssuntoEdicao
{
  assuntoId: Guid;
  valor: string;
  paiId?: Guid; 
  pai: IAssuntoEdicao;
  ativo: boolean;
}

export interface IAssuntoHierarquia {
  assuntoId?: Guid;
  valor?: string;
  hierarquia?: string;
  nivel?: number;
  assuntoPaiId?: Guid;
  filhos?: IAssuntoHierarquia[];
}
