import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { IDadosCombo } from '../../../../../../../shared/models/dados-combo.model';
import { IItemCatalogo, IItemCatalogoAssunto } from '../../../../../models/item-catalogo.model';
import { IPlanoTrabalho, IPlanoTrabalhoAtividade, IPlanoTrabalhoAtividadeItem, IPlanoTrabalhoAtividadeCriterio } from '../../../../../models/plano-trabalho.model';
import { PlanoTrabalhoDataService } from '../../../../../services/plano-trabalho.service';
import { IDominio } from '../../../../../../../shared/models/dominio.model';
import { PerfilEnum } from '../../../../../enums/perfil.enum';
import { DecimalValuesHelper } from '../../../../../../../shared/helpers/decimal-valuesr.helper';
import { ConfigurationService } from 'src/app/shared/services/configuration.service';
import { TipoModo } from 'src/app/shared/models/configuration.model';
import { isNumber } from 'util';

@Component({
  selector: 'plano-atividade-cadastro',
  templateUrl: './atividade-cadastro.component.html',  
})
export class PlanoAtividadeCadastroComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;
  @Input() modalidades: BehaviorSubject<IDadosCombo[]>;
  @Input() itensUnidade: BehaviorSubject<IItemCatalogo[]>;
  @Input() criterios: BehaviorSubject<IDominio[]>;
  @Input() atividadeEdicao: BehaviorSubject<IPlanoTrabalhoAtividade>;

  @Input() totalDisponivelColaboradores: number;

  form: FormGroup;

  itensCatalogo: IItemCatalogo[] = [];
  criteriosPerfil: IDominio[] = [];

  assuntos: IItemCatalogoAssunto[] = [];
  private todosOsAssuntos: IItemCatalogoAssunto[] = [];

  vagasMaiorQueZero: boolean;
  itemSelecionado: boolean;
  criterioSelecionado: boolean;

  public tempoMask: any;

  constructor(
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private decimalValuesHelper: DecimalValuesHelper,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,
    private configurationService: ConfigurationService) { }

  ngOnInit() {

    this.tempoMask = this.decimalValuesHelper.numberMask(2, 0);

    this.form = this.formBuilder.group({
      modalidadeExecucaoId: ['', [Validators.required]],
      quantidadeColaboradores: [null, [Validators.required]],
      descricao: ['', []],
    });

    this.itensUnidade.subscribe(val => this.carregarDados());
    this.criterios.subscribe(val => this.carregarDados());
    this.atividadeEdicao.subscribe(val => this.carregarDados());
  }

  fillForm() {    
    if (this.atividadeEdicao.value) {
      this.form.patchValue({
        modalidadeExecucaoId: this.atividadeEdicao.value.modalidadeExecucaoId,
        quantidadeColaboradores: this.atividadeEdicao.value.quantidadeColaboradores,
        descricao: this.atividadeEdicao.value.descricao,
      });      
      this.vagasMaiorQueZero = (this.atividadeEdicao.value.quantidadeColaboradores > 0);      
    }
  }

  carregarDados() {    
    this.verificarItensMarcados();
    this.verificarCriteriosMarcados();
    this.fillForm();
    this.verificarPossibilidadeSalvar();
  }

  private carregarTodosOsAssuntos() {
    this.todosOsAssuntos = this.itensCatalogo ? this.itensCatalogo
      .map(i => i.assuntos) // transforma em lista de lista de assuntos
      .reduce((p, c) => p = p.concat(c), []) // transforma em lista de assuntos
      .filter((value, index, array) => array.findIndex(v => v.assuntoId === value.assuntoId) === index) // remove duplicações
      : [];
  }

  verificarItensMarcados() {
    if (this.itensUnidade.value) {
      this.itensUnidade.value.forEach(i =>
        i.tempoExecucaoPreviamenteDefinido = this.atividadeEdicao.value &&
        this.atividadeEdicao.value.itensCatalogo &&
        this.atividadeEdicao.value.itensCatalogo.filter(ai => ai.itemCatalogoId === i.itemCatalogoId).length > 0);
      this.itensCatalogo = this.itensUnidade.value;
      this.carregarTodosOsAssuntos();
      this.atualizarAssuntosSelecionados();
      this.assuntos
        .filter(a => this.atividadeEdicao.value.assuntos.map(i => i.assuntoId).includes(a.assuntoId))
        .forEach(a => a['ativo'] = true);
    }
  }

  verificarCriteriosMarcados() {
    if (this.criterios.value) {
      this.criterios.value.forEach(i =>
        i.ativo = this.atividadeEdicao.value &&
                  this.atividadeEdicao.value.criterios &&
                  this.atividadeEdicao.value.criterios.filter(ai => ai.criterioId === i.id).length > 0);
      this.criteriosPerfil = this.criterios.value;
    }
  }

  cadastrarAtividade() {
    if (this.form.valid) {
      const dados: IPlanoTrabalhoAtividade = this.form.value;
      dados.planoTrabalhoId = this.dadosPlano.value.planoTrabalhoId;
      dados.itensCatalogo = this.itensCatalogo.filter(i => i.tempoExecucaoPreviamenteDefinido).map(function (i) { const model: IPlanoTrabalhoAtividadeItem = { 'itemCatalogoId': i.itemCatalogoId }; return model; });
      //dados.criterios = this.criteriosPerfil.filter(i => i.ativo).map(function (i) { const model: IPlanoTrabalhoAtividadeCriterio = { 'criterioId': i.id }; return model; });
      dados.idsAssuntos = this.getIdsAssuntosParaSalvar();
      if (this.atividadeEdicao.value && this.atividadeEdicao.value.planoTrabalhoAtividadeId) {
        dados.planoTrabalhoAtividadeId = this.atividadeEdicao.value.planoTrabalhoAtividadeId;
        this.planoTrabalhoDataService.AlterarAtividade(dados).subscribe(
          r => {
            this.dadosPlano.next(this.dadosPlano.value);
          });
      }
      else {
        this.planoTrabalhoDataService.CadastrarAtividade(dados).subscribe(
          r => {
            this.dadosPlano.next(this.dadosPlano.value);
          });
      }
    }
    else {
      this.getFormValidationErrors(this.form)
    }
  }

  private getIdsAssuntosParaSalvar() {
    const idsAssuntosSelecionados = this.todosOsAssuntos.filter(a => a['ativo'] === true).map(a => a.assuntoId);
    if (this.modo === 'avancado') 
      return idsAssuntosSelecionados.length ? idsAssuntosSelecionados : null;

    const ids = idsAssuntosSelecionados.filter(id => this.idsAssuntosDeItensSelecionados.includes(id));
    return ids.length ? ids : null;
  }

  getFormValidationErrors(form) {
    Object.keys(form.controls).forEach(field => {
      const control = form.get(field);
      control.markAsDirty({ onlySelf: true });
    });
  }

  mudarFormaExecucao(newValue) {
    this.itensCatalogo = this.itensUnidade.value;
    if (newValue !== '101') {    
      this.itensCatalogo = this.itensUnidade.value.filter(i => i.permiteTrabalhoRemoto)
    }
    this.verificarPossibilidadeSalvar();
  }

  mudarQtdeVagas(newValue: number) {
    this.vagasMaiorQueZero = (newValue > 0);    
  }

  toggleSituacao() {    
    this.verificarPossibilidadeSalvar();
    this.atualizarAssuntosSelecionados();
  }

  private atualizarAssuntosSelecionados() {
    this.assuntos = this.todosOsAssuntos
      .filter(a => this.idsAssuntosDeItensSelecionados.includes(a.assuntoId))
      .sort((a,b) => a.valor > b.valor ? 1 : -1);
    this.todosOsAssuntos
      .filter(a => !this.idsAssuntosDeItensSelecionados.includes(a.assuntoId))
      .forEach(a => a['ativo'] = false);
  }

  get modo(): TipoModo {
    return this.configurationService.getModo();
  }

  private get idsAssuntosDeItensSelecionados() {
    return this.itensCatalogo ? this.itensCatalogo
      .filter(i => i.tempoExecucaoPreviamenteDefinido) // somente itens catalogo selecionados
      .map(i => i.assuntos) // transforma em lista de lista de assuntos
      .reduce((p, c) => p = p.concat(c), []) // transforma em lista de assuntos
      .filter((value, index, array) => array.findIndex(v => v.assuntoId === value.assuntoId) === index) // remove duplicações
      .map(a => a.assuntoId)
      : [];
  }

  verificarPossibilidadeSalvar() {
    this.itemSelecionado = this.itensCatalogo.filter(i => i.tempoExecucaoPreviamenteDefinido).length > 0;
    this.criterioSelecionado = true;//this.criteriosPerfil.filter(i => i.ativo).length > 0;
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

}
