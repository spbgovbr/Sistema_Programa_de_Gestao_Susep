import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Guid } from 'src/app/shared/helpers/guid.helper';
import { ApplicationResult } from '../../../shared/models/application-result.model';
import { IDominio } from '../../../shared/models/dominio.model';
import { IDadosPaginados } from '../../../shared/models/pagination.model';
import { ConfigurationService } from '../../../shared/services/configuration.service';
import { DataService } from '../../../shared/services/data.service';
import { IAvaliacaoAtividade, IJustificarEstouroPrazoAtividade, IPactoTrabalho, IPactoTrabalhoAssuntosParaAssociar, IPactoTrabalhoAtividade, IPactoTrabalhoEmpresa, IPactoTrabalhoHistorico, IPactoTrabalhoInformacao, IPactoTrabalhoSolicitacao } from '../models/pacto-trabalho.model';
import { IPactoTrabalhoPesquisa } from '../models/pacto-trabalho.pesquisa.model';


@Injectable()
export class PactoTrabalhoDataService {

  constructor(
    private service: DataService,
    private configuration: ConfigurationService) { }

  ObterPagina(dadosBusca: IPactoTrabalhoPesquisa): Observable<ApplicationResult<IDadosPaginados<IPactoTrabalho>>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const params = this.service.toQueryParams(dadosBusca);
    const url = `${baseURI}pactotrabalho?${params}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterPacto(pactoTrabalhoId: string): Observable<ApplicationResult<IPactoTrabalho>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ExcluirPacto(pactoTrabalhoId: string): Observable<ApplicationResult<IPactoTrabalho>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}`;

    return this.service.delete(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterPactoAtual(): Observable<ApplicationResult<IPactoTrabalho>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/atual`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  Cadastrar(dados: IPactoTrabalho): Observable<ApplicationResult<string>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  Alterar(dados: IPactoTrabalho): Observable<ApplicationResult<string>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${dados.planoTrabalhoId}`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AlterarPeriodo(dados: IPactoTrabalho): Observable<ApplicationResult<string>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${dados.pactoTrabalhoId}/periodo`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AlterarFrequenciaTeletrabalhoParcial(dados: IPactoTrabalho): Observable<ApplicationResult<string>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${dados.pactoTrabalhoId}/frequenciaTeletrabalhoParcial`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }


  ObterAtividades(pactoTrabalhoId: string): Observable<ApplicationResult<IPactoTrabalhoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}/atividades`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  CadastrarAtividade(dados: IPactoTrabalhoAtividade): Observable<ApplicationResult<IPactoTrabalhoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${dados.pactoTrabalhoId}/atividades`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AlterarAtividade(dados: IPactoTrabalhoAtividade): Observable<ApplicationResult<IPactoTrabalhoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${dados.pactoTrabalhoId}/atividades/${dados.pactoTrabalhoAtividadeId}`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  ExcluirAtividade(pactoTrabalhoId: string, pactoTrabalhoAtividadeId: string): Observable<ApplicationResult<IPactoTrabalhoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}/atividades/${pactoTrabalhoAtividadeId}`;

    return this.service.delete(url).pipe(map((response: any) => {
      return response;
    }));
  }

  AlterarAndamentoAtividade(dados: IPactoTrabalhoAtividade): Observable<ApplicationResult<IPactoTrabalhoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${dados.pactoTrabalhoId}/atividades/${dados.pactoTrabalhoAtividadeId}/andamento`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AlterarAtividadeKanban(pactoTrabalhoId: string, pactoTrabalhoAtividadeId: string, acao: 'todo' | 'doing' | 'done'): Observable<ApplicationResult<IPactoTrabalhoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}/atividades/${pactoTrabalhoAtividadeId}/${acao}`;

    return this.service.put(url, {}).pipe(map((response: any) => {
      return response;
    }));
  }

  AvaliarAtividade(pactoTrabalhoId: string, pactoTrabalhoAtividadeId: string, dados: IAvaliacaoAtividade): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}/atividades/${pactoTrabalhoAtividadeId}/avaliar`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }


  EnviarParaAceite(pactotrabalhoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactotrabalhoId}/enviarParaAceite`;

    return this.service.put(url, {}).pipe(map((response: any) => {
      return response;
    }));
  }

  Aceitar(pactotrabalhoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactotrabalhoId}/aceitar`;

    return this.service.put(url, {}).pipe(map((response: any) => {
      return response;
    }));
  }

  Rejeitar(pactotrabalhoId: string, justificativa: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactotrabalhoId}/rejeitar`;

    return this.service.put(url, { 'observacoes': justificativa }).pipe(map((response: any) => {
      return response;
    }));
  }

  VoltarParaRascunho(pactotrabalhoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactotrabalhoId}/voltarParaRascunho`;

    return this.service.put(url, {}).pipe(map((response: any) => {
      return response;
    }));
  }

  IniciarExecucao(pactotrabalhoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactotrabalhoId}/iniciarExecucao`;

    return this.service.put(url, {}).pipe(map((response: any) => {
      return response;
    }));
  }

  ConcluirExecucao(pactotrabalhoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactotrabalhoId}/concluirExecucao`;

    return this.service.put(url, {}).pipe(map((response: any) => {
      return response;
    }));
  }

  Reabrir(pactotrabalhoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactotrabalhoId}/reabrir`;

    return this.service.put(url, {}).pipe(map((response: any) => {
      return response;
    }));
  }

  Concluir(pactotrabalhoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactotrabalhoId}/concluir`;

    return this.service.put(url, {}).pipe(map((response: any) => {
      return response;
    }));
  }


  ObterHistorico(pactotrabalhoId: string): Observable<ApplicationResult<IPactoTrabalhoHistorico[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactotrabalhoId}/historico`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }



  ObterSolicitacoes(pactoTrabalhoId: string): Observable<ApplicationResult<IPactoTrabalhoSolicitacao[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}/solicitacao`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ProporAtividade(dados: IPactoTrabalhoAtividade): Observable<ApplicationResult<IPactoTrabalhoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${dados.pactoTrabalhoId}/solicitacao/atividade`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  ProporAlteracaoPrazo(dados: IPactoTrabalhoAtividade): Observable<ApplicationResult<IPactoTrabalhoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${dados.pactoTrabalhoId}/solicitacao/prazo`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  JustificarEstouroPrazo(dados: IJustificarEstouroPrazoAtividade): Observable<ApplicationResult<IJustificarEstouroPrazoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${dados.pactoTrabalhoId}/solicitacao/justificarestouroprazo`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  ProporExclusaoAtividade(dados: IPactoTrabalhoAtividade): Observable<ApplicationResult<IJustificarEstouroPrazoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${dados.pactoTrabalhoId}/solicitacao/excluiratividade`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  ResponderSolicitacao(dados: IPactoTrabalhoSolicitacao): Observable<ApplicationResult<IPactoTrabalho>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${dados.pactoTrabalhoId}/solicitacao/${dados.pactoTrabalhoSolicitacaoId}/responder`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterAssuntosParaAssociar(pactoTrabalhoId: Guid): Observable<ApplicationResult<IPactoTrabalhoAssuntosParaAssociar>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}/assuntosParaAssociar`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  private empresas: IPactoTrabalhoEmpresa[] = [
    { pactoTrabalhoId: Guid.newGuid(), pactoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 1' }, 
    { pactoTrabalhoId: Guid.newGuid(), pactoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 2' }, 
    { pactoTrabalhoId: Guid.newGuid(), pactoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 3' }, 
    { pactoTrabalhoId: Guid.newGuid(), pactoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 4' }, 
    { pactoTrabalhoId: Guid.newGuid(), pactoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 5' }, 
    { pactoTrabalhoId: Guid.newGuid(), pactoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 6' }, 
    { pactoTrabalhoId: Guid.newGuid(), pactoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 7' }, 
    { pactoTrabalhoId: Guid.newGuid(), pactoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 8' }, 
    { pactoTrabalhoId: Guid.newGuid(), pactoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 9' }, 
];

  ObterEmpresas(pactoTrabalhoId: string): Observable<ApplicationResult<IPactoTrabalhoEmpresa[]>> {
    /* IMPLEMENTAR */
    /*
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/empresas`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
    */
    return of({
      retorno: this.empresas.filter((v, i) => i >= 0 && i < 5)
    });
  }

  ObterEmpresassPorTexto(valor: string): Observable<ApplicationResult<IPactoTrabalhoEmpresa[]>> {
    // IMPLEMENTAR
    return of({
      retorno: this.empresas.filter((v, i) => i >= 5 && i < 9)
    });
  }

  ObterEmpresaPorId(id: Guid): Observable<ApplicationResult<IPactoTrabalhoEmpresa>> {
    // IMPLEMENTAR
    return of({
      retorno: this.empresas.find(e => e.pactoTrabalhoEmpresaId === id)
    });
  }

  ObterDeclaracoesNaoRealizadas(pactoTrabalhoId: string): Observable<ApplicationResult<IDominio[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}/declaracao`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  RegistrarVisualizacaoDeclaracao(pactoTrabalhoId: string, declaracaoId: number): Observable<ApplicationResult<IPactoTrabalhoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}/declaracao/${declaracaoId}/visualizada`;

    return this.service.post(url, {}).pipe(map((response: any) => {
      return response;
    }));
  }

  RegistrarRealizacaoDeclaracao(pactoTrabalhoId: string, declaracaoId: number): Observable<ApplicationResult<IPactoTrabalhoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}/declaracao/${declaracaoId}/realizada`;

    return this.service.put(url, {}).pipe(map((response: any) => {
      return response;
    }));
  }


  ObterInformacoes(pactoTrabalhoId: string): Observable<ApplicationResult<IPactoTrabalhoInformacao[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}/informacao`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  RegistrarInformacao(pactoTrabalhoId: string, dados: IPactoTrabalhoInformacao): Observable<ApplicationResult<IPactoTrabalhoInformacao>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pactotrabalho/${pactoTrabalhoId}/informacao`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

}
