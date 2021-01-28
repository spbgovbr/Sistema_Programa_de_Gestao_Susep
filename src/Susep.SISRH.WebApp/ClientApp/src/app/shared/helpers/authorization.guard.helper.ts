import { Injectable, OnInit } from "@angular/core";
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { ApplicationStateService } from "../services/application.state.service";
import { SecurityService } from "../services/security.service";
import { StorageService } from "../services/storage.service";
import { IUsuario } from "../models/perfil-usuario.model";
import { PerfilEnum } from "../../modules/programa-gestao/enums/perfil.enum";

/*
 * Classe utilitária que verifica se uma rota é acessível pelo usuário ou não
*/

@Injectable()
export class AuthGuard implements CanActivate {

  public isAuthenticated: boolean;
  perfilUsuario: IUsuario;

  constructor(
    private applicationState: ApplicationStateService,
    private securityService: SecurityService,
    private storage: StorageService,
    private router: Router) {

    this.applicationState.isAuthenticated.subscribe(value => this.isAuthenticated = value);
    this.applicationState.perfilUsuario.subscribe(value => this.perfilUsuario = value);
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {

    // Tenta obter o token do usuário
    // Se o token existir, significa que o usuário está autenticado
    if (this.isAuthenticated) {

      //Se for gestor ou administrador do sistema, autoriza tudo
      if (this.perfilUsuario.perfis.filter(p => p.perfil === PerfilEnum.Gestor || p.perfil === PerfilEnum.Administrador).length > 0)
        return true;

      //Se não for, verifica as roles
      if (route.data.roles) {
        const temPerfil = this.perfilUsuario.perfis.filter(p => route.data.roles.indexOf(p.perfil) !== -1).length > 0;
        if (!temPerfil) {
          this.router.navigate(['/']);
          return false;
        }
      }

      return true;
    }

    //Armazena a URL que estava tentando ser acessada para redirecionar quando retornar da autenticação
    this.storage.store("returnUrl", state.url);

    //  Se o token não existir, redireciona para o login
    this.securityService.goToAuthentication();
  }
}
