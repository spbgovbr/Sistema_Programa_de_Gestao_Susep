import { Component, EventEmitter, Output, Self, Optional, Input } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'input-rating',
  templateUrl: './input-rating.component.html',
  styleUrls: ['./input-rating.component.css'],
})
export class InputRatingComponent implements ControlValueAccessor
{

  @Output() change: EventEmitter<any> = new EventEmitter();

  @Input() notaLimite?: number;

  private _value = new BehaviorSubject<number>(null);

  get value(): number[] {
    return this.ratingToArray(this._value.value);
  }

  set value(v: number[]) {
    this._value.next(this.arrayToRating(v));
    if (this.change)
      this.change.emit(this._value.value);
  }

  private _posicaoHover = null;

  get posicao(): number {
    return this._posicaoHover ? this._posicaoHover : this._value.value;
  }

  disabled: boolean;

  constructor(@Self() @Optional() public control: NgControl) {
    this.control && (this.control.valueAccessor = this);
  }

  getPosicao = (indice: string) => parseInt(indice) + 1;

  click(evt: MouseEvent, key: string) {
    if (!this.disabled) {
      const index = parseInt(key);
      this.writeValue(index + 1);
    }
  }

  mouseover(key) {
    if (!this.disabled) {
      this._posicaoHover = parseInt(key) + 1;
    }
  }

  mouseout() {
    if (!this.disabled) {
      this._posicaoHover = null;
    }
  }

  // Implementando CustomValueAccessor
  writeValue(v: number): void {
    this.value = this.ratingToArray(v);
  }

  registerOnChange(fn: any): void {
    this._value.subscribe(fn);
  }

  registerOnTouched(fn: any): void {
    this._value.subscribe(fn);
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  // Validação do formulário
  public get invalid(): boolean {
    return this.control ? this.control.invalid : false;
  }

  public get showError(): boolean {
    if (!this.control) return false;

    const { dirty, touched } = this.control;

    return this.invalid ? (dirty || touched) : false;
  }

  public get errors(): string[] {
    if (!this.control) return [];

    const { errors } = this.control;
    return Object.keys(errors).map(key => <string>errors[key] || key);
  }

  private ratingToArray(rating: number): number[] {

    const estrelas = [0,0,0,0,0,0,0,0,0,0];
    
    if (!rating) return [...estrelas];
    if (rating <= 0) return [...estrelas];
    if (rating >= 10) return [...estrelas].map(v => 1);
    
    return [...estrelas].map((item, i) => {
      if (i < rating) return 1;
      return 0;
    });
  }

  private arrayToRating(array: number[]): number {
    return array ? array.reduce((a, v) => a + v) : 0;
  }

}
