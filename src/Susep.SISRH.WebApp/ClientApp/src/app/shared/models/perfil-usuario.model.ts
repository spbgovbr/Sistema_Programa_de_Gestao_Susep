
export interface IUsuario {  
  pessoaId: number;
  nome: string;
  unidadeId: number;
  perfis: IPerfilUsuario[];
}

export interface IPerfilUsuario
{
  perfil: number;
  unidades?: number[];
}
