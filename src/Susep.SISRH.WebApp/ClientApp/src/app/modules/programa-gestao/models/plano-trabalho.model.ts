import { Guid } from "src/app/shared/helpers/guid.helper";
import { IAssunto } from "../../assunto/models/assunto.model";
import { IUnidade } from "../../unidade/models/unidade.model";

export interface IPlanoTrabalho
{
  planoTrabalhoId?: string;
  unidadeId?: number;
  unidade?: string;
  dataInicio?: Date;
  dataFim?: Date;
  tempoComparecimento?: number;
  tempoFaseHabilitacao?: number;
  situacaoId?: number;
  situacao?: string;
  totalServidoresSetor?: number;
  termoAceite?: string;

  atividades?: IPlanoTrabalhoAtividade[];
  metas?: IPlanoTrabalhoMeta[];
  reunioes?: IPlanoTrabalhoReuniao[];
  historico?: IPlanoTrabalhoHistorico[];
  custos?: IPlanoTrabalhoCusto[];
  empresas?: IPlanoTrabalhoEmpresa[];
  objetos?: IPlanoTrabalhoObjeto[];
}

export interface IPlanoTrabalhoAtividade {
  planoTrabalhoAtividadeId?: string;
  planoTrabalhoId?: string;

  modalidadeExecucaoId?: number;
  modalidadeExecucao?: string;

  quantidadeColaboradores?: number;
  descricao?: string;

  itensCatalogo?: IPlanoTrabalhoAtividadeItem[];
  criterios?: IPlanoTrabalhoAtividadeCriterio[];

  idsAssuntos?: Guid[]; 
  assuntos?: IPlanoTrabalhoAtividadeAssunto[];
}

export interface IPlanoTrabalhoAtividadeItem {
  planoTrabalhoAtividadeItemId?: string;
  planoTrabalhoAtividadeId?: string;

  itemCatalogoId?: string;
  itemCatalogo?: string;
}

export interface IPlanoTrabalhoAtividadeCriterio {
  planoTrabalhoAtividadeCriterioId?: string;
  planoTrabalhoAtividadeId?: string;

  criterioId?: number;
  criterio?: string;
}

export interface IPlanoTrabalhoMeta {
  planoTrabalhoMetaId?: string;
  planoTrabalhoId?: string;

  meta?: string;
  indicador?: string;
  descricao?: string;
}

export interface IPlanoTrabalhoReuniao {
  planoTrabalhoReuniaoId?: string;
  planoTrabalhoId?: string;

  titulo?: string;
  data?: Date;
  descricao?: string;
}

export interface IPlanoTrabalhoCusto {
  planoTrabalhoCustoId?: string;
  planoTrabalhoId?: string;

  valor?: string;
  descricao?: string;
}

export interface IPlanoTrabalhoEmpresa {
  planoTrabalhoEmpresaId?: string;
  planoTrabalhoId?: string;
  nome?: string;
}

export interface IPlanoTrabalhoAvaliacao {
  planoTrabalhoAvaliacaoId?: string;
  planoTrabalhoId?: string;
}

export interface IPlanoTrabalhoHistorico {
  planoTrabalhoHistoricoId?: string;
  planoTrabalhoId?: string;
  situacaoId?: number;
  situacao?: string;
  observacoes?: string;
  responsavelOperacao?: string;
  dataOperacao?: Date;
}

export interface IPlanoTrabalhoAtividadeCandidato {
  planoTrabalhoId?: string;
  planoTrabalhoAtividadeId?: string;
  planoTrabalhoAtividadeCandidatoId?: string;

  pessoaId?: number;
  nome?: string;

  situacaoId?: number;
  situacao?: string;
  aprovado?: boolean;

  legalidade?: number;
  selecao?: number;

  observacoes?: string;

  modalidadeId?: number;
  modalidade?: string;

  unidadeId?: number;
  unidade?: string;

  tarefas?: IPlanoTrabalhoAtividadeItem[];
  perfis?: IPlanoTrabalhoAtividadeCriterio[];

  descricao?: string;

}

export interface IPlanoTrabalhoAtividadeAssunto {
  planoTrabalhoAtividadeAssuntoId: Guid;
  planoTrabalhoAtividadeId: Guid;
  assuntoId: Guid;
}


export interface IPlanoTrabalhoPessoaModalidade {
  pessoaId?: number;
  modalidadeExecucaoId?: number;
  modalidadeExecucao?: string;
  termoAceite?: string;
}

export interface IPlanoTrabalhoObjeto {
  planoTrabalhoObjetoId?: string;
  planoTrabalhoId?: string;
  objetoId?: string;
  descricao?: string;
  tipo?: number;
  chave?: string;
  associadoAtividadePlano?: boolean;

  custos?: IPlanoTrabalhoCusto[];
  assuntos?: IPlanoTrabalhoObjetoAssunto[];
  reunioes?: IPlanoTrabalhoReuniao[];
}

export interface IPlanoTrabalhoObjetoAssunto {
  planoTrabalhoObjetoAssuntoId?: string;
  planoTrabalhoObjetoId?: string;
  assuntoId?: Guid;
  valor?: string;
  hierarquia?: string;
}
