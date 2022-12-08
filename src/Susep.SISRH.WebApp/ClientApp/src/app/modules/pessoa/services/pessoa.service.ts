import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { DataService } from '../../../shared/services/data.service';
import { ConfigurationService } from '../../../shared/services/configuration.service';
import { ApplicationResult } from '../../../shared/models/application-result.model';
import { IDadosPaginados } from '../../../shared/models/pagination.model';
import { IPessoa } from '../models/pessoa.model';
import { IPessoaPesquisa } from '../models/pessoa.pesquisa.model';
import { IUsuario } from '../../../shared/models/perfil-usuario.model';
import { IDadosCombo } from '../../../shared/models/dados-combo.model';
import { IDashboard } from '../../dashboard/models/dashboard.model';
import { IPactoTrabalho, IPactoTrabalhoSolicitacao } from '../../programa-gestao/models/pacto-trabalho.model';
import { IPlanoTrabalho } from '../../programa-gestao/models/plano-trabalho.model';

@Injectable()
export class PessoaDataService {

  constructor(
    private service: DataService,
    private configuration: ConfigurationService) { }


  ObterPagina(dadosBusca: IPessoaPesquisa): Observable<ApplicationResult<IDadosPaginados<IPessoa>>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const params = this.service.toQueryParams(dadosBusca);
    const url = `${baseURI}pessoa?${params}`;

    return this.service.get(url, null, null, true).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterDashboardPlanos(): Observable<ApplicationResult<IPlanoTrabalho[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pessoa/dashboardPlanos`;

    return this.service.get(url, null, null, true).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterDashboardPactos(): Observable<ApplicationResult<IPactoTrabalho[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pessoa/dashboardPactos`;

    return this.service.get(url, null, null, true).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterDashboardPendencias(): Observable<ApplicationResult<IPactoTrabalhoSolicitacao[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pessoa/dashboardPendencias`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }


  ObterPessoa(pessoaId: string): Observable<ApplicationResult<IPessoa>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pessoa/${pessoaId}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterFeriados(pessoaId: number, dataInicio: Date, dataFim: Date): Observable<ApplicationResult<Date[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pessoa/${pessoaId}/feriados/?dataInicio=${this.service.formatAsDate(dataInicio)}&dataFim=${this.service.formatAsDate(dataFim)}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterPerfil(): Observable<ApplicationResult<IUsuario>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pessoa/perfil`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterComPactoTrabalhoDadosCombo(closeLoading = true): Observable<ApplicationResult<IDadosCombo[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pessoa/compactotrabalho`;

    return this.service.get(url, null, null, closeLoading).pipe(map((response: any) => {
      return response;
    }));
  }


}
