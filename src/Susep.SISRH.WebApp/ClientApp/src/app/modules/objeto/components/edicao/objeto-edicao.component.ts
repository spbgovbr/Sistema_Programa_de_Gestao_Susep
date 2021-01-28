import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ObjetoDataService } from '../../services/objeto.service';
import { PerfilEnum } from 'src/app/modules/programa-gestao/enums/perfil.enum';
import { IDatasourceAutocompleteAsync } from 'src/app/shared/components/input-autocomplete-async/input-autocomplete-async.component';
import { IObjeto } from '../../models/objeto.model';
import { EnumHelper } from 'src/app/shared/helpers/enum.helper';
import { TipoObjetoEnum } from '../../enums/tipo-objeto.enum';
import { validacaoCondicional } from 'src/app/shared/functions/validations.function';
import { ValidateBrService } from 'angular-validate-br';

@Component({
  selector: 'objeto-edicao',
  templateUrl: './objeto-edicao.component.html',
})
export class ObjetoEdicaoComponent implements OnInit {

  PerfilEnum = PerfilEnum;
  TipoObjetoEnum = TipoObjetoEnum;

  form: FormGroup;

  datasource: IDatasourceAutocompleteAsync;

  entidadeEmEdicao: IObjeto;

  tipos = EnumHelper.toList(TipoObjetoEnum);

  private _tipoSelecionado;

  get tipoSelecionado(): number {
    if (this._tipoSelecionado && typeof(this._tipoSelecionado) === 'string') {
      const result = Number.parseInt(this._tipoSelecionado);
      return result || null;
    }
    return this._tipoSelecionado;
  }

  set tipoSelecionado(tipo) {
    this._tipoSelecionado = tipo;
  }

  constructor(
    public router: Router,    
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private objetoDataService: ObjetoDataService,
    private validateBrService: ValidateBrService
  ) {}
  
  ngOnInit() {
    this.criarForm();
    this.carregarDados();
  }

  private criarForm(): void {

    this.form = this.formBuilder.group({
      cnpj: ['', [validacaoCondicional(() => this.ehEmpresa, Validators.compose([Validators.required, this.validateBrService.cnpj]))]],
      chave: ['', [validacaoCondicional(() => !this.ehEmpresa, Validators.required)]],
      descricao: ['', [Validators.required]],
      tipo: [null, [Validators.required]],
      ativo: [null, []]
    });
    
  }

  private carregarDados() {
    const id = this.route.snapshot.params.id;
    if (id) {
      
      this.objetoDataService.ObterPorId(id).subscribe(result => {
        this.entidadeEmEdicao = result.retorno;
        this.tipoSelecionado = this.entidadeEmEdicao.tipo;
        this.fillForm();
      });
      
    } 
  }

  private fillForm(): void {
    this.form.patchValue({
      chave: !this.ehEmpresa ? this.entidadeEmEdicao.chave : '',
      cnpj: this.ehEmpresa ? this.entidadeEmEdicao.chave : '',
      descricao: this.entidadeEmEdicao.descricao,
      tipo: this.entidadeEmEdicao.tipo,
      ativo: this.entidadeEmEdicao.ativo
    });
  }

  salvar() {
    if (this.form.valid) {
      const formValue = this.form.value;
      const dados: IObjeto = {
        chave: this.ehEmpresa ? formValue.cnpj : formValue.chave,
        descricao: formValue.descricao,
        tipo: formValue.tipo,
        ativo: true
      };
      
      this.objetoDataService.CadastrarObjeto(dados).subscribe(result => {
        if (result.retorno) {
          this.router.navigateByUrl('/objeto');
        }
      });
      
    }
  }

  atualizar() {
    if (this.form.valid) {
      const formValue = this.form.value;

//      const dados = Object.assign({...this.entidadeEmEdicao}, formValue);
      const dados: IObjeto = {
        objetoId: this.entidadeEmEdicao.objetoId,
        chave: this.ehEmpresa ? formValue.cnpj : formValue.chave,
        descricao: formValue.descricao,
        tipo: formValue.tipo,
        ativo: formValue.ativo
      };


      this.objetoDataService.AtualizarObjeto(dados).subscribe(result => {
        if (result.retorno) {
          this.router.navigateByUrl('/objeto');
        }
      });
    }
  }

  alterouTipo(tipoId) {
    this.form.patchValue({ chave : '', cnpj: '' });
    this.tipoSelecionado = tipoId;
    if (this.ehEmpresa) {
      this.form.controls['cnpj'].enable();
      this.form.controls['chave'].disable();
    } else {
      this.form.controls['cnpj'].disable();
      this.form.controls['chave'].enable();
    }
  }

  get ehEmpresa() {
    return this.tipoSelecionado && this.tipoSelecionado === 1;
  }
}

