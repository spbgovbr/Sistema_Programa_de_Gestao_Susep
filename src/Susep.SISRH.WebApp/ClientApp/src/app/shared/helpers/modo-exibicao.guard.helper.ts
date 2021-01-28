import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { ConfigurationService } from "../services/configuration.service";
import { TipoModo } from "../models/configuration.model";

/*
 * Classe utilitária que permite acesso a uma rota se o modo de exibição da aplicação for 'avancado'
*/

@Injectable()
export class ModoExibicaoGuard implements CanActivate {

  public modoExibicao: TipoModo;

  constructor(
    private configurationService: ConfigurationService,
    private router: Router) {
    this.modoExibicao = configurationService.getModo();
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.modoExibicao !== 'avancado') {
      this.router.navigate(['/']);
      return false;
    }
    return true;
  }

}
