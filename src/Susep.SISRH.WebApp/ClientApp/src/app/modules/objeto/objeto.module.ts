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
import { ObjetoDataService } from './services/objeto.service';

// Components:
import { ObjetoPesquisaComponent } from './components/objeto-pesquisa.component';
import { ObjetoEdicaoComponent } from './components/edicao/objeto-edicao.component';
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
    ObjetoPesquisaComponent,
    ObjetoEdicaoComponent
  ],
  exports: [
    ObjetoPesquisaComponent,
    ObjetoEdicaoComponent
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    ObjetoDataService
  ]
})
export class ObjetoModule { }


