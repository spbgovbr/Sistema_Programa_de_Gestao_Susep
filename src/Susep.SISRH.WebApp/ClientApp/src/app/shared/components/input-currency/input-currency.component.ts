import { Component, forwardRef, HostListener, Input } from '@angular/core';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { map } from 'rxjs/operators';

@Component({
  selector: 'input-currency',
  templateUrl: './input-currency.component.html',
  providers: [
  { 
    provide: NG_VALUE_ACCESSOR,
    multi: true,
    useExisting: forwardRef(() => InputCurrencyComponent),
  }
]
})
export class InputCurrencyComponent implements ControlValueAccessor {

  @Input() separadorDecimais: string = ',';
  @Input() numeroCasasDecimais: number = 2;

  control = new FormControl();
  disabled: boolean;

  @HostListener('keypress', ['$event'])
  onKeyPress(e: KeyboardEvent) {
    if (!this.isCaracterPermitido(e)) 
      e.preventDefault();
    this.removerDuplicacaoSeparadorDecimais(e);
  }

  @HostListener('keyup', ['$event'])
  onKeyUp(e: KeyboardEvent) {
    this.truncarCasasDecimais(e);
  }

  blur(event: FocusEvent) {
    if (!this.isFormatoValorPermitido()) {
      this.control.patchValue('');
    }
  }

  // Implementação ControlValueAccessor
  writeValue(value: number): void {
    this.control.patchValue(this.modelToInput(value));
  }

  registerOnChange(fn: any): void {
    this.control.valueChanges.pipe(
      map(v => this.inputToModel(v)),
    ).subscribe(fn);
  }

  registerOnTouched(fn: any): void {
    this.control.valueChanges.subscribe(fn);
  }

  setDisabledState?(isDisabled: boolean): void {}

  // Metodos auxiliares
  private modelToInput(model: number): string {
    if (!model && model !== 0) return '';
    const parts = model.toString().split(/\D/);
    return parts.length > 1 ? `${parts[0]},${parts[1]}` : `${parts[0]}`;
  }

  private inputToModel(input: string): number {
    if (!input || !input.trim().length) return null;
    if (/^\d+$/g.test(input)) return parseInt(input);
    if (/^\d+\D(\d+)?$/g.test(input)) return parseFloat(input.split(/\D/).join('.'));
  }

  private isCaracterPermitido(c: KeyboardEvent): boolean {
    return /\d/g.test(c.key) || this.separadorDecimais === c.key;
  }

  private isFormatoValorPermitido(): boolean {
    const value = this.control.value;
    return /^$/g.test(value) 
        || /^\d+$/g.test(value)
        || /^\d+\D\d+$/g.test(value);
  }

  private removerDuplicacaoSeparadorDecimais(e: KeyboardEvent) {
    const value: string = this.control.value;
    if (value && value.includes(this.separadorDecimais) && e.key === this.separadorDecimais) {
      this.control.patchValue(value.replace(this.separadorDecimais, ''));
    }
  }

  private truncarCasasDecimais(e: KeyboardEvent) {
    const value = this.control.value;
    const parts = value.split(/\D/);
    if (parts && parts.length > 1 && parts[1].length > this.numeroCasasDecimais) {
      parts[1] = parts[1].substring(0, this.numeroCasasDecimais);
      this.control.patchValue(parts.join(this.separadorDecimais));
    }
  }
}