import { Location } from '@angular/common';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { PessoaDataService } from '../../modules/pessoa/services/pessoa.service';
import { CustomHttpParamEncoder } from '../helpers/http.param.encoder.helper';
import { TokenHelper } from '../helpers/token.helper';
import { IConnectTokenResponse } from '../models/connect.token.response.model';
import { ApplicationStateService } from './application.state.service';
import { ConfigurationService } from './configuration.service';
import { DataService } from './data.service';
import { StorageService } from './storage.service';

/*
 * Serviço que faz o controle de autenticação das aplicações
 */
@Injectable()
export class SecurityService implements OnInit {

  constructor(private http: HttpClient,
    private router: Router,
    private dataService: DataService,
    private applicationState: ApplicationStateService,
    private configurationService: ConfigurationService,
    private pessoaDataService: PessoaDataService,
    private storage: StorageService,
    private location: Location) {

  }

  ngOnInit(): void {
  }

  //Limpa as informações de autenticação do storage
  public resetUserAuthorizationData() {

    this.storage.store('userAuthorizationCode', '');
    this.storage.store('userAuthorizationToken', '');
    this.storage.store('userAuthorizationIdToken', '');

    this.applicationState.changeAuthenticatedInformation(false);
    this.applicationState.changePerfisUsuario(null);
  }

  //Armazena as informações de autenticação do usuário
  public setUserAuthorizationData(code: any, token: any, idToken: any) {

    if (this.storage.retrieve('userAuthorizationToken') !== '') {
      this.storage.store('userAuthorizationToken', '');
    }

    this.storage.store('userAuthorizationCode', code);
    this.storage.store('userAuthorizationIdToken', idToken);

    if (token) {
      this.storage.store('userAuthorizationToken', token);

      this.pessoaDataService.ObterPerfil().subscribe(perfil => {
        this.applicationState.changeAuthenticatedInformation(true);
        this.applicationState.changePerfisUsuario(perfil.retorno);

        let returnUrl = this.storage.retrieve('returnUrl');

        if (!returnUrl)
          returnUrl = '/dashboard';

        this.router.navigateByUrl(returnUrl);

        this.storage.store('returnUrl', null);
      });
    }

  }

  //Obtém o token do cliente
  public getClientToken() {

    const identityUrl = this.configurationService.getIdentityUrl();
    const clientId = this.configurationService.getClientId();
    const clientSecret = this.configurationService.getClientSecret();
    const scope = this.configurationService.getClientAuthScope();

    if (clientId && identityUrl) {

      const headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

      let params = new HttpParams().set('grant_type', 'client_credentials')
        .set('client_id', clientId)
        .set('scope', scope);

      if (clientSecret)
        params = params.set('client_secret', clientSecret);

      const options = { headers };

      this.http
        .post<IConnectTokenResponse>(identityUrl + 'connect/token', params, options)
        .subscribe(response => { this.storage.store('appAuthorizationToken', response.access_token); });
    }
  }

  public goToAuthentication() {
    this.resetUserAuthorizationData();
    this.router.navigateByUrl('/login');
  }

  public authenticate(username: string, password: string): Observable<IConnectTokenResponse> {
    const identityUrl = this.configurationService.getIdentityUrl();
    const clientId = this.configurationService.getClientId();
    const clientSecret = this.configurationService.getClientSecret();
    const scope = this.configurationService.getClientAuthScope();

    const headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

    let params = new HttpParams()
      .set('grant_type', 'password')
      .set('client_id', clientId)
      .set('scope', scope)
      .set('username', username)
      .set('password', password);

    if (clientSecret)
      params = params.set('client_secret', clientSecret);

    const options = { headers };

    this.applicationState.changeLoadingStatus(true); 
    return this.http
      .post<IConnectTokenResponse>(identityUrl + 'connect/token', params, options)
      .pipe(
        map((res: IConnectTokenResponse) => {
          const token = res.access_token;
          this.setUserAuthorizationData(null, token, null);
          return res;
        }),
        catchError((err: any) => {
          this.applicationState.changeLoadingStatus(false); 
          if (err.status === 400) {
            return of({ 'token_type': err.error.error_description})
          }
          return of(err);
        })
      );
  }


  //REcebe o retorno da autenticação do usuário
  public authenticatedCallback() {
    this.resetUserAuthorizationData();

    const hash = window.location.hash.substr(1);

    const result: any = hash.split('&').reduce(function (result: any, item: string) {
      const parts = item.split('=');
      result[parts[0]] = parts[1];
      return result;
    }, {});

    let code = '';
    let token = '';
    let idToken = '';
    let authResponseIsValid = false;

    if (!result.error) {
      if (result.state !== this.storage.retrieve('authStateControl')) {
        console.log('AuthorizedCallback incorrect state');
      } else {

        code = result.code;
        token = result.access_token;
        idToken = result.id_token;

        const dataIdToken: any = TokenHelper.getDataFromToken(idToken);

        // validate nonce
        if (dataIdToken.nonce !== this.storage.retrieve('authNonce')) {

          console.log('AuthorizedCallback incorrect nonce');

        } else {

          this.storage.store('authNonce', '');
          this.storage.store('authStateControl', '');

          const dataToken: any = TokenHelper.getDataFromToken(token);
          const clientId = this.configurationService.getClientId();
          let temPerfil = false;
          if (dataToken.perfis) {
            const perfis: string[] = dataToken.perfis;
            temPerfil = perfis.filter(p => p.toUpperCase().includes(clientId.toUpperCase())).length > 0;
          }
          if (!temPerfil) {
            console.log('Usuário não tem perfil no sistema');
          }
          else {
            authResponseIsValid = true;
          }
        }
      }
    }

    if (authResponseIsValid) {
      this.setUserAuthorizationData(code, token, idToken);
    }
    else {
      this.logoff();
    }
  }

  //Faz logoff
  public logoff() {

    // Monta a URL de logoff
    //let domainUrl = window.location.href.replace(this.location.path(), '');
    //if (domainUrl.includes('#'))
    //  domainUrl = domainUrl.substr(0, domainUrl.indexOf('#'));
    //domainUrl = domainUrl.endsWith('/') ? domainUrl : `${domainUrl}/`;
    
    let params = new HttpParams({ encoder: new CustomHttpParamEncoder() });
    params = params.set('id_token_hint', this.storage.retrieve('userAuthorizationIdToken'));
    //params = params.set('post_logout_redirect_uri', domainUrl);

    //Remove os dados de autenticação
    this.resetUserAuthorizationData();
    
    //Redireciona o usuário para a URL de logoff
    this.dataService.get(this.configurationService.getIdentityUrl() + 'connect/endsession?' + params.toString(), true).subscribe(res => { });

    //Recarrega a página para atualizar as informações de login
    window.location.reload();
  }

}
