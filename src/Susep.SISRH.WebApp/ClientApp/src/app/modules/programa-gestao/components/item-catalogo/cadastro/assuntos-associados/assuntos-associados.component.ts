import { Component, Input, OnInit } from '@angular/core';
import { IItemCatalogoAssunto } from 'src/app/modules/programa-gestao/models/item-catalogo.model';
import { IDatasourceAutocompleteAsync, IConfiguracoesAutocompleteAsync } from 'src/app/shared/components/input-autocomplete-async/input-autocomplete-async.component';
import { of, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Guid } from 'src/app/shared/helpers/guid.helper';
import { FormGroup, FormBuilder } from '@angular/forms';
import { AssuntoDataService } from 'src/app/modules/assunto/services/assunto.service';

@Component({
  selector: 'assuntos-associados',
  templateUrl: './assuntos-associados.component.html',  
})
export class AssuntosAssociadosComponent implements OnInit {

  @Input() assuntos: IItemCatalogoAssunto[];

  form: FormGroup;

  private assuntosCarregados: Map<Guid, IItemCatalogoAssunto> = new Map();

  datasource: IDatasourceAutocompleteAsync;

  get chavesJaEscolhidas(): Guid[] {
    return this.assuntos.map(a => a.assuntoId);
  }

  constructor(private formBuilder: FormBuilder,
    private assuntoService: AssuntoDataService) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      assuntoId: [null, []],
    });
    this.datasource = this.criarDatasourceAutocomplete();
  }

  private criarDatasourceAutocomplete(): IDatasourceAutocompleteAsync {
    return {
      buscarPorChave: this.buscarAssuntoPorId,
      buscarPorValor: (valor) => this.assuntoService.ObterAssuntosPorTexto(valor).pipe(
        map(result => {
          if (!result || !result.retorno) return [];
          const assuntos = result.retorno.filter(a => !this.assuntos.find(i => i.assuntoId === a.assuntoId));
          assuntos.forEach(a => this.assuntosCarregados.set(a.assuntoId, a));
          return assuntos;
        })),
      modelToValue: (model) => { return { chave: model.assuntoId, descricao: model.hierarquia } }
    }
  }

  private buscarAssuntoPorId(id: Guid): Observable<IItemCatalogoAssunto> {
    if (this.assuntosCarregados && this.assuntosCarregados.has(id))
      return of(this.assuntosCarregados.get(id));

    if (!this.assuntos)
      return of(null);

    return this.assuntoService.ObterPorId(id).pipe(
      map(result => {
        const dado = result.retorno;
        if (!dado) return null;
        const assunto = {
          assuntoId: dado.assuntoId,
//          chave: dado.chave,
          valor: dado.valor,
          hierarquia: ''
        };
        this.assuntosCarregados.set(assunto.assuntoId, assunto);
        return assunto;
      })
    )
  }

  excluir(assunto: IItemCatalogoAssunto) {
    const idx = this.assuntos.findIndex(a => a.assuntoId === assunto.assuntoId);
    if (idx >= 0)
      this.assuntos.splice(idx, 1);
  }

  associar() {
    if (this.form.valid) {
      const formValue = this.form.value;
      if (formValue.assuntoId) {
        this.buscarAssuntoPorId(formValue.assuntoId).subscribe(assunto => {
          if (assunto) {
            this.assuntos.push(assunto);
          }
        });
      }
    }
  }
}
