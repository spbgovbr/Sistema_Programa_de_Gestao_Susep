import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { IDominio } from '../../../../../../../shared/models/dominio.model';
import { DominioDataService } from '../../../../../../../shared/services/dominio.service';
import { IPactoTrabalho, IPactoTrabalhoAtividade } from '../../../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../../../services/pacto-trabalho.service';
import { CatalogoDataService } from '../../../../../services/catalogo.service';
import { IItemCatalogo } from '../../../../../models/item-catalogo.model';
import { DecimalValuesHelper } from '../../../../../../../shared/helpers/decimal-valuesr.helper';

@Component({
  selector: 'atividade-pacto-nova',
  templateUrl: './atividade-nova.component.html',
})
export class AtividadesPactoNovaComponent implements OnInit {

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;
  @Input() itensUnidade: BehaviorSubject<IItemCatalogo[]>;
  @Input() situacoesAtividade: BehaviorSubject<IDominio[]>;

  form: FormGroup;
  todasSituacoes: IDominio[];
  situacoes: IDominio[];
  itensCatalogo: IItemCatalogo[];

  situacaoId: number;
  tipoItemSelecionado = 203;
  tempoItemSelecionado = 0;

  exibirTempoPrevisto: boolean;
  exibirDataInicio: boolean;
  exibirDataFim: boolean;
  exibirTempoRealizado: boolean;

  tempoMask: any;
  minDataInicio: any;
  maxDataInicio: any;
  minDataConclusao: any;
  maxDataConclusao: any;

  constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private decimalValuesHelper: DecimalValuesHelper,
    private pactoTrabalhoDataService: PactoTrabalhoDataService,
    private catalogoDataService: CatalogoDataService,
    private dominioDataService: DominioDataService) { }

  ngOnInit() {
    this.tempoMask = this.decimalValuesHelper.numberMask(3, 1);

    //this.dadosPacto.subscribe(val => { if (val) { this.configurarDatas(val.dataInicio, val.dataFim); } });
    this.itensUnidade.subscribe(val => this.carregarItens());
    this.situacoesAtividade.subscribe(val => this.carregarSituacoes());

    this.form = this.formBuilder.group({
      itemCatalogoId: [null, [Validators.required]],
      situacaoId: [null, [Validators.required]],
      tempoPrevistoPorItem: [null, [Validators.required]],
      dataInicio: [null, []],
      dataFim: [null, []],
      tempoRealizado: [null, []],
      descricao: [null, []],
    });

  }

  carregarItens() {
    if (this.itensUnidade.value) {
      let itens = this.itensUnidade.value;
      itens = itens.filter(i => this.dadosPacto.value.formaExecucaoId === 101 || i.permiteTrabalhoRemoto);
      this.itensCatalogo = itens;
    }
  }

  carregarSituacoes() {
    if (this.situacoesAtividade.value) {
      this.todasSituacoes = this.situacoesAtividade.value;
      this.situacoes = this.situacoesAtividade.value;
    }    
  }

  mudarItemCatalogo(value) {
    const itemSelecionado = this.itensCatalogo.filter(i => i.itemCatalogoId === value)[0];
    this.tipoItemSelecionado = itemSelecionado.formaCalculoTempoItemCatalogoId;
    this.tempoItemSelecionado = this.dadosPacto.value.formaExecucaoId === 101 ? itemSelecionado.tempoExecucaoPresencial : itemSelecionado.tempoExecucaoRemoto;    

    if (this.tipoItemSelecionado === 201) {
      this.form.get('dataInicio').setValue(this.dadosPacto.value.dataInicio);
      this.form.get('dataInicio').disable();

      this.form.get('situacaoId').setValue(502);
      this.form.get('situacaoId').disable();

      this.situacaoId = 502;
    }
    else {
      this.situacoes = this.todasSituacoes;
      this.form.get('situacaoId').enable();
    }

    this.configurarCampos();
  }

  mudarSituacao(value) {
    this.situacaoId = value;    
    this.configurarCampos();
  }

  configurarCampos() {
    this.exibirTempoPrevisto = (this.tipoItemSelecionado !== 201) &&
                               !(this.tipoItemSelecionado > 202 && this.situacaoId > 502);
    this.configurarCampo('tempoPrevistoPorItem', this.exibirTempoPrevisto);

    this.exibirDataInicio = this.tipoItemSelecionado !== 201 && this.situacaoId > 501;
    this.configurarCampo('dataInicio', this.exibirDataInicio);

    this.exibirDataFim = this.tipoItemSelecionado !== 201 && this.situacaoId > 502;
    this.configurarCampo('dataFim', this.exibirDataFim);

    this.exibirTempoRealizado = this.tipoItemSelecionado !== 201 && this.situacaoId > 502;
    this.configurarCampo('tempoRealizado', this.exibirTempoRealizado);

    this.minDataConclusao = this.formatarData(new Date(this.getDataInicio()));
    this.maxDataConclusao = this.formatarData(new Date());
    if (this.situacaoId === 501) {
      //Se estiver programada, tem que começar em uma data futura
      this.minDataInicio = this.formatarData(new Date());
      this.maxDataInicio = this.formatarData(new Date(this.dadosPacto.value.dataFim));
    }
    else {
      //Se estiver em execução ou concluída, tem que ter começado após o início do pacto e antes da data atual
      this.minDataInicio = this.formatarData(new Date(this.dadosPacto.value.dataInicio));
      this.maxDataInicio = this.formatarData(new Date())
    }

    this.form.get('tempoPrevistoPorItem').enable();
    if (this.tipoItemSelecionado === 202) {
      this.form.get('tempoPrevistoPorItem').setValue(this.tempoItemSelecionado);
      this.form.get('tempoPrevistoPorItem').disable();
    }
  }

  getDataInicio() {
    if (this.form.get('dataInicio').value)
      return this.form.get('dataInicio').value;
    return this.dadosPacto.value.dataInicio;
  }

  alterarDataInicio() {
    const dataInicio = new Date(this.getDataInicio());
    this.minDataConclusao = this.formatarData(dataInicio);

    if (this.form.get('dataFim').value) {
      const dataFim = new Date(this.form.get('dataFim').value);
      if (dataFim < dataInicio)
        this.form.get('dataFim').setValue(null);
    }
  }

  formatarData(data: Date) {
    return {
      'year': data.getFullYear(),
      'month': data.getMonth() + 1,
      'day': data.getDate()
    };
  }

  configurarCampo(campo: string, habilitado: boolean) {
    if (habilitado) {
      this.form.get(campo).setValidators(Validators.required);
    }
    else {
      this.form.get(campo).setValue(null);
      this.form.get(campo).clearValidators();
    }
    this.form.get(campo).updateValueAndValidity();
  }

  cadastrarAtividade() {
    if (this.form.valid) {
      const dados: IPactoTrabalhoAtividade = this.form.value;
      dados.pactoTrabalhoId = this.dadosPacto.value.pactoTrabalhoId;
      const itemCatalogo = this.itensCatalogo.filter(s => s.itemCatalogoId === dados.itemCatalogoId)[0];
      dados.itemCatalogo = itemCatalogo.titulo;
      if (itemCatalogo.complexidade)
        dados.itemCatalogo += ' - ' + itemCatalogo.complexidade;
      if (itemCatalogo.formaCalculoTempoItemCatalogoId !== 203) {
        dados.tempoPrevistoPorItem = this.dadosPacto.value.formaExecucaoId === 101 ? itemCatalogo.tempoExecucaoPresencial : itemCatalogo.tempoExecucaoRemoto;
      }

      if (this.tipoItemSelecionado === 201) {
        dados.dataInicio = this.dadosPacto.value.dataInicio;
        dados.situacaoId = 502;
      }
      
      dados.situacao = this.situacoes.filter(s => s.id === +dados.situacaoId)[0].descricao;


      this.pactoTrabalhoDataService.ProporAtividade(dados).subscribe(
        r => {
          this.dadosPacto.next(this.dadosPacto.value);
        });
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

  fecharModal() {
    this.modalService.dismissAll();
  }
}
