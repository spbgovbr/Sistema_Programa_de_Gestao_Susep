import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { DataService } from '../../../shared/services/data.service';
import { ConfigurationService } from '../../../shared/services/configuration.service';
import { ICatalogo, ICatalogoItemCatalogo } from '../models/catalogo.model';
import { ApplicationResult } from '../../../shared/models/application-result.model';
import { IDadosPaginados } from '../../../shared/models/pagination.model';
import { ICatalogoPesquisa } from '../models/catalogo.pesquisa.model';
import { IItemCatalogo } from '../models/item-catalogo.model';

@Injectable()
export class CatalogoDataService {

  constructor(
    private service: DataService,
    private configuration: ConfigurationService) { }

  ObterPorUnidade(unidadeId: number): Observable<ApplicationResult<ICatalogo>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}catalogo/${unidadeId}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterItensPorUnidade(unidadeId: number): Observable<ApplicationResult<IItemCatalogo[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}catalogo/${unidadeId}/itens`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterPagina(dadosBusca: ICatalogoPesquisa): Observable<ApplicationResult<IDadosPaginados<ICatalogo>>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const params = this.service.toQueryParams(dadosBusca);
    const url = `${baseURI}catalogo?${params}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterItem(itemId: string): Observable<ApplicationResult<ICatalogo>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}catalogo/${itemId}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }


  CadastrarItem(dados: ICatalogoItemCatalogo): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}catalogo/${dados.catalogoId}/item`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  ExcluirItem(catalogoId: string, itemCatalogoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}catalogo/${catalogoId}/item/${itemCatalogoId}`;

    return this.service.delete(url).pipe(map((response: any) => {
      return response;
    }));
  }

  Cadastrar(dados: ICatalogo): Observable<ApplicationResult<string>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}catalogo`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

}
