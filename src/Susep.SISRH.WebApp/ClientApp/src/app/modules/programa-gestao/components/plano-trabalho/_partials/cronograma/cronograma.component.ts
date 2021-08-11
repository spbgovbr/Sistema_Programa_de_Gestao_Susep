import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { IPlanoTrabalho, IPlanoTrabalhoReuniao } from '../../../../models/plano-trabalho.model';
import { BehaviorSubject } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PlanoTrabalhoDataService } from '../../../../services/plano-trabalho.service';
import { PlanoTrabalhoSituacaoEnum } from '../../../../enums/plano-trabalho-situacao.enum';
import { PerfilEnum } from '../../../../enums/perfil.enum';
import { minDateValidator } from 'src/app/shared/functions/validations.function';

@Component({
  selector: 'plano-cronograma',
  templateUrl: './cronograma.component.html',
})
export class PlanoCronogramaComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  PlanoTrabalhoSituacaoEnum = PlanoTrabalhoSituacaoEnum;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;
  unidade = new BehaviorSubject<number>(null);

  @ViewChild('modalCadastro', { static: true }) modalCadastro;

  form: FormGroup;

  reuniaoEdicao: IPlanoTrabalhoReuniao = {};

  minDate = new Date();

  isReadOnly: boolean;

  constructor(
    private router: Router,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private planoTrabalhoDataService: PlanoTrabalhoDataService, ) {
  }

  ngOnInit() {

    this.dadosPlano.subscribe(val => this.carregarReunioes());

    this.form = this.formBuilder.group({
      titulo: ['', [Validators.required]],
      data: ['', [Validators.required, minDateValidator(this.minDate)]],
      descricao: ['', []],
    });
  }

  fillForm() {
    this.form.patchValue({
      titulo: this.reuniaoEdicao.titulo,
      data: this.reuniaoEdicao.data,
      descricao: this.reuniaoEdicao.descricao,
    });
  }

  carregarReunioes() {
    this.unidade.next(this.dadosPlano.value.unidadeId);
    this.planoTrabalhoDataService.ObterReunioes(this.dadosPlano.value.planoTrabalhoId).subscribe(
      resultado => {
        this.dadosPlano.value.reunioes = resultado.retorno;
      }
    );
    this.isReadOnly = this.dadosPlano.value.situacaoId === PlanoTrabalhoSituacaoEnum.Executado ||
                      this.dadosPlano.value.situacaoId === PlanoTrabalhoSituacaoEnum.Concluído;
  }

  abrirTelaCadastro() {
    this.fillForm();
    this.modalService.open(this.modalCadastro, { size: 'sm' });
  }

  cadastrarReuniao() {

    if (this.form.valid) {
      const dados: IPlanoTrabalhoReuniao = this.form.value;
      dados.planoTrabalhoId = this.dadosPlano.value.planoTrabalhoId;
      if (this.reuniaoEdicao && this.reuniaoEdicao.planoTrabalhoReuniaoId) {
        dados.planoTrabalhoReuniaoId = this.reuniaoEdicao.planoTrabalhoReuniaoId;
        this.planoTrabalhoDataService.AlterarReuniao(dados).subscribe(
          r => {
            this.carregarReunioes();
            this.fecharModal();
          });
      }
      else {
        this.planoTrabalhoDataService.CadastrarReuniao(dados).subscribe(
          r => {
            this.carregarReunioes();
            this.fecharModal();
          });
      }
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

  editar(planoTrabalhoReuniaoId: string) {
    this.reuniaoEdicao = this.dadosPlano.value.reunioes.filter(a => a.planoTrabalhoReuniaoId === planoTrabalhoReuniaoId)[0];
    this.fillForm();
    this.abrirTelaCadastro();
  }

  excluir(planoTrabalhoReuniaoId: string) {
    this.planoTrabalhoDataService.ExcluirReuniao(this.dadosPlano.value.planoTrabalhoId, planoTrabalhoReuniaoId).subscribe(
      appResult => {
        this.carregarReunioes();
      }
    );
  }

  fecharModal() {
    this.reuniaoEdicao = {};
    this.modalService.dismissAll();
  }


}
