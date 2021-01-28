import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { DataService } from './data.service';
import { ConfigurationService } from './configuration.service';
import { ApplicationResult } from '../models/application-result.model';
import { IDominio } from '../models/dominio.model';

@Injectable()
export class DominioDataService {

  constructor(
    private service: DataService,
    private configuration: ConfigurationService) { }

  ObterModalidadesExecucao(closeLoading = true): Observable<ApplicationResult<IDominio[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}dominio/modalidadeexecucao`;

    return this.service.get(url, null, null, closeLoading).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterFormaCalculoTempoItemCatalogo(): Observable<ApplicationResult<IDominio[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}dominio/formacalculotempoitemcatalogo`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterSituacaoPlanoTrabalho(closeLoading = true): Observable<ApplicationResult<IDominio[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}dominio/situacaoplanotrabalho`;

    return this.service.get(url, null, null, closeLoading).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterSituacaoPactoTrabalho(closeLoading = true): Observable<ApplicationResult<IDominio[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}dominio/situacaopactotrabalho`;

    return this.service.get(url, null, null, closeLoading).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterSituacaoAtividadePactoTrabalho(): Observable<ApplicationResult<IDominio[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}dominio/situacaoatividadepactotrabalho`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterCriterioPerfilAtividadePlano(): Observable<ApplicationResult<IDominio[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}dominio/CriterioPerfilAtividadePlano`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterSituacaoCandidaturaPlanoTrabalhoSolicitada(): Observable<ApplicationResult<IDominio[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}dominio/SituacaoCandidaturaPlanoTrabalho/Solicitada`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

 
}
