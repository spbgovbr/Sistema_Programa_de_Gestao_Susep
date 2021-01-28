import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AssuntoDataService } from '../../services/assunto.service';
import { map } from 'rxjs/operators';
import { PerfilEnum } from 'src/app/modules/programa-gestao/enums/perfil.enum';
import { IAssuntoCadastro, IAssuntoEdicao, IAssuntoHierarquia } from '../../models/assunto.model';
import { IDatasourceAutocompleteAsync, IChaveDescricao } from 'src/app/shared/components/input-autocomplete-async/input-autocomplete-async.component';
import { of } from 'rxjs';

@Component({
  selector: 'assunto-edicao',
  templateUrl: './assunto-edicao.component.html',
})
export class AssuntoEdicaoComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  form: FormGroup;

  datasource: IDatasourceAutocompleteAsync;

  entidadeEmEdicao: IAssuntoEdicao;

  constructor(
    public router: Router,    
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private assuntoDataService: AssuntoDataService
  ) {}

  ngOnInit() {
    this.criarForm();
    this.carregarDados();
    this.datasource = this.criarDatasource();
  }

  private criarDatasource(): IDatasourceAutocompleteAsync {
    return {
      buscarPorChave: (chave: any) => of(this.entidadeEmEdicao ? this.entidadeEmEdicao.pai : null),
      buscarPorValor: (valor: string) => this.assuntoDataService.ObterAssuntosPorTexto(valor).pipe(
        map(result => result.retorno
          .filter(a => this.deveExibirAssunto(a))
          .sort((a, b) => ('' + a.hierarquia).localeCompare(b.hierarquia)))
        ),
      modelToValue: (model: any) => this.toIChaveDescricao(model)
    };
  }

  private deveExibirAssunto(assunto: IAssuntoHierarquia): boolean {
    if (!this.entidadeEmEdicao) return true;
    if (!assunto.hierarquia || !assunto.hierarquia.trim().length) return true;
    // Não pode exibir o próprio assunto em edição ou os assuntos que forem ancestrais dele.
    return !assunto.hierarquia.includes(this.entidadeEmEdicao.valor);
  }

  private toIChaveDescricao(value: IAssuntoHierarquia): IChaveDescricao {
    return {
      chave: value.assuntoId,
      descricao: value.hierarquia
    };
  }

  private criarForm(): void {

    this.form = this.formBuilder.group({
      valor: ['', [Validators.required, Validators.minLength(5)]],
      paiId: [null, []],
      ativo: [null, []]
    });
    
  }

  private carregarDados() {
    const id = this.route.snapshot.params.id;
    if (id) {
      this.assuntoDataService.ObterPorId(id).subscribe(result => {
        this.entidadeEmEdicao = result.retorno;
        this.fillForm();
      });
    } 
  }

  private fillForm(): void {
    this.form.patchValue({
      valor: this.entidadeEmEdicao.valor,
      paiId: this.entidadeEmEdicao.pai ? this.entidadeEmEdicao.pai.assuntoId : null,
      ativo: this.entidadeEmEdicao.ativo
    });
  }

  salvar() {
    if (this.form.valid) {
      const formValue = this.form.value;
      const dados: IAssuntoCadastro = {
        valor: formValue.valor,
        assuntoPaiId: formValue.paiId
      };
      this.assuntoDataService.CadastrarAssunto(dados).subscribe(result => {
        if (result.retorno) {
          this.router.navigateByUrl('/assunto');
        }
      });
    }
  }

  atualizar() {
    if (this.form.valid) {
      const formValue = this.form.value;

      const dados = Object.assign({...this.entidadeEmEdicao}, formValue);

      this.assuntoDataService.AtualizarAssunto(dados).subscribe(result => {
        if (result.retorno) {
          this.router.navigateByUrl('/assunto');
        }
      });
      
    }
  }

}

