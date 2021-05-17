import { ModuleWithProviders } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { AuthGuard } from "./shared/helpers/authorization.guard.helper";

import { HomeComponent } from "./shared/components/home/home.component";
import { LoginComponent } from "./shared/components/login/login.component";

import { CatalogoPesquisaComponent } from "./modules/programa-gestao/components/catalogo/catalogo-pesquisa.component";
import { CatalogoCadastroComponent } from "./modules/programa-gestao/components/catalogo/cadastro/catalogo-cadastro.component";
import { CatalogoEdicaoComponent } from "./modules/programa-gestao/components/catalogo/edicao/catalogo-edicao.component";
import { ItemCatalogoCadastroComponent } from "./modules/programa-gestao/components/item-catalogo/cadastro/item-catalogo-cadastro.component";
import { ItemCatalogoPesquisaComponent } from "./modules/programa-gestao/components/item-catalogo/item-catalogo-pesquisa.component";
import { ItemCatalogoDetalhesComponent } from "./modules/programa-gestao/components/item-catalogo/detalhes/item-catalogo-detalhes.component";
import { PlanoTrabalhoPesquisaComponent } from "./modules/programa-gestao/components/plano-trabalho/plano-trabalho-pesquisa.component";
import { PlanoTrabalhoCadastroComponent } from "./modules/programa-gestao/components/plano-trabalho/cadastro/plano-trabalho-cadastro.component";
import { PlanoTrabalhoDetalhesComponent } from "./modules/programa-gestao/components/plano-trabalho/detalhes/plano-trabalho-detalhes.component";
import { PactoTrabalhoPesquisaComponent } from "./modules/programa-gestao/components/pacto-trabalho/pacto-trabalho-pesquisa.component";
import { PactoTrabalhoCadastroComponent } from "./modules/programa-gestao/components/pacto-trabalho/cadastro/pacto-trabalho-cadastro.component";
import { PactoTrabalhoDetalhesComponent } from "./modules/programa-gestao/components/pacto-trabalho/detalhes/pacto-trabalho-detalhes.component";
import { AtividadesPactoAtualComponent } from "./modules/programa-gestao/components/atividades-servidor/pacto-atual/atividades-pacto-atual.component";
import { AtividadesServidorHistoricoComponent } from "./modules/programa-gestao/components/atividades-servidor/historico/atividades-servidor-historico.component";
import { PessoaPesquisaComponent } from "./modules/pessoa/components/pessoa-pesquisa.component";
import { PessoaEdicaoComponent } from "./modules/pessoa/components/edicao/pessoa-edicao.component";
import { UnidadePesquisaComponent } from "./modules/unidade/components/unidade-pesquisa.component";
import { UnidadeEdicaoComponent } from "./modules/unidade/components/edicao/unidade-edicao.component";
import { PerfilEnum } from "./modules/programa-gestao/enums/perfil.enum";
import { PlanoHabilitacaoComponent } from "./modules/programa-gestao/components/atividades-servidor/plano-habilitacao/plano-habilitacao.component";
import { DashboardComponent } from "./modules/dashboard/components/dashboard.component";
import { AssuntoPesquisaComponent } from "./modules/assunto/components/assunto-pesquisa.component";
import { AssuntoEdicaoComponent } from "./modules/assunto/components/edicao/assunto-edicao.component";
import { ModoExibicaoGuard } from "./shared/helpers/modo-exibicao.guard.helper";
import { ObjetoPesquisaComponent } from "./modules/objeto/components/objeto-pesquisa.component";
import { ObjetoEdicaoComponent } from "./modules/objeto/components/edicao/objeto-edicao.component";


const routes: Routes = [
  { path: '', component: HomeComponent, data: { breadcrumb: 'Home' }, pathMatch: 'full' },
  { path: 'login', component: LoginComponent, data: { breadcrumb: 'Login' }, pathMatch: 'full' },  
  { path: 'dashboard', canActivate: [AuthGuard], component: DashboardComponent, data: { breadcrumb: 'Dashboard' }, pathMatch: 'full' },  
  {
    path: 'programagestao', canActivate: [AuthGuard], data: { breadcrumb: 'Programa de gestão' }, children: [
      { path: '', component: PlanoTrabalhoPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
      { path: 'pesquisa', component: PlanoTrabalhoPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
      { path: 'cadastro', component: PlanoTrabalhoCadastroComponent, data: { breadcrumb: 'Cadastro', roles: [PerfilEnum.Diretor, PerfilEnum.CoordenadorGeral, PerfilEnum.ChefeUnidade] } },
      { path: 'detalhar/:id', component: PlanoTrabalhoDetalhesComponent, data: { breadcrumb: 'Detalhes' } },     
      {
        path: 'catalogo', data: { breadcrumb: 'Lista de atividade', roles: [PerfilEnum.GestorIndicadores] }, children: [      
          { path: '', component: CatalogoPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
          { path: 'pesquisa', component: CatalogoPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
          { path: 'cadastro', component: CatalogoCadastroComponent, data: { breadcrumb: 'Cadastro' } },
          { path: 'editar/:id', component: CatalogoEdicaoComponent, data: { breadcrumb: 'Editar' } },
          {
            path: 'item', data: { breadcrumb: 'Atividade' }, children: [
              { path: '', component: ItemCatalogoPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
              { path: 'pesquisa', component: ItemCatalogoPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
              { path: 'cadastro', component: ItemCatalogoCadastroComponent, data: { breadcrumb: 'Cadastro' } },
              { path: 'editar/:id', component: ItemCatalogoCadastroComponent, data: { breadcrumb: 'Editar' } },
              { path: 'copiar/:id', component: ItemCatalogoCadastroComponent, data: { breadcrumb: 'Copiar' } },
              { path: 'detalhar/:id', component: ItemCatalogoDetalhesComponent, data: { breadcrumb: 'Detalhes' } },  
              { path: 'excluir/:id', component: ItemCatalogoDetalhesComponent, data: { breadcrumb: 'Excluir' } }             
            ]
          },
        ]
      },  
      {
        path: 'pactotrabalho', data: { breadcrumb: 'Plano de trabalho' }, children: [      
          { path: '', component: PactoTrabalhoPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
          { path: 'pesquisa', component: PactoTrabalhoPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
          { path: 'cadastro', component: PactoTrabalhoCadastroComponent, data: { breadcrumb: 'Cadastro' } },
          { path: 'detalhar/:id', component: PactoTrabalhoDetalhesComponent, data: { breadcrumb: 'Detalhes' } },  
        ]
      },
      {
        path: 'atividade', data: { breadcrumb: 'Atividades' }, children: [      
          { path: '', component: AtividadesPactoAtualComponent, data: { breadcrumb: 'Pacto atual' } },
          { path: 'atual', component: AtividadesPactoAtualComponent, data: { breadcrumb: 'Pacto atual' } },
          { path: 'habilitacao', component: PlanoHabilitacaoComponent, data: { breadcrumb: 'Habilitação' } },
          { path: 'historico', component: AtividadesServidorHistoricoComponent, data: { breadcrumb: 'Meus planos de trabalho' } },
        ]
      },      
    ],
  },  
  {
    path: 'pessoa', canActivate: [AuthGuard], data: { breadcrumb: 'Pessoas', roles: [PerfilEnum.GestorPessoas] }, children: [      
      { path: '', component: PessoaPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
      { path: 'pesquisa', component: PessoaPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
      { path: 'editar/:id', component: PessoaEdicaoComponent, data: { breadcrumb: 'Editar' } },
    ]
  },
  {
    path: 'unidade', canActivate: [AuthGuard], data: { breadcrumb: 'Unidades', roles: [PerfilEnum.GestorPessoas] }, children: [      
      { path: '', component: UnidadePesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
      { path: 'pesquisa', component: UnidadePesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
      { path: 'editar/:id', component: UnidadeEdicaoComponent, data: { breadcrumb: 'Editar' } },
    ]
  },
  {
    path: 'assunto', canActivate: [AuthGuard, ModoExibicaoGuard], data: { breadcrumb: 'Assuntos', roles: [PerfilEnum.GestorPessoas] }, children: [      
      { path: '', component: AssuntoPesquisaComponent, data: { breadcrumb: 'Assunto' } },
      { path: 'pesquisa', component: AssuntoPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
      { path: 'cadastro', component: AssuntoEdicaoComponent, data: { breadcrumb: 'Cadastro' } },
      { path: 'editar/:id', component: AssuntoEdicaoComponent, data: { breadcrumb: 'Editar' } },
    ]
  },
  {
    path: 'objeto', canActivate: [AuthGuard], data: { breadcrumb: 'Objetos', roles: [PerfilEnum.GestorPessoas] }, children: [      
      { path: '', component: ObjetoPesquisaComponent, data: { breadcrumb: 'Objeto' } },
      { path: 'pesquisa', component: ObjetoPesquisaComponent, data: { breadcrumb: 'Pesquisa' } },
      { path: 'cadastro', component: ObjetoEdicaoComponent, data: { breadcrumb: 'Cadastro' } },
      { path: 'editar/:id', component: ObjetoEdicaoComponent, data: { breadcrumb: 'Editar' } },
    ]
  },
];

export const RoutingModule: ModuleWithProviders = RouterModule.forRoot(routes);
