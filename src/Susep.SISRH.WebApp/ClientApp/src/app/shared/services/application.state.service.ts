import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IUsuario } from '../models/perfil-usuario.model';
import { StorageService } from './storage.service';

@Injectable()
export class ApplicationStateService {

  private isAuthenticatedMessage = new BehaviorSubject<boolean>(false);
  isAuthenticated = this.isAuthenticatedMessage.asObservable();

  private isLoadingMessage = new BehaviorSubject<boolean>(false);
  isLoading = this.isLoadingMessage.asObservable();

  private perfilUsuarioMessage = new BehaviorSubject<IUsuario>(null);
  perfilUsuario = this.perfilUsuarioMessage.asObservable();

  private modalOpenMessage = new BehaviorSubject<string>(null);
  modalOpen = this.modalOpenMessage.asObservable();


  constructor(private storageService: StorageService) {
    const isAuth = this.storageService.retrieve('isAuthenticated');
    this.changeAuthenticatedInformation(isAuth);

    const perfis = this.storageService.retrieve('perfilUsuario');
    this.changePerfisUsuario(perfis);
  }

  changeAuthenticatedInformation(authenticated: boolean) {
    this.storageService.store('isAuthenticated', authenticated);
    this.isAuthenticatedMessage.next(authenticated);
  }

  changePerfisUsuario(perfil: IUsuario) {
    this.storageService.store('perfilUsuario', perfil);
    this.perfilUsuarioMessage.next(perfil);
  }

  changeLoadingStatus(loading: boolean) {
    this.isLoadingMessage.next(loading)
  }

  changeOpenModal(nome: string) {
    this.modalOpenMessage.next(nome);
  }

}
