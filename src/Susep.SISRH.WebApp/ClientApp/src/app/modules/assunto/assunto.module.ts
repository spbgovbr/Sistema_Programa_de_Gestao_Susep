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
import { AssuntoDataService } from './services/assunto.service';

// Components:
import { AssuntoPesquisaComponent } from './components/assunto-pesquisa.component';
import { AssuntoEdicaoComponent } from './components/edicao/assunto-edicao.component';
import { MatInputModule, MatFormFieldModule, MatButtonModule } from '@angular/material';

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
    
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,

    SharedModule,
    App3rdPartyModule,
  ],
  declarations: [
    AssuntoPesquisaComponent,
    AssuntoEdicaoComponent
  ],
  exports: [
    AssuntoPesquisaComponent,
    AssuntoEdicaoComponent
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    AssuntoDataService
  ]
})
export class AssuntoModule { }


