import { Component, OnInit } from '@angular/core';
import { PessoaDataService } from '../../pessoa/services/pessoa.service';
import { IDashboard } from '../models/dashboard.model';
import { PerfilEnum } from '../../programa-gestao/enums/perfil.enum';
import { PlanoTrabalhoSituacaoEnum } from '../../programa-gestao/enums/plano-trabalho-situacao.enum';
import { ApplicationStateService } from '../../../shared/services/application.state.service';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',  
})
export class DashboardComponent implements OnInit {

  dashboard: IDashboard;
  PerfilEnum = PerfilEnum;
  PlanoTrabalhoSituacaoEnum = PlanoTrabalhoSituacaoEnum;

  chefe: boolean;
  usuarioLogadoId: number;

  constructor(
    private pessoaDataService: PessoaDataService,
    private applicationState: ApplicationStateService) {
  }

  ngOnInit() {
    this.pessoaDataService.ObterDashboard().subscribe(
      appResult => {
        this.dashboard = appResult.retorno;
      }
    );

    this.applicationState.perfilUsuario.subscribe(perfil => {
      this.usuarioLogadoId = perfil.pessoaId;

      this.chefe = perfil.perfis.filter(p =>
        p.perfil === PerfilEnum.Gestor ||
        p.perfil === PerfilEnum.Administrador ||
        p.perfil === PerfilEnum.Diretor ||
        p.perfil === PerfilEnum.CoordenadorGeral ||
        p.perfil === PerfilEnum.ChefeUnidade).length > 0;
    });

    
  }

}
