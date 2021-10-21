import { NgModule, LOCALE_ID } from '@angular/core';

import { BrowserModule } from '@angular/platform-browser';
import { NgbModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';


// Services
import { DataService } from './services/data.service';
import { SecurityService } from './services/security.service';
import { ConfigurationService } from './services/configuration.service';
import { StorageService } from './services/storage.service';
import { ApplicationStateService } from './services/application.state.service';

// Components:
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { PageHeaderComponent } from './components/_partials/page-header/page-header.component';
import { NavMenuComponent } from './components/_partials/nav-menu/nav-menu.component';
import { BreadcrumbComponent } from './components/_partials/breadcrumb/breadcrumb.component';
import { IdentityComponent } from './components/_partials/identity/identity.component';
import { HomePublicaComponent } from './components/_partials/home-publica/home-publica.component';
import { PaginationComponent } from './components/_partials/pagination/pagination.component';
import { InputRatingComponent } from './components/input-rating/input-rating.component';
import { InputAutocompleteAsyncComponent } from './components/input-autocomplete-async/input-autocomplete-async.component';

//Helpers
import { AuthGuard } from './helpers/authorization.guard.helper';
import { EnvServiceProvider } from './services/environment.service.provider';
import { DominioDataService } from './services/dominio.service';
import { InputValidationComponent } from './components/input-validation/input-validation.component';
import { CustomDatepickerComponent } from './components/custom-datepicker/custom-datepicker.component';
import { SecureInputComponent } from './components/secure-input/secure-input.component';
import { InputDatepickerDirective } from './directives/input-datepicker/input-datepicker.directive';
import { DecimalValuesHelper } from './helpers/decimal-valuesr.helper';
import { MatButtonToggleModule, MatSelectModule, MatAutocompleteModule, MatProgressSpinnerModule } from '@angular/material';
import { ModoExibicaoGuard } from './helpers/modo-exibicao.guard.helper';
import { FiltrarItensJaEscolhidosPipe } from './components/input-autocomplete-async/filtrar-ja-escolhidos.pipe';
import { InputCurrencyComponent } from './components/input-currency/input-currency.component';
import { InputCharsCunterComponent } from './components/input-chars-cunter/input-chars-cunter.component';

registerLocaleData(localePt)

@NgModule({
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),

    RouterModule,
    HttpClientModule,    
    NgbModule,
    NgbModalModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    MatButtonToggleModule,
    MatSelectModule,
    MatAutocompleteModule,
    MatProgressSpinnerModule,
  ],
  declarations: [
    HomeComponent,
    LoginComponent,
    PageHeaderComponent,
    NavMenuComponent,
    BreadcrumbComponent,
    IdentityComponent,
    HomePublicaComponent,
    PaginationComponent,
    InputValidationComponent,
    InputCharsCunterComponent,
    CustomDatepickerComponent,
    SecureInputComponent,
    InputDatepickerDirective,
    InputRatingComponent,
    InputAutocompleteAsyncComponent,
    FiltrarItensJaEscolhidosPipe,
    InputCurrencyComponent,
  ],
  exports: [
    HomeComponent,
    PageHeaderComponent,
    NavMenuComponent,
    BreadcrumbComponent,
    IdentityComponent,
    HomePublicaComponent,
    PaginationComponent,
    InputValidationComponent,
    InputCharsCunterComponent,
    CustomDatepickerComponent,
    SecureInputComponent,
    InputRatingComponent,
    InputAutocompleteAsyncComponent,
    InputCurrencyComponent,
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    DataService,
    SecurityService,
    ConfigurationService,
    StorageService,
    ApplicationStateService,
    AuthGuard,
    ModoExibicaoGuard,
    DecimalValuesHelper,
    EnvServiceProvider,

    DominioDataService,
  ]
})
export class SharedModule { }


