import { IPlanoTrabalhoAtividadeCandidato, IPlanoTrabalhoAtividadeItem } from "../../programa-gestao/models/plano-trabalho.model";
import { Guid } from "../../../shared/helpers/guid.helper";

export interface IPessoa
{
  pessoaId?: number;
  nome?: string;
  unidadeId?: number;
  unidade?: string;
  cargaHoraria?: number;
  candidaturas?: IPlanoTrabalhoAtividadeCandidato[];



}
