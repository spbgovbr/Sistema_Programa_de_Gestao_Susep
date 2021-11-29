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
import { CatalogoDataService } from './services/catalogo.service';
import { ItemCatalogoDataService } from './services/item-catalogo.service';
import { PlanoTrabalhoDataService } from './services/plano-trabalho.service';
import { PactoTrabalhoDataService } from './services/pacto-trabalho.service';

// Components:
import { CatalogoPesquisaComponent } from './components/catalogo/catalogo-pesquisa.component';
import { CatalogoCadastroComponent } from './components/catalogo/cadastro/catalogo-cadastro.component';
import { CatalogoEdicaoComponent } from './components/catalogo/edicao/catalogo-edicao.component';
import { ItemCatalogoPesquisaComponent } from './components/item-catalogo/item-catalogo-pesquisa.component';
import { ItemCatalogoCadastroComponent } from './components/item-catalogo/cadastro/item-catalogo-cadastro.component';
import { ItemCatalogoDetalhesComponent } from './components/item-catalogo/detalhes/item-catalogo-detalhes.component';
import { PlanoTrabalhoPesquisaComponent } from './components/plano-trabalho/plano-trabalho-pesquisa.component';
import { PlanoTrabalhoCadastroComponent } from './components/plano-trabalho/cadastro/plano-trabalho-cadastro.component';
import { PlanoTrabalhoDetalhesComponent } from './components/plano-trabalho/detalhes/plano-trabalho-detalhes.component';
import { PlanoListaPactoTrabalhoComponent } from './components/plano-trabalho/_partials/pacto-trabalho/pacto-trabalho-lista.component';
import { PlanoListaAtividadeComponent } from './components/plano-trabalho/_partials/atividade/atividade-lista.component';
import { PlanoListaObjetoComponent } from './components/plano-trabalho/_partials/objetos/objeto-lista.component';
import { PlanoListaMetaIndicadorComponent } from './components/plano-trabalho/_partials/meta-indicador/meta-indicador.component';
import { PlanoCronogramaComponent } from './components/plano-trabalho/_partials/cronograma/cronograma.component';
import { PlanoCustoComponent } from './components/plano-trabalho/_partials/custo/custo.component';
import { PlanoEmpresasComponent } from './components/plano-trabalho/_partials/empresas/empresas.component';
import { PlanoAvaliacaoResultadoComponent } from './components/plano-trabalho/_partials/avaliacao-resultados/avaliacao-resultados.component';
import { PactoTrabalhoPesquisaComponent } from './components/pacto-trabalho/pacto-trabalho-pesquisa.component';
import { PactoTrabalhoCadastroComponent } from './components/pacto-trabalho/cadastro/pacto-trabalho-cadastro.component';
import { PactoTrabalhoDetalhesComponent } from './components/pacto-trabalho/detalhes/pacto-trabalho-detalhes.component';
import { PactoAvaliacaoResultadoComponent } from './components/pacto-trabalho/_partials/avaliacao-resultados/avaliacao-resultados.component';
import { AtividadesPactoAtualComponent } from './components/atividades-servidor/pacto-atual/atividades-pacto-atual.component';
import { AtividadesServidorHistoricoComponent } from './components/atividades-servidor/historico/atividades-servidor-historico.component';
import { PlanoHistoricoComponent } from './components/plano-trabalho/_partials/historico/historico.component';
import { PactoListaAtividadeComponent } from './components/pacto-trabalho/_partials/atividade/atividade-lista.component';
import { PactoListaAtividadeAndamentoComponent } from './components/pacto-trabalho/_partials/atividade-andamento/atividade-andamento.component';
import { AtividadesPactoKanbanComponent } from './components/atividades-servidor/_partials/kanban/atividade-kanban.component';
import { PactoHistoricoComponent } from './components/pacto-trabalho/_partials/historico/historico.component';
import { PactoCabecalhoComponent } from './components/pacto-trabalho/_partials/cabecalho/cabecalho.component';
import { AtividadesPactoNovaComponent } from './components/pacto-trabalho/_partials/solicitacao/atividade-nova/atividade-nova.component';
import { PactoListaSolicitacaoComponent } from './components/pacto-trabalho/_partials/solicitacao/solicitacao-lista.component';
import { AlteracaoPrazoPactoComponent } from './components/pacto-trabalho/_partials/solicitacao/alteracao-prazo/alteracao-prazo.component';
import { PactoDetalhesSolicitacaoComponent } from './components/pacto-trabalho/_partials/solicitacao/detalhes/detalhes-solicitacao.component';
import { PlanoAtividadeCadastroComponent } from './components/plano-trabalho/_partials/atividade/cadastro/atividade-cadastro.component';
import { PlanoAtividadeCandidaturaComponent } from './components/plano-trabalho/_partials/atividade/candidatura/atividade-candidatura.component';
import { PlanoListaAtividadeCandidatoComponent } from './components/plano-trabalho/_partials/atividade/candidato/atividade-lista-candidato.component';
import { PlanoHabilitacaoComponent } from './components/atividades-servidor/plano-habilitacao/plano-habilitacao.component';
import { DashboardComponent } from '../dashboard/components/dashboard.component';
import { JustificarEstouroPrazoPactoComponent } from './components/pacto-trabalho/_partials/solicitacao/justificar-estouro-prazo/justificar-estouro-prazo.component';
import { AssuntosAssociadosComponent } from './components/item-catalogo/cadastro/assuntos-associados/assuntos-associados.component';
import { PactoEmpresasComponent } from './components/pacto-trabalho/_partials/empresas/empresas.component';
import { PlanoObjetoCadastroComponent } from './components/plano-trabalho/_partials/objetos/cadastro/objeto-cadastro.component';
import { AtividadeExcluirComponent } from './components/pacto-trabalho/_partials/solicitacao/atividade-excluir/atividade-excluir.component';
import { AgendamentoPresencialComponent } from './components/agendamento-presencial/agendamento-presencial.component';

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
    DashboardComponent,
    CatalogoPesquisaComponent,
    CatalogoCadastroComponent,
    CatalogoEdicaoComponent,
    ItemCatalogoPesquisaComponent,
    ItemCatalogoCadastroComponent,
    ItemCatalogoDetalhesComponent,
    PlanoTrabalhoPesquisaComponent,
    PlanoTrabalhoCadastroComponent,    
    PlanoTrabalhoDetalhesComponent,
    PactoTrabalhoPesquisaComponent,
    PactoTrabalhoCadastroComponent,
    PactoTrabalhoDetalhesComponent,
    AtividadesPactoAtualComponent,
    AtividadesServidorHistoricoComponent,
    PlanoListaAtividadeCandidatoComponent,
    PlanoListaPactoTrabalhoComponent,
    PlanoListaAtividadeComponent,
    PlanoListaMetaIndicadorComponent,
    PlanoListaObjetoComponent,
    PlanoObjetoCadastroComponent,
    PlanoCronogramaComponent,
    PlanoAvaliacaoResultadoComponent,
    PlanoHistoricoComponent,
    PlanoAtividadeCadastroComponent,
    PlanoCustoComponent,
    PlanoEmpresasComponent,
    PactoListaAtividadeComponent,
    PactoListaAtividadeAndamentoComponent,
    PactoAvaliacaoResultadoComponent,
    PactoHistoricoComponent,
    PactoCabecalhoComponent,
    PactoEmpresasComponent,
    PlanoAtividadeCandidaturaComponent,
    AtividadesPactoKanbanComponent,
    AtividadesPactoNovaComponent,
    PactoListaSolicitacaoComponent,
    AlteracaoPrazoPactoComponent,
    JustificarEstouroPrazoPactoComponent,
    AtividadeExcluirComponent,
    PactoDetalhesSolicitacaoComponent,
    PlanoHabilitacaoComponent,
    AssuntosAssociadosComponent,
    AgendamentoPresencialComponent,
  ],
  exports: [
    DashboardComponent,
    CatalogoPesquisaComponent,
    CatalogoCadastroComponent,
    CatalogoEdicaoComponent,
    ItemCatalogoPesquisaComponent,
    ItemCatalogoCadastroComponent,
    PlanoTrabalhoPesquisaComponent,
    PlanoTrabalhoCadastroComponent,
    PlanoHistoricoComponent,
    PactoHistoricoComponent,
    PactoCabecalhoComponent,
    PlanoHabilitacaoComponent,
    PactoTrabalhoPesquisaComponent,
    PactoTrabalhoCadastroComponent,
    PactoTrabalhoDetalhesComponent,
    AtividadesPactoAtualComponent,
    AtividadesServidorHistoricoComponent
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    CatalogoDataService,
    ItemCatalogoDataService,
    PlanoTrabalhoDataService,
    PactoTrabalhoDataService
  ]
})
export class ProgramaGestaoModule { }


