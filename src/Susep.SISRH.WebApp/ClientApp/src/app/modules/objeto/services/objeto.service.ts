import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { DataService } from '../../../shared/services/data.service';
import { ConfigurationService } from '../../../shared/services/configuration.service';
import { ApplicationResult } from '../../../shared/models/application-result.model';
import { IObjeto } from '../models/objeto.model';
import { IObjetoPesquisa } from '../models/objeto.pesquisa.model';
import { IDadosPaginados } from 'src/app/shared/models/pagination.model';
import { Guid } from 'src/app/shared/helpers/guid.helper';

@Injectable()
export class ObjetoDataService {

  constructor(
    private service: DataService,
    private configuration: ConfigurationService) { }

  ObterPagina(dadosBusca: IObjetoPesquisa): Observable<ApplicationResult<IDadosPaginados<IObjeto>>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const params = this.service.toQueryParams(dadosBusca);
    const url = `${baseURI}objeto?${params}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterPorId(id: Guid): Observable<ApplicationResult<IObjeto>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}objeto/${id}`;
    
    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterPorTexto(texto: string): Observable<ApplicationResult<IObjeto[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const params = this.service.toQueryParams({texto: texto});
    const url = `${baseURI}objeto/texto?${params}`;

    return this.service.get(url, true, 2, false, false).pipe(map((response: any) => {
      return response;
    }));
  }
  
  CadastrarObjeto(dados: IObjeto): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}objeto`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AtualizarObjeto(dados: IObjeto): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}objeto`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

}
