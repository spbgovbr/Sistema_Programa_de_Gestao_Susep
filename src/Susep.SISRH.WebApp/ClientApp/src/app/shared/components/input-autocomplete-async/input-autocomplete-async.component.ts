import { Component, Input, forwardRef, AfterViewInit, ViewChild, ElementRef, Output, EventEmitter, Pipe, PipeTransform, OnDestroy } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS, Validator, FormControl, AbstractControl, ValidationErrors, Validators } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { debounceTime, filter, tap, switchMap, finalize, map, startWith, distinctUntilChanged } from 'rxjs/operators';
import { FiltrarItensJaEscolhidosPipe } from './filtrar-ja-escolhidos.pipe';

@Component({
  selector: 'input-autocomplete-async',
  templateUrl: './input-autocomplete-async.component.html',
  styleUrls: ['./input-autocomplete-async.component.css'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => InputAutocompleteAsyncComponent),
    multi: true
  },
  {
    provide: NG_VALIDATORS,
    useExisting:  forwardRef(() => InputAutocompleteAsyncComponent),
    multi: true
  }
]
})
export class InputAutocompleteAsyncComponent implements ControlValueAccessor, AfterViewInit, OnDestroy, Validator
{

  @ViewChild('inputtext', { static: true }) inputText: ElementRef;

  @Input() datasource: IDatasourceAutocompleteAsync;
  @Input() tamanhoMinimoInformado?: number = 3;
  @Input() tempoEsperaEmMilisegundos?: number = 500;
  @Input() modo?: 'selecao' | 'adicao' = 'selecao';
  @Input() chavesJaEscolhidas: any[] = []

  @Output() acaoBotao = new EventEmitter();

  control = new FormControl('', Validators.minLength(this.tamanhoMinimoInformado));

  itensLista: IChaveDescricao[] = [];

  isLoading = false;

  disabled = false;

  private subscription: Subscription;

  constructor() {}

  ngAfterViewInit() {

    this.subscription = this.control.valueChanges.pipe(
      debounceTime(this.tempoEsperaEmMilisegundos),
  //    startWith(''),
      filter(value => value && value.length >= this.tamanhoMinimoInformado),
  //    distinctUntilChanged(),
      tap(() => {
        this.isLoading = true;
        this.inputText.nativeElement.blur(); // Previne erro do ng-bootstrap no console
      }),
      switchMap(v => this.atualizarLista(v)),
    ).subscribe(itens => this.itensLista = itens);

  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  private atualizarLista(valor: string): Observable<IChaveDescricao[]> {
    return this.datasource.buscarPorValor(valor).pipe(
      map(v => v.map(i => this.datasource.modelToValue(i))),
      finalize(() => this.isLoading = false));
  }

  displayFn(item: IChaveDescricao): string {
    return item ? item.descricao : '';
  }

  limpar() {
    if (this.control && this.control.value) {
      this.control.setValue(null);
      this.itensLista = [];
      this.inputText.nativeElement.value = null;
    }
  }

  click(event) {
    this.acaoBotao.emit();
    this.limpar();
  }

  // Implementando CustomValueAccessor
  writeValue(chave: any): void {
    this.datasource.buscarPorChave(chave).subscribe(o => {
      this.control.setValue(o ? this.datasource.modelToValue(o) : null);
    });
  }

  registerOnChange(fn: any): void {
    this.control.valueChanges.pipe(
      map(v => v ? v.chave : null),
    ).subscribe(fn);
  }

  registerOnTouched(fn: any): void {
    this.control.valueChanges.pipe(
    ).subscribe(fn);
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
    this.disabled ? this.control.disable() : this.control.enable();
  }

  // Implementando Validator
  validate(control: AbstractControl): ValidationErrors {
    if (!control) return null;
    const value = control.value;
    if (value === undefined) return { 'invalid': true };
    return null;
  }
}

export interface IChaveDescricao {
  chave: any;
  descricao: string;
}

export interface IDatasourceAutocompleteAsync {
  buscarPorChave(chave: any): Observable<any>;
  buscarPorValor(valor: string): Observable<any[]>;
  modelToValue(model: any): IChaveDescricao;
}

export interface IConfiguracoesAutocompleteAsync {
  tamanhoMinimoInformado?: number;
  tempoEsperaEmMilisegundos?: number;
  modo?: 'selecao' | 'adicao';
}

