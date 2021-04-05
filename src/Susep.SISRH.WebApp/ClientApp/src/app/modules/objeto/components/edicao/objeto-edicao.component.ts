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

  objeto: IObjeto;

  tipos = EnumHelper.toList(TipoObjetoEnum);

  constructor(
    public router: Router,    
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private objetoDataService: ObjetoDataService
  ) {}
  
  ngOnInit() {
    this.criarForm();
    this.carregarDados();
  }

  private criarForm(): void {

    this.form = this.formBuilder.group({
      chave: ['', [Validators.required]],
      descricao: ['', [Validators.required]],
      tipo: [null, [Validators.required]],
      ativo: [null, []]
    });
    
  }

  private carregarDados() {
    const id = this.route.snapshot.params.id;
    if (id) {
      
      this.objetoDataService.ObterPorId(id).subscribe(result => {
        this.objeto = result.retorno;
        this.fillForm();
      });
      
    } 
  }

  private fillForm(): void {
    this.form.patchValue({
      chave: this.objeto.chave,
      descricao: this.objeto.descricao,
      tipo: this.objeto.tipo,
      ativo: this.objeto.ativo
    });
  }

  salvar() {
    if (this.form.valid) {
      const dados: IObjeto = this.form.value;
      dados.chave = '###' + dados.chave;
      if (this.objeto) {
        dados.objetoId = this.objeto.objetoId;
        this.objetoDataService.AtualizarObjeto(dados).subscribe(result => {
          if (result.retorno) {
            this.router.navigateByUrl('/objeto');
          }
        });
      }
      else {
        dados.ativo = true;
        this.objetoDataService.CadastrarObjeto(dados).subscribe(result => {
          if (result.retorno) {
            this.router.navigateByUrl('/objeto');
          }
        });
      }
      
    }
  }

}

