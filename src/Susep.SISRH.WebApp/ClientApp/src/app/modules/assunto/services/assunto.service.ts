import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Guid } from 'src/app/shared/helpers/guid.helper';
import { IDadosPaginados } from 'src/app/shared/models/pagination.model';
import { ApplicationResult } from '../../../shared/models/application-result.model';
import { ConfigurationService } from '../../../shared/services/configuration.service';
import { DataService } from '../../../shared/services/data.service';
import { IAssunto } from '../models/assunto.model';
import { IAssuntoPesquisa } from '../models/assunto.pesquisa.model';


@Injectable()
export class AssuntoDataService {

  constructor(
    private service: DataService,
    private configuration: ConfigurationService) { }

  ObterPagina(dadosBusca: IAssuntoPesquisa): Observable<ApplicationResult<IDadosPaginados<IAssunto>>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const params = this.service.toQueryParams(dadosBusca);
    const url = `${baseURI}assunto?${params}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterAtivos(): Observable<ApplicationResult<IAssunto[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}assunto/ativos`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterPorId(id: Guid): Observable<ApplicationResult<IAssunto>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}assunto/${id}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterAssuntosPorTexto(texto: string): Observable<ApplicationResult<IAssunto[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const params = this.service.toQueryParams({texto: texto});
    const url = `${baseURI}assunto/texto?${params}`;

    return this.service.get(url, true, 2, false, false).pipe(map((response: any) => {
      return response;
    }));
  }

  CadastrarAssunto(dados: IAssunto): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}assunto`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AtualizarAssunto(dados: IAssunto): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}assunto`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

}
