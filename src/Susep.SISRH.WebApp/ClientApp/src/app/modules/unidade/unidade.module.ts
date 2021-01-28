import { NgModule, LOCALE_ID } from '@angular/core';

import { BrowserModule } from '@angular/platform-browser';
import { NgbModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { NgBrazil } from 'ng-brazil'
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

import { SharedModule } from '../../shared/shared.module';
import { App3rdPartyModule } from '../../app.3rdparty.module';

// Services
import { UnidadeDataService } from './services/unidade.service';

// Components:
import { UnidadePesquisaComponent } from './components/unidade-pesquisa.component';
import { UnidadeEdicaoComponent } from './components/edicao/unidade-edicao.component';

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

    SharedModule,
    App3rdPartyModule,
  ],
  declarations: [
    UnidadePesquisaComponent,
    UnidadeEdicaoComponent
  ],
  exports: [
    UnidadePesquisaComponent,
    UnidadeEdicaoComponent
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    UnidadeDataService
  ]
})
export class UnidadeModule { }


