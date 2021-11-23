import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ItemCatalogoDataService } from '../../../services/item-catalogo.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemCatalogo, IItemCatalogoAssunto } from '../../../models/item-catalogo.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DominioDataService } from '../../../../../shared/services/dominio.service';
import { IDominio } from '../../../../../shared/models/dominio.model';
import { PerfilEnum } from '../../../enums/perfil.enum';
import { DecimalValuesHelper } from '../../../../../shared/helpers/decimal-valuesr.helper';
import { ConfigurationService } from 'src/app/shared/services/configuration.service';
import { TipoModo } from 'src/app/shared/models/configuration.model';

@Component({
  selector: 'item-catalogo-cadastro',
  templateUrl: './item-catalogo-cadastro.component.html',
})
export class ItemCatalogoCadastroComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  modoExibicao: TipoModo;

  assuntos: IItemCatalogoAssunto[] = [];

  form: FormGroup;
  formaCalculoTempo: IDominio[];
  dadosItemCatalogo: IItemCatalogo;

  tempoCalculadoPreviamente?: boolean;
  exibeDescricaoComplexidade?: boolean;
  permiteTrabalhoRemoto?: boolean = true;

  ganhoProdutividade: number;

  public tempoMask: any;

  @ViewChild('entregasEsperadas', { static: true }) entregasEsperadas: ElementRef;

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private decimalValuesHelper: DecimalValuesHelper,
    private itemCatalogoDataService: ItemCatalogoDataService,
    private dominioDataService: DominioDataService,
    private configuracaoService: ConfigurationService,
  ) { }

  ngOnInit() {

    this.modoExibicao = this.configuracaoService.getModo();

    this.tempoMask = this.decimalValuesHelper.numberMask(3, 1);

    this.dominioDataService.ObterFormaCalculoTempoItemCatalogo().subscribe(
      appResult => {
        this.formaCalculoTempo = appResult.retorno;
      }
    );

    this.carregarDados();

    this.form = this.formBuilder.group({
      titulo: ['', [Validators.required]],
      complexidade: ['', []],
      definicaoComplexidade: ['', []],
      permiteTrabalhoRemoto: [null, [Validators.required]],
      descricao: [null, []],
      formaCalculoTempoItemCatalogoId: [null, [Validators.required]],
      tempoExecucaoPresencial: [null, []],
      tempoExecucaoRemoto: [null, []],      
      entregasEsperadas: ['', [Validators.required]],
    }, { updateOn: 'blur' });
  }

  carregarDados() {
    const itemCatalogoId = this.activatedRoute.snapshot.paramMap.get('id');
    if (itemCatalogoId) {      
      this.itemCatalogoDataService.ObterItem(itemCatalogoId).subscribe(
        resultado => {
          this.dadosItemCatalogo = resultado.retorno;

          if (this.router.url.indexOf('/copiar/') > -1) {
            this.dadosItemCatalogo.temPactoCadastrado = false;
          }

          this.assuntos = this.dadosItemCatalogo.assuntos;
          this.fillForm();
        }
      );
    }
  }

  fillForm() {
    this.form.patchValue({
      titulo: this.dadosItemCatalogo.titulo,
      complexidade: this.dadosItemCatalogo.complexidade,
      definicaoComplexidade: this.dadosItemCatalogo.definicaoComplexidade,
      permiteTrabalhoRemoto: this.dadosItemCatalogo.permiteTrabalhoRemoto,      
      descricao: this.dadosItemCatalogo.descricao,
      formaCalculoTempoItemCatalogoId: this.dadosItemCatalogo.formaCalculoTempoItemCatalogoId,
      tempoExecucaoPresencial: this.decimalValuesHelper.toPtBr(this.dadosItemCatalogo.tempoExecucaoPresencial),
      tempoExecucaoRemoto: this.decimalValuesHelper.toPtBr(this.dadosItemCatalogo.tempoExecucaoRemoto),
      entregasEsperadas: this.dadosItemCatalogo.entregasEsperadas,
    });
    this.mudarFormaCalculoTempo(this.dadosItemCatalogo.formaCalculoTempoItemCatalogoId);
    this.mudarPermiteTrabalhoRemoto(this.dadosItemCatalogo.permiteTrabalhoRemoto);
    this.mudarComplexidade(this.dadosItemCatalogo.complexidade);
    this.mudarTempoExecucao(null);
  }

  mudarFormaCalculoTempo(newValue) {
    if (newValue.toString().includes('203')) {
      this.tempoCalculadoPreviamente = false;
      this.form.get('tempoExecucaoPresencial').setValue(null);
      this.form.get('tempoExecucaoRemoto').setValue(null);
      this.form.get('tempoExecucaoPresencial').clearValidators();
      this.form.get('tempoExecucaoRemoto').clearValidators();      
    }
    else {
      this.tempoCalculadoPreviamente = true;
      this.form.get('tempoExecucaoPresencial').setValidators(Validators.required);
      this.mudarPermiteTrabalhoRemoto(this.permiteTrabalhoRemoto);
    }
    this.form.get('tempoExecucaoRemoto').updateValueAndValidity();
    this.form.get('tempoExecucaoPresencial').updateValueAndValidity();

    const permiteTrabalhoRemoto = this.form.get('permiteTrabalhoRemoto').value;
    this.mudarPermiteTrabalhoRemoto(permiteTrabalhoRemoto)
  }

  mudarComplexidade(newValue) {    
    if (newValue && newValue !== '') {
      this.exibeDescricaoComplexidade = true;
      this.form.get('definicaoComplexidade').setValidators(Validators.required);
    }
    else {
      this.exibeDescricaoComplexidade = false;
      this.form.get('definicaoComplexidade').setValue(null);
      this.form.get('definicaoComplexidade').clearValidators();      
    }
    this.form.get('definicaoComplexidade').updateValueAndValidity();
  }

  mudarTempoExecucao(event) {
    let tempoPresencial = this.form.get('tempoExecucaoPresencial').value;
    let tempoRemoto = this.form.get('tempoExecucaoRemoto').value;

    if (event) {
      if (event.target.id === 'tempoExecucaoPresencial')
        tempoPresencial = event.target.value;
      else
        tempoRemoto = event.target.value;
    }

    const ganhoProdutividade = Number((this.decimalValuesHelper.fromPtBr(tempoPresencial) / this.decimalValuesHelper.fromPtBr(tempoRemoto)) - 1);
    this.ganhoProdutividade = ganhoProdutividade;    
  }

  mudarPermiteTrabalhoRemoto(newValue) {
    if (!this.tempoCalculadoPreviamente || newValue.toString().includes('false')) {
      this.permiteTrabalhoRemoto = false;
      this.form.get('tempoExecucaoRemoto').setValue(null);
      this.form.get('tempoExecucaoRemoto').clearValidators();
    }
    else {
      this.permiteTrabalhoRemoto = true;
      this.form.get('tempoExecucaoRemoto').setValidators(Validators.required);
    }
    this.form.get('tempoExecucaoRemoto').updateValueAndValidity();    
  }

  salvarItem() {
    if (this.form.valid) {
      const dados = this.form.value;
      dados.assuntos = this.assuntos && this.assuntos.length ? this.assuntos : null;
      const itemCatalogoId = this.activatedRoute.snapshot.paramMap.get('id');
      const edicao = itemCatalogoId && this.router.url.indexOf('/copiar/') === -1;
      if (edicao) {
        dados.itemCatalogoId = itemCatalogoId;
        this.itemCatalogoDataService.Alterar(dados).subscribe(
          r => {
            this.carregarDados();
            this.router.navigateByUrl(`/programagestao/catalogo/item`);
          });        
      }
      else {
        this.itemCatalogoDataService.Cadastrar(dados).subscribe(
          r => {
            this.carregarDados();
            this.router.navigateByUrl(`/programagestao/catalogo/item`);
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

}
