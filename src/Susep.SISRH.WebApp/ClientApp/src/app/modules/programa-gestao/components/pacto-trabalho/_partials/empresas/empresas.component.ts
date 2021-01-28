import { Component, OnInit, Input } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PerfilEnum } from '../../../../enums/perfil.enum';
import { Guid } from 'src/app/shared/helpers/guid.helper';
import { IDatasourceAutocompleteAsync } from 'src/app/shared/components/input-autocomplete-async/input-autocomplete-async.component';
import { map } from 'rxjs/operators';
import { IPactoTrabalho, IPactoTrabalhoEmpresa } from 'src/app/modules/programa-gestao/models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from 'src/app/modules/programa-gestao/services/pacto-trabalho.service';

@Component({
  selector: 'pacto-empresas',
  templateUrl: './empresas.component.html',
  styleUrls: ['./empresas.component.css']
})
export class PactoEmpresasComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;
  unidade = new BehaviorSubject<number>(null);

  form: FormGroup;

//  minDate = new Date();

  private empresasCarregadas: Map<Guid, IPactoTrabalhoEmpresa> = new Map();

  datasource: IDatasourceAutocompleteAsync;

  isReadOnly: boolean;

  get chavesJaEscolhidas(): Guid[] {
    if (!this.dadosPacto || !this.dadosPacto.value || !this.dadosPacto.value.empresas) 
      return [];
    return this.dadosPacto.value.empresas.map(a => a.pactoTrabalhoEmpresaId);
  }


  constructor(
    private formBuilder: FormBuilder,
    private pactoTrabalhoDataService: PactoTrabalhoDataService, ) {
  }

  ngOnInit() {

    this.dadosPacto.subscribe(val => this.carregarEmpresas());

    this.form = this.formBuilder.group({
      empresaId: ['', []],
    });
    
    this.datasource = this.criarDatasourceAutocomplete();
  }

  private criarDatasourceAutocomplete(): IDatasourceAutocompleteAsync {
    return {
      buscarPorChave: this.buscarEmpresaPorId,
      buscarPorValor: (valor) => this.pactoTrabalhoDataService.ObterEmpresassPorTexto(valor).pipe(
        map(result => {
          if (!result || !result.retorno) return [];
          const empresas = result.retorno.filter(a => !this.dadosPacto.value.empresas.find(i => i.pactoTrabalhoEmpresaId === a.pactoTrabalhoEmpresaId));
          empresas.forEach(a => this.empresasCarregadas.set(a.pactoTrabalhoEmpresaId, a));
          return empresas;
        })),
      modelToValue: (model: IPactoTrabalhoEmpresa) => {  return { chave: model.pactoTrabalhoEmpresaId, descricao: model.nome } }
    }
  }

  private buscarEmpresaPorId(id: Guid): Observable<IPactoTrabalhoEmpresa> {
    if (this.empresasCarregadas && this.empresasCarregadas.has(id))
      return of(this.empresasCarregadas.get(id));

   if (!this.dadosPacto || !this.dadosPacto.value || !this.dadosPacto.value.empresas)
      return of(null);

    return this.pactoTrabalhoDataService.ObterEmpresaPorId(id).pipe(
      map(result => {
        const empresa = result.retorno;
        if (!empresa) return null;
        this.empresasCarregadas.set(empresa.pactoTrabalhoEmpresaId, empresa);
        return empresa;
      })
    )
  }

  carregarEmpresas() {
    this.unidade.next(this.dadosPacto.value.unidadeId);
    this.pactoTrabalhoDataService.ObterEmpresas(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.dadosPacto.value.empresas = resultado.retorno;
      }
    );
//TODO - verificar o que fazer    
//this.isReadOnly = this.dadosPacto.value.situacaoId !== PlanoTrabalhoSituacaoEnum.Rascunho;
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

  excluir(pactoTrabalhoEmpresaId: Guid) {
    const idx = this.dadosPacto.value.empresas.findIndex(a => a.pactoTrabalhoEmpresaId === pactoTrabalhoEmpresaId);
    if (idx >= 0)
      this.dadosPacto.value.empresas.splice(idx, 1);

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
            this.dadosPacto.value.empresas.push(empresa);
          }
        });
      }
    }
  }

}
