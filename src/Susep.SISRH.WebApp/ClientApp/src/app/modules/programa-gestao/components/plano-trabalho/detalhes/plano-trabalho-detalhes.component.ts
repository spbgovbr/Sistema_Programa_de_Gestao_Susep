import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { PlanoTrabalhoSituacaoEnum } from '../../../enums/plano-trabalho-situacao.enum';
import { IPlanoTrabalho } from '../../../models/plano-trabalho.model';
import { PlanoTrabalhoDataService } from '../../../services/plano-trabalho.service';
import { PerfilEnum } from '../../../enums/perfil.enum';
import { ApplicationStateService } from '../../../../../shared/services/application.state.service';
import { ConfigurationService } from 'src/app/shared/services/configuration.service';
import { TipoModo } from 'src/app/shared/models/configuration.model';

@Component({
  selector: 'plano-trabalho-detalhes',
  templateUrl: './plano-trabalho-detalhes.component.html',
  styleUrls: ['./plano-trabalho-detalhes.component.css'],  
})
export class PlanoTrabalhoDetalhesComponent implements OnInit {

  @ViewChild('modalInicioHabilitacao', { static: true }) modalInicioHabilitacao;
  @ViewChild('modalConcluirExecucao', { static: true }) modalConcluirExecucao;
  @ViewChild('modalTermoAceite', { static: true }) modalTermoAceite;  

  PerfilEnum = PerfilEnum;
  PlanoTrabalhoSituacaoEnum = PlanoTrabalhoSituacaoEnum;

  abaVisivel = 'atividades';
  classeTextoSituacao = 'text-danger';

  dadosPlano = new BehaviorSubject<IPlanoTrabalho>(null);
  unidade = new BehaviorSubject<number>(null);

  pactoNaoExecutado: boolean;
  quantidadeCandidatosAprovados: 0;

  modo: TipoModo;

  form: FormGroup;

  constructor(
    public router: Router,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,
    private applicationStateService: ApplicationStateService,
    private configurationService: ConfigurationService
  ) { }

  ngOnInit() {
    this.modo = this.configurationService.getModo();

    this.carregarDados();

    this.form = this.formBuilder.group({
      descricao: ['', [Validators.required]],
    });
  }

  fillForm(descricao: string) {
    this.form.patchValue({
      descricao: descricao,
    });
  }

  carregarDados() {
    const planoTrabalhoId = this.activatedRoute.snapshot.paramMap.get('id');
    this.dadosPlano.next({ 'planoTrabalhoId': planoTrabalhoId });
    //this.dadosPlanoMessage = new BehaviorSubject<IPlanoTrabalho>(null);
    //this.dadosPlanoMessage.next(this.dadosPlano);

    this.carregarDadosPlano();
  }

  carregarDadosPlano() {
    this.planoTrabalhoDataService.ObterPlano(this.dadosPlano.value.planoTrabalhoId).subscribe(
      resultado => {
        this.dadosPlano.next(resultado.retorno);
        if (this.dadosPlano.value)
          this.unidade.next(this.dadosPlano.value.unidadeId);

        this.alterarAba('atividades');
        this.definirClasseTextoSituacao();

        //this.quantidadeCandidatosAprovados = this.dadosPlano.value.atividades.filter(a => a.)
      }
    );
  }

  alterarAba(aba: string) {
    this.abaVisivel = aba;
  }

  definirClasseTextoSituacao() {
    switch (this.dadosPlano.value.situacaoId) {
      case PlanoTrabalhoSituacaoEnum.Rascunho: this.classeTextoSituacao = 'text-gray-5'; break;
      case PlanoTrabalhoSituacaoEnum.EnviadoParaAprovacao: this.classeTextoSituacao = 'text-warning'; break;
      case PlanoTrabalhoSituacaoEnum.Rejeitado: this.classeTextoSituacao = 'text-danger'; break;
      case PlanoTrabalhoSituacaoEnum.AprovadoIndicadores: this.classeTextoSituacao = 'text-secondary'; break;
      case PlanoTrabalhoSituacaoEnum.AprovadoGestaoPessoas: this.classeTextoSituacao = 'text-teal'; break;
      case PlanoTrabalhoSituacaoEnum.Aprovado: this.classeTextoSituacao = 'text-success'; break;
      case PlanoTrabalhoSituacaoEnum.Habilitacao: this.classeTextoSituacao = 'text-brown'; break;
      case PlanoTrabalhoSituacaoEnum.ProntoParaExecucao: this.classeTextoSituacao = 'text-teal'; break;
      case PlanoTrabalhoSituacaoEnum.EmExecucao: this.classeTextoSituacao = 'text-primary-lighten-15'; break;
      case PlanoTrabalhoSituacaoEnum.Executado: this.classeTextoSituacao = 'text-primary-darken-15'; break;
      case PlanoTrabalhoSituacaoEnum.Concluído: this.classeTextoSituacao = 'text-primary-darken-25'; break;
    }
  }

  enviarParaAprovacao() {
    this.planoTrabalhoDataService.AlterarFase(this.dadosPlano.value.planoTrabalhoId, PlanoTrabalhoSituacaoEnum.EnviadoParaAprovacao).subscribe(
      resultado => {
        this.carregarDadosPlano();
      }
    );
  }

  aprovarIndicadores() {
    this.planoTrabalhoDataService.AlterarFase(this.dadosPlano.value.planoTrabalhoId, PlanoTrabalhoSituacaoEnum.AprovadoIndicadores).subscribe(
      resultado => {
        this.carregarDadosPlano();
      }
    );
  }

  aprovarGP() {
    this.planoTrabalhoDataService.AlterarFase(this.dadosPlano.value.planoTrabalhoId, PlanoTrabalhoSituacaoEnum.AprovadoGestaoPessoas).subscribe(
      resultado => {
        this.carregarDadosPlano();
      }
    );
  }

  aprovar() {
    this.planoTrabalhoDataService.AlterarFase(this.dadosPlano.value.planoTrabalhoId, PlanoTrabalhoSituacaoEnum.Aprovado).subscribe(
      resultado => {
        this.carregarDadosPlano();
      }
    );
  }

  abrirTelaIniciarHabilitacao() {
    this.modalService.open(this.modalInicioHabilitacao, { size: 'sm' });
  }

  abrirTelConcluirExecucao() {
    this.modalService.open(this.modalConcluirExecucao, { size: 'sm' });
  }

  
  confirmarRejeicao() {   

    if (this.form.valid) {

      this.fecharModal();

      this.planoTrabalhoDataService.AlterarFase(this.dadosPlano.value.planoTrabalhoId, PlanoTrabalhoSituacaoEnum.Rejeitado, this.form.get('descricao').value).subscribe(
        resultado => {
          this.carregarDadosPlano();
        }
      );

    }
    else {
      this.getFormValidationErrors(this.form)
    }
  }

  getFormValidationErrors(form) {
    Object.keys(form.controls).forEach(field => {
      const control = form.get(field);
      control.markAsDirty({ onlySelf: true });
    });
  }

  voltarParaRascunho() {
    this.planoTrabalhoDataService.AlterarFase(this.dadosPlano.value.planoTrabalhoId, PlanoTrabalhoSituacaoEnum.Rascunho).subscribe(
      resultado => {
        this.carregarDadosPlano();
      }
    );
  }

  iniciarHabilitacao() {
    this.planoTrabalhoDataService.AlterarFase(this.dadosPlano.value.planoTrabalhoId, PlanoTrabalhoSituacaoEnum.Habilitacao).subscribe(
      resultado => {
        this.carregarDadosPlano();
      }
    );
  }

  concluirHabilitacao() {
    this.applicationStateService.changeOpenModal('modalCandidatos');
  }

  iniciarExecucao() {
    this.planoTrabalhoDataService.AlterarFase(this.dadosPlano.value.planoTrabalhoId, PlanoTrabalhoSituacaoEnum.EmExecucao).subscribe(
      resultado => {
        this.carregarDadosPlano();
      }
    );
  }

  concluirExecucao() {
    this.planoTrabalhoDataService.AlterarFase(this.dadosPlano.value.planoTrabalhoId, PlanoTrabalhoSituacaoEnum.Executado).subscribe(
      resultado => {
        this.carregarDadosPlano();
      }
    );
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

  ExibirTermoAceite() {
    this.planoTrabalhoDataService.ObterTermoAceite(this.dadosPlano.value.planoTrabalhoId).subscribe(r => {
      this.dadosPlano.value.termoAceite = r.retorno.termoAceite;
      this.modalService.open(this.modalTermoAceite, { size: 'sm' });

    });
    

  }
}
