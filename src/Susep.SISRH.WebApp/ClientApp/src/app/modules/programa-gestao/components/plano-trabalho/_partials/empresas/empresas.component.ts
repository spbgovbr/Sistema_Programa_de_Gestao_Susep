import { Component, OnInit, Input } from '@angular/core';
import { IPlanoTrabalho, IPlanoTrabalhoEmpresa } from '../../../../models/plano-trabalho.model';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PlanoTrabalhoDataService } from '../../../../services/plano-trabalho.service';
import { PlanoTrabalhoSituacaoEnum } from '../../../../enums/plano-trabalho-situacao.enum';
import { PerfilEnum } from '../../../../enums/perfil.enum';
import { Guid } from 'src/app/shared/helpers/guid.helper';
import { IDatasourceAutocompleteAsync } from 'src/app/shared/components/input-autocomplete-async/input-autocomplete-async.component';
import { map } from 'rxjs/operators';

@Component({
  selector: 'plano-empresas',
  templateUrl: './empresas.component.html',
})
export class PlanoEmpresasComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  PlanoTrabalhoSituacaoEnum = PlanoTrabalhoSituacaoEnum;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;
  unidade = new BehaviorSubject<number>(null);

  form: FormGroup;

//  minDate = new Date();

  private empresasCarregadas: Map<Guid, IPlanoTrabalhoEmpresa> = new Map();

  datasource: IDatasourceAutocompleteAsync;

  isReadOnly: boolean;

  get chavesJaEscolhidas(): Guid[] {
    if (!this.dadosPlano || !this.dadosPlano.value || !this.dadosPlano.value.empresas) 
      return [];
    return this.dadosPlano.value.empresas.map(a => a.planoTrabalhoEmpresaId);
  }


  constructor(
    private formBuilder: FormBuilder,
    private planoTrabalhoDataService: PlanoTrabalhoDataService, ) {
  }

  ngOnInit() {

    this.dadosPlano.subscribe(val => this.carregarEmpresas());

    this.form = this.formBuilder.group({
      empresaId: ['', []],
    });
    
    this.datasource = this.criarDatasourceAutocomplete();
  }

  private criarDatasourceAutocomplete(): IDatasourceAutocompleteAsync {
    return {
      buscarPorChave: this.buscarEmpresaPorId,
      buscarPorValor: (valor) => this.planoTrabalhoDataService.ObterEmpresassPorTexto(valor).pipe(
        map(result => {
          if (!result || !result.retorno) return [];
          const empresas = result.retorno.filter(a => !this.dadosPlano.value.empresas.find(i => i.planoTrabalhoEmpresaId === a.planoTrabalhoEmpresaId));
          empresas.forEach(a => this.empresasCarregadas.set(a.planoTrabalhoEmpresaId, a));
          return empresas;
        })),
      modelToValue: (model: IPlanoTrabalhoEmpresa) => {  return { chave: model.planoTrabalhoEmpresaId, descricao: model.nome } }
    }
  }

  private buscarEmpresaPorId(id: Guid): Observable<IPlanoTrabalhoEmpresa> {
    if (this.empresasCarregadas && this.empresasCarregadas.has(id))
      return of(this.empresasCarregadas.get(id));

   if (!this.dadosPlano || !this.dadosPlano.value || !this.dadosPlano.value.empresas)
      return of(null);

    return this.planoTrabalhoDataService.ObterEmpresaPorId(id).pipe(
      map(result => {
        const empresa = result.retorno;
        if (!empresa) return null;
        this.empresasCarregadas.set(empresa.planoTrabalhoEmpresaId, empresa);
        return empresa;
      })
    )
  }

  carregarEmpresas() {
    this.unidade.next(this.dadosPlano.value.unidadeId);
    this.planoTrabalhoDataService.ObterEmpresas(this.dadosPlano.value.planoTrabalhoId).subscribe(
      resultado => {
        this.dadosPlano.value.empresas = resultado.retorno;
      }
    );
    this.isReadOnly = this.dadosPlano.value.situacaoId !== PlanoTrabalhoSituacaoEnum.Rascunho;
  }

  registrarEmpresa() {
/*
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
    */
  }

  getFormValidationErrors(form) {
    Object.keys(form.controls).forEach(field => {
      const control = form.get(field);
      control.markAsDirty({ onlySelf: true });
    });
  }

  excluir(planoTrabalhoEmpresaId: Guid) {
    const idx = this.dadosPlano.value.empresas.findIndex(a => a.planoTrabalhoEmpresaId === planoTrabalhoEmpresaId);
    if (idx >= 0)
      this.dadosPlano.value.empresas.splice(idx, 1);

    /*
    this.planoTrabalhoDataService.ExcluirEmpresa(this.dadosPlano.value.planoTrabalhoId, planoTrabalhoCustoId).subscribe(
      appResult => {
        this.carregarCustos();
      }
    );
    */
  }

  associar() {
    if (this.form.valid) {
      const formValue = this.form.value;
      if (formValue.empresaId) {
        this.buscarEmpresaPorId(formValue.empresaId).subscribe(empresa => {
          if (empresa) {
            this.dadosPlano.value.empresas.push(empresa);
          }
        });
      }
    }
  }

}
