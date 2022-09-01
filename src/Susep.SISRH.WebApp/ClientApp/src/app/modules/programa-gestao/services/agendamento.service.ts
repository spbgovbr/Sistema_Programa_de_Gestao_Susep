import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApplicationResult } from '../../../shared/models/application-result.model';
import { ConfigurationService } from '../../../shared/services/configuration.service';
import { DataService } from '../../../shared/services/data.service';
import { IAgendamento } from '../models/agendamento.model';
import { ICatalogo } from '../models/catalogo.model';


@Injectable()
export class AgendamentoDataService {

  constructor(
    private service: DataService,
    private configuration: ConfigurationService) { }

  ObterPorFiltro(dadosBusca: any): Observable<ApplicationResult<IAgendamento[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const params = this.service.toQueryParams(dadosBusca);
    const url = `${baseURI}pessoa/agendamentos?${params}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  Cadastrar(data: Date): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pessoa/agendamentos`;

    return this.service.post(url, {'Data': data}).pipe(map((response: any) => {
      return response;
    }));
  }

  Excluir(agendamentoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}pessoa/agendamentos/${agendamentoId}`;

    return this.service.delete(url).pipe(map((response: any) => {
      return response;
    }));
  }


}
