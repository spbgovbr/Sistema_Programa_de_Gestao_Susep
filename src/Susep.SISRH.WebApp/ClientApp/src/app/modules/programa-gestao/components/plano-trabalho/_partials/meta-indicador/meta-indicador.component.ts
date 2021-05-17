import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { IPlanoTrabalhoMeta, IPlanoTrabalho } from '../../../../models/plano-trabalho.model';
import { BehaviorSubject } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PlanoTrabalhoDataService } from '../../../../services/plano-trabalho.service';
import { PlanoTrabalhoSituacaoEnum } from '../../../../enums/plano-trabalho-situacao.enum';
import { PerfilEnum } from '../../../../enums/perfil.enum';

@Component({
  selector: 'plano-lista-meta-indicador',
  templateUrl: './meta-indicador.component.html',  
})
export class PlanoListaMetaIndicadorComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  PlanoTrabalhoSituacaoEnum = PlanoTrabalhoSituacaoEnum;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;
  unidade = new BehaviorSubject<number>(null);

  @ViewChild('modalCadastro', { static: true }) modalCadastro;

  form: FormGroup;

  metaEdicao: IPlanoTrabalhoMeta = {};

  isReadOnly: boolean;

  constructor(
    private router: Router,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,) { }

  ngOnInit() {

    this.dadosPlano.subscribe(val => this.carregarMetas());

    this.form = this.formBuilder.group({
      meta: ['', [Validators.required]],
      indicador: ['', [Validators.required]],
      descricao: ['', []],
    });
  }

  fillForm() {
    this.form.patchValue({
      meta: this.metaEdicao.meta,
      indicador: this.metaEdicao.indicador,
      descricao: this.metaEdicao.descricao,
    });
  }

  carregarMetas() {
    this.unidade.next(this.dadosPlano.value.unidadeId);
    this.planoTrabalhoDataService.ObterMetas(this.dadosPlano.value.planoTrabalhoId).subscribe(
      resultado => {
        this.dadosPlano.value.metas = resultado.retorno;
      }
    );
    this.isReadOnly = this.dadosPlano.value.situacaoId === PlanoTrabalhoSituacaoEnum.Executado ||
                      this.dadosPlano.value.situacaoId === PlanoTrabalhoSituacaoEnum.Concluído;
  }

  abrirTelaCadastro() {
    this.fillForm();
    this.modalService.open(this.modalCadastro, { size: 'sm' });
  }

  cadastrarMeta() {
    
    if (this.form.valid) {
      const dados: IPlanoTrabalhoMeta = this.form.value;
      dados.planoTrabalhoId = this.dadosPlano.value.planoTrabalhoId;
      if (this.metaEdicao && this.metaEdicao.planoTrabalhoMetaId) {
        dados.planoTrabalhoMetaId = this.metaEdicao.planoTrabalhoMetaId;
        this.planoTrabalhoDataService.AlterarMeta(dados).subscribe(
          r => {
            this.carregarMetas();
            this.fecharModal();
          });
      }
      else {
        this.planoTrabalhoDataService.CadastrarMeta(dados).subscribe(
          r => {
            this.carregarMetas();
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

  editar(planoTrabalhoMetaId: string) {
    this.metaEdicao = this.dadosPlano.value.metas.filter(a => a.planoTrabalhoMetaId === planoTrabalhoMetaId)[0];
    this.fillForm();
    this.abrirTelaCadastro();
  }

  excluir(planoTrabalhoMetaId: string) {
    this.planoTrabalhoDataService.ExcluirMeta(this.dadosPlano.value.planoTrabalhoId, planoTrabalhoMetaId).subscribe(
      appResult => {
        this.carregarMetas();
      }
    );
  }

  fecharModal() {
    this.metaEdicao = {};
    this.modalService.dismissAll();
  }

}
