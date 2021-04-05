import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IMenuItem } from '../../../models/menu.item.model';
import { IUsuario } from '../../../models/perfil-usuario.model';
import { ApplicationStateService } from '../../../services/application.state.service';
import { PerfilEnum } from '../../../../modules/programa-gestao/enums/perfil.enum';
import { ConfigurationService } from 'src/app/shared/services/configuration.service';
import { TipoModo } from 'src/app/shared/models/configuration.model';

@Component({
  selector: 'app-nav-menu',
  providers: [Location, { provide: LocationStrategy, useClass: PathLocationStrategy }],
  templateUrl: './nav-menu.component.html'
})
export class NavMenuComponent implements OnInit {
  menuItems: IMenuItem[];

  public isAuthenticated: boolean;
  perfilUsuario: IUsuario;

  private modoExibicao: TipoModo;

  constructor(
    private location: Location,
    private applicationState: ApplicationStateService,
    private configurationService: ConfigurationService,
  ) {

    this.getMenuItems();
  }

  ngOnInit() {

    this.applicationState.isAuthenticated.subscribe(value => {
      this.isAuthenticated = value;
      this.loadItems();
    });

    this.applicationState.perfilUsuario.subscribe(perfis => {
      this.perfilUsuario = perfis;
      this.modoExibicao = this.configurationService.getModo();
      this.loadItems();
    });

    this.loadItems();
  }

  loadItems() {
    const items = this.getMenuItems();
    items.forEach(item => {
      this.setIsOpen(item);
    });
    this.menuItems = items;
  }

  getMenuItems() {
    let items: IMenuItem[] = [
      { text: 'Página inicial', url: '/' }
    ];

    if (this.isAuthenticated) {

      if (this.perfilUsuario) {

        const gestorIndicadores = this.perfilUsuario.perfis.filter(p =>
          p.perfil === PerfilEnum.Gestor ||
          p.perfil === PerfilEnum.Administrador ||
          p.perfil === PerfilEnum.GestorIndicadores).length > 0;

        if (gestorIndicadores) {

          if (this.modoExibicao === 'avancado') {
            items.push({
              text: 'Administração',
              subItems: [
                { text: 'Assuntos', url: '/assunto' },
                { text: 'Objetos', url: '/objeto' },
                {
                  text: 'Atividades',
                  subItems: [
                    { text: 'Todas as atividades', url: '/programagestao/catalogo/item' },
                    { text: 'Listas de atividades das unidades', url: '/programagestao/catalogo' },
                  ]
                }
              ]
            });
          }
          else {
            items = items.concat([{
              text: 'Atividades',
              subItems: [
                { text: 'Todas as atividades', url: '/programagestao/catalogo/item' },
                { text: 'Listas de atividades das unidades', url: '/programagestao/catalogo' },
              ]
            }]);
          }
        }


        const gestorUnidade = this.perfilUsuario.perfis.filter(p =>
          p.perfil === PerfilEnum.Gestor ||
          p.perfil === PerfilEnum.Administrador ||
          p.perfil === PerfilEnum.Diretor ||
          p.perfil === PerfilEnum.CoordenadorGeral ||
          p.perfil === PerfilEnum.ChefeUnidade).length > 0;

        if (gestorUnidade) {

          items = items.concat([{
            text: 'Planejamento',
            subItems: [
              { text: 'Programas de gestão', url: '/programagestao' },
              { text: 'Planos de trabalho', url: '/programagestao/pactotrabalho' },
            ]
          }]);
        }

        items = items.concat([
          {
            text: 'Meu trabalho',
            subItems: [
              { text: 'Plano em execução', url: '/programagestao/atividade' },
              { text: 'Habilitação', url: '/programagestao/atividade/habilitacao' },
              { text: 'Histórico de planos de trabalho', url: '/programagestao/atividade/historico' },
            ]
          }]);

        //items = items.concat([items]);
      }

    }

    return items;
  }

  setIsOpen(item: IMenuItem) {
    let isOpen: boolean;
    if (item.subItems) {
      item.subItems.forEach(subitem => {
        if ((subitem.url && this.isActive(subitem.url, false)) ||
          this.setIsOpen(subitem)) {
          isOpen = true;
        }
      });
    }
    item.isOpen = isOpen;
    return isOpen;
  }

  public toggle($event) {
    $event.currentTarget.classList.contains('is-open')
      ? $event.currentTarget.classList.remove('is-open')
      : $event.currentTarget.classList.add('is-open')
  }

  isActive(route, $child) {
    let $path: string = this.location.path();

    if ($child === true)
      $path = "/" + this.location.path().split("/")[1];

    return route === $path;
  };

}
