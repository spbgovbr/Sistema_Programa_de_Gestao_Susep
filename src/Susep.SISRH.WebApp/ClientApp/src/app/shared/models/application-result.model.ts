export class ApplicationResult<TResultMessage> {

  retorno?: TResultMessage;
  mensagem?: string;
  validacoes?: string[];
  protocol?: string;
  httpStatusCode?: number;
  length?: number
}
