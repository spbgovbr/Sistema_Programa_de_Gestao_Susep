import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { DataService } from '../../../shared/services/data.service';
import { ConfigurationService } from '../../../shared/services/configuration.service';
import { ApplicationResult } from '../../../shared/models/application-result.model';
import { IUnidade } from '../models/unidade.model';
import { IDadosCombo } from '../../../shared/models/dados-combo.model';

@Injectable()
export class UnidadeDataService {

  constructor(
    private service: DataService,
    private configuration: ConfigurationService) { }

  ObterAtivasDadosCombo(): Observable<ApplicationResult<IDadosCombo[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}unidade/ativas`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterComCatalogoCadastrado(): Observable<ApplicationResult<IDadosCombo[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}unidade/comcatalogocadastrado`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterSemCatalogoCadastrado(): Observable<ApplicationResult<IDadosCombo[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}unidade/semcatalogocadastrado`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }


  ObterComPlanotrabalhoDadosCombo(closeLoading = true): Observable<ApplicationResult<IDadosCombo[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}unidade/complanotrabalho`;

    return this.service.get(url, null, null, closeLoading).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterItem(unidadeId: number): Observable<ApplicationResult<IUnidade>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}unidade/${unidadeId}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterModalidadesExecucao(unidadeId: number): Observable<ApplicationResult<IDadosCombo[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}unidade/${unidadeId}/modalidadeexecucao`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterPessoas(unidadeId: number): Observable<ApplicationResult<IDadosCombo[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}unidade/${unidadeId}/pessoas`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

}
