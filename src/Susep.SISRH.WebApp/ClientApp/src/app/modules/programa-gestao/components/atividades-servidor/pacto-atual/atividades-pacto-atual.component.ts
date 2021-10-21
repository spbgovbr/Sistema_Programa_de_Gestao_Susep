import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { IPactoTrabalho, IPactoTrabalhoAtividade } from '../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../services/pacto-trabalho.service';
import { DataService } from '../../../../../shared/services/data.service';
import { PerfilEnum } from '../../../enums/perfil.enum';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSlideToggleChange } from '@angular/material';
import { PlanoTrabalhoDataService } from '../../../services/plano-trabalho.service';
import { IUsuario } from '../../../../../shared/models/perfil-usuario.model';
import { ApplicationStateService } from '../../../../../shared/services/application.state.service';

@Component({
  selector: 'atividades-pacto-atual',
  templateUrl: './atividades-pacto-atual.component.html',
  styleUrls: ['./atividades-pacto-atual.component.css'],  
})
export class AtividadesPactoAtualComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  abaVisivel = 'lista';
  classeTextoSituacao = 'text-danger';

  @ViewChild('modalAceite', { static: true }) modalAceite;
  @ViewChild('modalAceiteComTermoAceite', { static: true }) modalAceiteComTermoAceite;
  @ViewChild('modalFluxo', { static: true }) modalFluxo;
  @ViewChild('modalConcluirExecucao', { static: true }) modalConcluirExecucao;

  dadosPacto = new BehaviorSubject<IPactoTrabalho>(null);
  atividades = new BehaviorSubject<IPactoTrabalhoAtividade[]>(null);
  unidade = new BehaviorSubject<number>(null);
  servidor = new BehaviorSubject<number>(null);

  periodoExecucao: boolean;

  perfilUsuario: IUsuario;
  gestorUnidade: boolean;
  usuarioPodeAceitar: boolean;

  termoAceite: string;
  aceitouTermos: boolean;

  form: FormGroup;

  constructor(
    public router: Router,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private dataService: DataService,
    private activatedRoute: ActivatedRoute,
    private applicationState: ApplicationStateService,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,
    private pactoTrabalhoDataService: PactoTrabalhoDataService) { }

  ngOnInit() {
    this.carregarDadosPacto();

    this.applicationState.perfilUsuario.subscribe(appResult => {
      this.perfilUsuario = appResult;
      this.gestorUnidade = this.perfilUsuario.perfis.filter(p =>
        p.perfil === PerfilEnum.Gestor ||
        p.perfil === PerfilEnum.Administrador ||
        p.perfil === PerfilEnum.Diretor ||
        p.perfil === PerfilEnum.CoordenadorGeral ||
        p.perfil === PerfilEnum.ChefeUnidade).length > 0;
    });

    this.form = this.formBuilder.group({
      descricao: ['', [Validators.required]],
    });
  }

  fillForm(descricao: string) {
    this.form.patchValue({
      descricao: descricao,
    });
  }

  carregarDadosPacto() {
    const planoTrabalhoId = this.activatedRoute.snapshot.paramMap.get('id');

    if (!planoTrabalhoId) {
      this.pactoTrabalhoDataService.ObterPactoAtual().subscribe(
        resultado => {
          if (resultado.retorno) {
            this.unidade.next(resultado.retorno.unidadeId);

            this.preencherPlano(resultado.retorno);
            this.verificarSeUsuarioPodeAceitar();
          }
        }
      );
    }
    else {
      this.pactoTrabalhoDataService.ObterPacto(planoTrabalhoId).subscribe(
        resultado => {
          if (resultado.retorno) {
            this.unidade.next(resultado.retorno.unidadeId);

            this.preencherPlano(resultado.retorno);
            this.verificarSeUsuarioPodeAceitar();
          }
        }
      );
    }
  }

  verificarSeUsuarioPodeAceitar() {
    this.usuarioPodeAceitar =
      (this.perfilUsuario.pessoaId === this.dadosPacto.value.pessoaId &&
        this.perfilUsuario.pessoaId.toString() !== this.dadosPacto.value.responsavelEnvioAceite.toString()) ||
      (this.perfilUsuario.pessoaId !== this.dadosPacto.value.pessoaId &&
        this.dadosPacto.value.pessoaId.toString() === this.dadosPacto.value.responsavelEnvioAceite.toString());
  }

  preencherPlano(dados: IPactoTrabalho) {
    this.dadosPacto.next(dados);
    this.atividades.next(dados.atividades);
    this.servidor.next(dados.pessoaId);


    this.definirClasseTextoSituacao();

    this.periodoExecucao = this.dataService.formatAsDate(new Date()) >= this.dataService.formatAsDate(this.dadosPacto.value.dataInicio);
  }

  definirClasseTextoSituacao() {
    if (this.dadosPacto.value) {
      switch (this.dadosPacto.value.situacaoId) {
        case 401: this.classeTextoSituacao = 'text-gray-5'; break;
        case 402: this.classeTextoSituacao = 'text-warning'; break;
        case 403: this.classeTextoSituacao = 'text-success'; break;
        case 404: this.classeTextoSituacao = 'text-orange'; break;
        case 405: this.classeTextoSituacao = 'text-teal'; break;
        case 406: this.classeTextoSituacao = 'text-primary-lighten-25'; break;
        case 407: this.classeTextoSituacao = 'text-primary-darken-25'; break;
      }
    }
  }

  iniciarExecucao() {
    this.pactoTrabalhoDataService.IniciarExecucao(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.carregarDadosPacto();
      }
    );
  }

  abrirTelConcluirExecucao() {
    this.modalService.open(this.modalConcluirExecucao, { size: 'sm' });
  }

  concluirExecucao() {
    this.pactoTrabalhoDataService.ConcluirExecucao(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.pactoTrabalhoDataService.ObterPacto(this.dadosPacto.value.pactoTrabalhoId).subscribe(
          resultado => {
            if (resultado.retorno) {
              this.dadosPacto.next(resultado.retorno);
            }
          });      
      }
    );
  }

  aceitar() {    
    if (this.dadosPacto.value.pessoaId === this.perfilUsuario.pessoaId) {

      this.planoTrabalhoDataService.ObterTermoAceite(this.dadosPacto.value.planoTrabalhoId).subscribe(
        r => {
          this.termoAceite = r.retorno.termoAceite;
          this.modalService.open(this.modalAceiteComTermoAceite, { size: 'sm' });
        });
    }
    else {
      this.modalService.open(this.modalAceite, { size: 'sm' });
    }
  }

  toggleSituacao(event: MatSlideToggleChange) {
    this.aceitouTermos = event.checked;
  }

  confirmarAceite() {
    this.pactoTrabalhoDataService.Aceitar(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.carregarDadosPacto();
      }
    );
  }

  rejeitar() {
    this.fillForm('');
    this.modalService.open(this.modalFluxo, { size: 'sm' });
  }

  confirmarRejeicao() {

    if (this.form.valid) {
      this.pactoTrabalhoDataService.Rejeitar(this.dadosPacto.value.pactoTrabalhoId, this.form.get('descricao').value).subscribe(
        resultado => {
          this.carregarDadosPacto();
        }
      );

      this.fecharModal();
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

  alterarAba(aba: string) {
    this.abaVisivel = aba;
  }

  voltarParaPlano() {
    this.router.navigateByUrl(`/programagestao/detalhar/${this.dadosPacto.value.planoTrabalhoId}`);
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

}
