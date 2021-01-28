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
import { PessoaDataService } from './services/pessoa.service';

// Components:
import { PessoaPesquisaComponent } from './components/pessoa-pesquisa.component';
import { PessoaEdicaoComponent } from './components/edicao/pessoa-edicao.component';

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
    PessoaPesquisaComponent,
    PessoaEdicaoComponent
  ],
  exports: [
    PessoaPesquisaComponent,
    PessoaEdicaoComponent
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    PessoaDataService
  ]
})
export class PessoaModule { }


