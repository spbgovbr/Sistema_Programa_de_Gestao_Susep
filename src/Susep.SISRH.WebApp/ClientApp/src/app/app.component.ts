import { Component, OnInit } from '@angular/core';
import { Router, Event, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';
import { ApplicationStateService } from './shared/services/application.state.service';
import { SecurityService } from './shared/services/security.service';
import { ConfigurationService } from './shared/services/configuration.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  title = 'app';
  isAtuthenticated: boolean;

  closeLoading = true;

  constructor(private router: Router,
    private securityService: SecurityService,
    private configurationService: ConfigurationService,
    private applicationState: ApplicationStateService) {

    //Atualiza as informações de processamento durante eventos de navegação
    this.setUpRoutingEvents();

    //if (window.location.hash) {
    //  this.securityService.authenticatedCallback();
    //}

  }

  ngOnInit() {

    //Get configuration from server environment variables:
    this.configurationService.load();

    //Obtém o token de autorização da aplicação para acessar os serviços
    if (this.configurationService.isReady) {

      this.securityService.getClientToken();
    } else {

      this.configurationService.settingsLoaded$.subscribe(x => {
        this.securityService.getClientToken();
      });
    }

    this.applicationState.isAuthenticated.subscribe(value => this.isAtuthenticated = value);
  }

  setUpRoutingEvents() {

    this.router.events.subscribe((event: Event) => {

      switch (true) {

        case event instanceof NavigationStart: {

          //this.applicationState.changeLoadingStatus(true);
          break;
        }

        case event instanceof NavigationEnd:
        case event instanceof NavigationCancel:
        case event instanceof NavigationError: {

          this.closeMenu();
          if (this.closeLoading)
            //this.applicationState.changeLoadingStatus(false);
          break;
        }
      }
    });
  }

  closeMenu() {

    const toggleMenuButton = document.getElementsByClassName('menu-hamburger')[0];

    if (toggleMenuButton) {

      const menu = document.getElementById('menu-col');
      menu.classList.add('d-none');

      const toggleMenuButtoni: any = toggleMenuButton.childNodes[0]
      toggleMenuButtoni.classList.add('fa-bars');
      toggleMenuButtoni.classList.remove('fa-times');
    }
  }
}
