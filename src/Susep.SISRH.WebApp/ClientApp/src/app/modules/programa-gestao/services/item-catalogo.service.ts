import { Injectable } from '@angular/core';

import { Observable, from, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { DataService } from '../../../shared/services/data.service';
import { ConfigurationService } from '../../../shared/services/configuration.service';
import { ApplicationResult } from '../../../shared/models/application-result.model';
import { IItemCatalogo } from '../models/item-catalogo.model';
import { IDadosPaginados } from '../../../shared/models/pagination.model';
import { IItemCatalogoPesquisa } from '../models/item-catalogo.pesquisa.model';


@Injectable()
export class ItemCatalogoDataService {

  constructor(
    private service: DataService,
    private configuration: ConfigurationService) { }

  ObterPagina(dadosBusca: IItemCatalogoPesquisa): Observable<ApplicationResult<IDadosPaginados<IItemCatalogo>>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const params = this.service.toQueryParams(dadosBusca);
    const url = `${baseURI}itemcatalogo?${params}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterItem(itemId: string): Observable<ApplicationResult<IItemCatalogo>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}itemcatalogo/${itemId}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  Cadastrar(item: IItemCatalogo): Observable<ApplicationResult<IItemCatalogo>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}itemcatalogo`;

    return this.service.post(url, item).pipe(map((response: any) => {
      return response;
    }));
  }

  Alterar(item: IItemCatalogo): Observable<ApplicationResult<IItemCatalogo>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}itemcatalogo/${item.itemCatalogoId}`;

    return this.service.put(url, item).pipe(map((response: any) => {
      return response;
    }));
  }

  Excluir(itemCatalogoId: string): Observable<ApplicationResult<IItemCatalogo>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}itemcatalogo/${itemCatalogoId}`;

    return this.service.delete(url).pipe(map((response: any) => {
      return response;
    }));
  }  


}
