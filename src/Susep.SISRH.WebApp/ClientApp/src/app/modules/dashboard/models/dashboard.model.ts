import { IPlanoTrabalho } from "../../programa-gestao/models/plano-trabalho.model";
import { IPactoTrabalho, IPactoTrabalhoSolicitacao } from "../../programa-gestao/models/pacto-trabalho.model";

export interface IDashboard
{
  planosTrabalho?: IPlanoTrabalho[];
  pactosTrabalho?: IPactoTrabalho[];
  solicitacoes?: IPactoTrabalhoSolicitacao[];
}
