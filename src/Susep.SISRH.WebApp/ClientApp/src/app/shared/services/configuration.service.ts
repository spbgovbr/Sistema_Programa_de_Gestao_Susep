import { Injectable, isDevMode } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { IConfiguration, TipoModo } from '../models/configuration.model';
import { StorageService } from './storage.service';
import { Subject } from 'rxjs';
import { EnvironmentService } from './environment.service';
import { ApplicationStateService } from './application.state.service';

/*
 * Serviço que obtém as configurações da aplicação definidas no servidor
*/

@Injectable()
export class ConfigurationService {

  serverSettings: IConfiguration;

  // observable that is fired when settings are loaded from server
  private settingsLoadedSource = new Subject();
  settingsLoaded$ = this.settingsLoadedSource.asObservable();
  isReady: boolean;

  constructor(private http: HttpClient,
    private storageService: StorageService,
    private environment: EnvironmentService,
    private applicationStateService: ApplicationStateService) { }

  //Faz a requisição ao servidor para obter as configurações
  load() {

    this.serverSettings = {
      identityUrl: this.environment.identityUrl,
      apiGatewayUrl: this.environment.apiGatewayUrl,
      modo: this.environment.modo,
      valorPadraoTempoComparecimento: this.environment.valorPadraoTempoComparecimento,
      valorPadraoTermosUso: this.environment.valorPadraoTermosUso,
      clientId: this.environment.client.id,
      clientSecret: this.environment.client.secret,
      clientAuthScope: this.environment.client.scope,
      userAuthScope: 'openid ' + this.environment.client.scope,

    } as IConfiguration;

    this.storageService.store('identityUrl', this.serverSettings.identityUrl);
    this.storageService.store('apiGatewayUrl', this.serverSettings.apiGatewayUrl);

    this.storageService.store('modo', this.serverSettings.modo);

    if (this.serverSettings.valorPadraoTempoComparecimento)
      this.storageService.store('tempoComparecimento', this.serverSettings.valorPadraoTempoComparecimento);
    else this.storageService.store('tempoComparecimento', null);

    if (this.serverSettings.valorPadraoTermosUso)
      this.storageService.store('termosUso', this.serverSettings.valorPadraoTermosUso);
    else this.storageService.store('termosUso', null);

    // Para permitir exibir o modo avançado durante a homologação
    this.applicationStateService.perfilUsuario.subscribe(usuario => {
      if (usuario && usuario.perfis.find(p => p.perfil === 1009)) {
        console.log('Setou modo avançado!');
        this.storageService.store('modo', 'avancado');
      }
    });

    this.storageService.store('clientId', this.serverSettings.clientId);

    if (this.serverSettings.clientSecret)
      this.storageService.store('clientSecret', this.serverSettings.clientSecret);

    this.storageService.store('userAuthScope', this.serverSettings.userAuthScope);
    this.storageService.store('clientAuthScope', this.serverSettings.clientAuthScope);

    this.isReady = true;

    this.settingsLoadedSource.next();
  }

  //Retorna a url para a página de autenticação
  getIdentityUrl(): string {    
    const url = this.storageService.retrieve('identityUrl');
    if (url) {
      return url.endsWith('/') ? url : `${url}/`;
    }
    return '';
  }

  //Retorna a url para a página de autenticação
  getApiGatewayUrl(): string {

    const url = this.storageService.retrieve('apiGatewayUrl');

    if (url) {
      return url.endsWith('/') ? url : `${url}/`;
    }

    return '';
  }

  //Retorna o modo no qual o sistema será exibido
  getModo(): TipoModo {
    const modo: TipoModo = this.storageService.retrieve('modo');
    switch (modo) {
      case ('avancado'):
      case ('normal'):
        return modo;
      default:
        return 'normal';
    }
  }

  //Retorna o prazo padrão para comparecimento
  getTempoComparecimento(): string {
    const tempoComparecimento = this.storageService.retrieve('tempoComparecimento');
    return tempoComparecimento;
  }

  //Retorna o prazo padrão para comparecimento
  getTermosUso(): string {
    const termosUso = this.storageService.retrieve('termosUso');
    return termosUso;
  }

  //Retorna o client id da aplicação
  getClientId(): string {
    return this.storageService.retrieve('clientId');
  }

  //Retorna o client secret da aplicação
  getClientSecret(): string {
    return this.storageService.retrieve('clientSecret');
  }

  //Retorna o scope de autenticação da aplicação
  getClientAuthScope(): string {
    return this.storageService.retrieve('clientAuthScope');
  }

  //Retorna o scope de autenticação da usuário
  getUserAuthScope(): string {
    return this.storageService.retrieve('userAuthScope');
  }
}
