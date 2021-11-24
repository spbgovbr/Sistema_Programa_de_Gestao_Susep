import { Guid } from "src/app/shared/helpers/guid.helper";
import { IPlanoTrabalhoObjetoAssunto } from "./plano-trabalho.model";

export interface IPactoTrabalho
{
  pactoTrabalhoId?: string;
  planoTrabalhoId?: string;
  unidadeId?: number;
  unidade?: string;
  pessoaId?: number;
  pessoa?: string;
  dataInicio?: Date;
  dataFim?: Date;
  formaExecucaoId?: number;
  formaExecucao?: string;
  situacaoId?: number;
  situacao?: string;
  tempoTotalDisponivel?: number;
  cargaHorariaDiaria?: number;
  percentualExecucao?: number;
  relacaoPrevistoRealizado?: number;
  tempoComparecimento?: string;
  termoAceite?: string;
  consideracoes?: string;
  responsavelEnvioAceite?: number;

  atividades?: IPactoTrabalhoAtividade[];
  solicitacoes?: IPactoTrabalhoSolicitacao[];
  historico?: IPactoTrabalhoHistorico[];
  empresas?: IPactoTrabalhoEmpresa[];

  minDataInicio?: Date;
  maxDataFim?: Date;
}

export interface IPactoTrabalhoAtividade {
  pactoTrabalhoAtividadeId?: string;
  pactoTrabalhoId?: string;

  itemCatalogoId?: string;
  itemCatalogo?: string;
  formaCalculoTempoItemCatalogoId?: number;

  quantidade?: number;
  tempoPrevistoPorItem?: number;
  tempoPrevistoTotal?: number;
  tempoRealizado?: number;
  tempoHomologado?: number;
  dataInicio?: Date;
  dataFim?: Date;
  situacaoId?: number;
  situacao?: string;
  execucaoRemota?: boolean;
  descricao?: string;
  consideracoes?: string;
  justificativa?: string;

  nota?: number;

  adicionadoCalendario?: boolean;

  assuntosId?: Guid[];
  objetosId?: Guid[];

}

export interface IPactoTrabalhoAssuntosParaAssociar {
  todosOsAssuntosParaAssociar: IPactoTrabalhoAtividadeAssunto[];
  assuntosAssociados: IPactoTrabalhoAtividadeAssunto[];
}

export interface IPactoTrabalhoAtividadeAssunto {
  pactoTrabalhoAtividadeId?: Guid;
  itemCatalogoId: Guid;
  assuntoId: Guid;
  descricao: string;
}

export interface IPactoTrabalhoEmpresa {
  pactoTrabalhoEmpresaId?: string;
  pactoTrabalhoId?: string;
  nome?: string;
}

export interface IJustificarEstouroPrazoAtividade {
  pactoTrabalhoAtividadeId?: string;
  pactoTrabalhoId?: string;

  justificativa?: string;
}

export interface IPactoTrabalhoDataCalendario {
  date: Date;
  ultimoHorarioOcupado: number;
  events: IPactoTrabalhoEvent[];
}

export interface IPactoTrabalhoEvent {  
  title: string;
  start: any;
  end: any;
  color: any;
}

export interface IPactoTrabalhoSolicitacao {
  pactoTrabalhoSolicitacaoId?: string;
  pactoTrabalhoId?: string;

  tipoSolicitacaoId?: number;
  tipoSolicitacao?: string;

  dataSolicitacao?: Date;
  solicitanteId?: number;
  solicitante?: string;
  dadosSolicitacao?: string;
  observacoesSolicitante?: string;

  analisado?: boolean;

  dataAnalise?: Date;
  analista?: string;
  aprovado?: boolean;
  observacoesAnalista?: string;

  ajustarPrazo?: boolean;
}

export interface IPactoTrabalhoHistorico {
  pactoTrabalhoHistoricoId?: string;
  pactoTrabalhoId?: string;
  situacaoId?: number;
  situacao?: string;
  observacoes?: string;
  responsavelOperacao?: string;
  dataOperacao?: Date;
}

export interface IAvaliacaoAtividade {
  pactoTrabalhoHistoricoId?: string;
  pactoTrabalhoId?: string;
  nota?: number;
  justificativa?: string;
}

export interface IPactoTrabalhoObjeto {
  planoTrabalhoObjetoId?: string;
  planoTrabalhoId?: string;
  objetoId?: string;
  descricao?: string;

  assuntos?: IPlanoTrabalhoObjetoAssunto[];

  associado?: boolean;
}
