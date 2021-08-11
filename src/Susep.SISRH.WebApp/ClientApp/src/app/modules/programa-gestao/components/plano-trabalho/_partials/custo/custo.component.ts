import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { IPlanoTrabalho, IPlanoTrabalhoCusto } from '../../../../models/plano-trabalho.model';
import { BehaviorSubject } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PlanoTrabalhoDataService } from '../../../../services/plano-trabalho.service';
import { PlanoTrabalhoSituacaoEnum } from '../../../../enums/plano-trabalho-situacao.enum';
import { PerfilEnum } from '../../../../enums/perfil.enum';

@Component({
  selector: 'plano-custo',
  templateUrl: './custo.component.html',
})
export class PlanoCustoComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  PlanoTrabalhoSituacaoEnum = PlanoTrabalhoSituacaoEnum;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;
  unidade = new BehaviorSubject<number>(null);

  @ViewChild('modalCadastro', { static: true }) modalCadastro;

  form: FormGroup;

  custoEdicao: IPlanoTrabalhoCusto = {};

  minDate = new Date();

  isReadOnly: boolean;

  constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private planoTrabalhoDataService: PlanoTrabalhoDataService, ) {
  }

  ngOnInit() {

    this.dadosPlano.subscribe(val => this.carregarCustos());

    this.form = this.formBuilder.group({
      valor: ['', [Validators.required]],
      descricao: ['', []],
    });
  }

  fillForm() {
    this.form.patchValue({
      valor: this.custoEdicao.valor,
      descricao: this.custoEdicao.descricao,
    });
  }

  carregarCustos() {
    this.unidade.next(this.dadosPlano.value.unidadeId);
    this.planoTrabalhoDataService.ObterCustos(this.dadosPlano.value.planoTrabalhoId).subscribe(
      resultado => {
        this.dadosPlano.value.custos = resultado.retorno;
      }
    );
    this.isReadOnly = this.dadosPlano.value.situacaoId === PlanoTrabalhoSituacaoEnum.Executado ||
                      this.dadosPlano.value.situacaoId === PlanoTrabalhoSituacaoEnum.Concluído;
  }

  abrirTelaCadastro() {
    this.fillForm();
    this.modalService.open(this.modalCadastro, { size: 'sm' });
  }

  cadastrarCusto() {

    if (this.form.valid) {
      const dados: IPlanoTrabalhoCusto = this.form.value;
      dados.planoTrabalhoId = this.dadosPlano.value.planoTrabalhoId;
      if (this.custoEdicao && this.custoEdicao.planoTrabalhoCustoId) {
        dados.planoTrabalhoCustoId = this.custoEdicao.planoTrabalhoCustoId;
        this.planoTrabalhoDataService.AlterarCusto(dados).subscribe(
          r => {
            this.carregarCustos();
            this.fecharModal();
          });
      }
      else {
        this.planoTrabalhoDataService.CadastrarCusto(dados).subscribe(
          r => {
            this.carregarCustos();
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

  editar(planoTrabalhoCustoId: string) {
    this.custoEdicao = this.dadosPlano.value.custos.filter(a => a.planoTrabalhoCustoId === planoTrabalhoCustoId)[0];
    this.fillForm();
    this.abrirTelaCadastro();
  }

  excluir(planoTrabalhoCustoId: string) {
    this.planoTrabalhoDataService.ExcluirCusto(this.dadosPlano.value.planoTrabalhoId, planoTrabalhoCustoId).subscribe(
      appResult => {
        this.carregarCustos();
      }
    );
  }

  fecharModal() {
    this.custoEdicao = {};
    this.modalService.dismissAll();
  }


}
