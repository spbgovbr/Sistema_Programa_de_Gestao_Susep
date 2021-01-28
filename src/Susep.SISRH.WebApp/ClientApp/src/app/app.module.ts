import { NgModule, LOCALE_ID } from '@angular/core';

import { BrowserModule } from '@angular/platform-browser';
import { NgbModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NgBrazil } from 'ng-brazil'
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

import { AppComponent } from './app.component';
import { RoutingModule } from './app.routes';

import { App3rdPartyModule } from './app.3rdparty.module';
import { SharedModule } from './shared/shared.module';
import { ProgramaGestaoModule } from './modules/programa-gestao/programa-gestao.module';
import { PessoaModule } from './modules/pessoa/pessoa.module';
import { UnidadeModule } from './modules/unidade/unidade.module';
import { AssuntoModule } from './modules/assunto/assunto.module';
import { ObjetoModule } from './modules/objeto/objeto.module';


registerLocaleData(localePt)

@NgModule({
  declarations: [
    AppComponent,    
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),

    RouterModule,
    HttpClientModule,
    NgbModule,
    NgbModalModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,

    NgBrazil,
    BrowserAnimationsModule,

    RoutingModule,

    App3rdPartyModule,
    SharedModule,
    ProgramaGestaoModule,
    PessoaModule,
    UnidadeModule,
    AssuntoModule,
    ObjetoModule,
  ],
  exports: [RouterModule],
  providers: [    
    { provide: LOCALE_ID, useValue: 'pt-BR' }
  ],
  entryComponents: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
