import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { DecimalValuesHelper } from '../helpers/decimal-valuesr.helper';
import { Guid } from '../helpers/guid.helper';
import { ApplicationStateService } from './application.state.service';
import { StorageService } from './storage.service';

@Injectable()
export class DataService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService,
    private storage: StorageService,
    private decimalValuesHelper: DecimalValuesHelper,
    private applicationState: ApplicationStateService) { }//private securityService: SecurityService


  get(url: string, ignorarMensagem?: boolean, retry = 1, closeLoading = true, openLoading = true): Observable<Response> {
    const options = {};
    this.setHeaders(options);
    if (openLoading)
      this.applicationState.changeLoadingStatus(true);
    return this.http.get(url, options)
      .pipe(
        // retry(3), // retry a failed request up to 3 times
        map((res: Response) => {
          if (closeLoading)
            this.applicationState.changeLoadingStatus(false);
          return res;
        }),
        catchError(err => {
          if (err.status === 0 || err.status === 502) {
            if (retry < 5)
              return this.get(url, ignorarMensagem, retry + 1, closeLoading);
          }

          return this.handleError(err, ignorarMensagem);
        })
      );
  }


  postWithId(url: string, data: any, params?: any, ignorarMensagem?: boolean): Observable<Response> {
    return this.doPost(url, data, true, params, ignorarMensagem);
  }

  post(url: string, data: any, params?: any, ignorarMensagem?: boolean): Observable<Response> {
    return this.doPost(url, data, false, params, ignorarMensagem);
  }

  private doPost(url: string, data: any, needId: boolean, params?: any, ignorarMensagem?: boolean): Observable<Response> {
    const options = {};
    this.setHeaders(options, needId);

    this.applicationState.changeLoadingStatus(true);

    return this.http.post(url, this.changePropertiesValuesByTypes(data), options)
      .pipe(
        map((res: Response) => {
          this.applicationState.changeLoadingStatus(false);
          if (!ignorarMensagem) this.toastr.success('Dados salvos com sucesso');
          console.log('ATENCAO!!!!', 'Executou POST no data.service.ts', res);
          return res;
        }),
        catchError(err => {
          return this.handleError(err);
        })
      );
  }


  put(url: string, data: any, params?: any): Observable<Response> {
    return this.doPut(url, data, true, params);
  }

  putWithId(url: string, data: any, params?: any): Observable<Response> {
    return this.doPut(url, data, true, params);
  }

  private doPut(url: string, data: any, needId: boolean, params?: any): Observable<Response> {
    let options = {};
    this.setHeaders(options, needId);

    this.applicationState.changeLoadingStatus(true);

    return this.http.put(url, this.changePropertiesValuesByTypes(data), options)
      .pipe(
        map((res: Response) => {
          this.applicationState.changeLoadingStatus(false);
          this.toastr.success('Dados alterados com sucesso');
          return res;
        }),
        catchError(err => {
          return this.handleError(err);
        })
      );
  }


  delete(url: string, params?: any): Observable<Response> {
    let options = {};
    this.setHeaders(options,true);

    console.log('data.service deleting');

    this.applicationState.changeLoadingStatus(true);

    return this.http.delete(url, options)
      .pipe(
        map((res: Response) => {
          this.applicationState.changeLoadingStatus(false);
          this.toastr.success('Dados excluídos com sucesso');
          return res;
        }),
        catchError(err => {
          return this.handleError(err);
        })
      );
  }

  private changePropertiesValuesByTypes(data) {    
    Object.keys(data).forEach(field => {
      if (data[field]) {
        const numeric = Number(this.decimalValuesHelper.fromPtBr(data[field]));
        if (data[field] instanceof Date)
          data[field] = this.formatAsDate(data[field])
        else if (data[field].toString().toLowerCase() === 'true')
          data[field] = true;
        else if (data[field].toString().toLowerCase() === 'false')
          data[field] = false;
        else if (!isNaN(numeric))
          data[field] = numeric;
      }
    });    
    return data;
  }

  public toQueryParams(data: any) {
    return Object.entries(data).map(([key, val]) => val && val !== 'null' ? `${key}=${(val instanceof Date ? this.formatAsDate(val) : val)}` : '').filter(p => p !== '').join('&');
  }

  public formatAsDate(val: any) {    
    const d = new Date(val);
    const month = '' + (d.getMonth() + 1);
    const day = '' + d.getDate();
    const year = d.getFullYear();

    let formatedDate = [year, this.padLeft(month), this.padLeft(day)].join('-');

    const hour = '' + d.getHours();
    const minute = '' + d.getMinutes();
    if (hour) {
      formatedDate += `T${this.padLeft(hour)}:${minute ? this.padLeft(minute) : '00'}:00`
    }
    return formatedDate;
  }

  private padLeft(value: string) {
    if (value.length < 2)
      value = '0' + value;
    return value;
  }


  private handleError(error: any, ignorarMensagem?: boolean) {
    this.applicationState.changeLoadingStatus(false);

    let tituloErro: string = null;
    let mensagemErro: string = null;

    if (error.status === 0 || error.status === 502) {
      //this.toastr.error('Verifique sua conexão ou entre em contato com a TI da Susep', 'Não foi possível conectar com o servidor');
    }
    else {
      tituloErro = 'Erro';
      mensagemErro = 'Ocorreu um erro durante o processamento';
    }

    if (error.error && error.error.validacoes && error.error.validacoes.length) {
      this.toastr.error(error.error.validacoes);
      return;
    }    

    if (error.error instanceof ErrorEvent) {
      if (error.error && error.error.message) {
        // A client-side or network error occurred. Handle it accordingly.
        console.error('Client side network error occurred:', error.error.message);
      }
      else {
        // A client-side or network error occurred. Handle it accordingly.
        console.error('Client side network error occurred:', JSON.stringify(error));
      }
    } else {
      if (error.error && error.error.mensagem) {
        // The backend returned an unsuccessful response code.
        // The response body may contain clues as to what went wrong,
        mensagemErro = error.error.validacoes && error.error.validacoes.length?
          error.error.validacoes.map((e: string) => {
              return e.trim().endsWith('.')? `${ e.trim() } `: `${ e.trim() }. `
          }):
          "Ocorreu um erro ao processar a solicitação.";
        console.error('Backend - ' +
          `status: ${error.status}, ` +
          `statusText: ${error.statusText}, ` +
          `message: ${error.error.mensagem}`);
      }
      else {
        // The backend returned an unsuccessful response code.
        // The response body may contain clues as to what went wrong,
        console.error('Backend - ' + JSON.stringify(error));
      }

    }

    if (!ignorarMensagem && (mensagemErro || tituloErro)) {
      this.toastr.error(mensagemErro, tituloErro);
    }

    // return an observable with a user-facing error message
    return throwError(error || 'server error');
  }

  private setHeaders(options: any, needId?: boolean) {
    let headers = new HttpHeaders();

    const token = this.getToken();
    if (token) {
      headers = headers.append('Authorization', 'Bearer ' + token);
    }

    if (needId) {
      headers = headers.append('x-requestid', Guid.newGuid());
    }

    options["headers"] = headers;
  }

  //Retorna o token a ser utilizado pela aplicação
  // Se o usuário estiver autenticado, retorna o token do usuário
  // Caso contrário, retorna o token do cliente
  private getToken(): any {
    const userToken = this.storage.retrieve('userAuthorizationToken');
    if (userToken) {
      return userToken;
    }
    return this.storage.retrieve('appAuthorizationToken');
  }
}
