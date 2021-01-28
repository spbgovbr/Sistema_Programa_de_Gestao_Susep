import { Injectable } from "@angular/core";
import { FormControl } from '@angular/forms';

@Injectable()
export class ValidatorService {

  public noWhitespaceValidator(control: FormControl) {
    const isWhitespace = (control.value || '').trim().length === 0;
    const isValid = !isWhitespace;
    return isValid ? null : { 'whitespace': true };
  }

  public cpfValidator(control: FormControl) {

    const cpf = control.value;
    const valido = this.cpfValido(cpf);

    if (valido) return null;

    return { cpfInvalido: true };

  }

  public cpfValido(cpf: string): boolean {

    let soma = 0;
    let resto;

    if (cpf.length !== 11 || this._somenteNumerosIguais(cpf) || !this._somenteNumeros(cpf)) return false;
      
    for (let i=1; i<=9; i++) soma = soma + parseInt(cpf.substring(i-1, i)) * (11 - i);
    resto = (soma * 10) % 11;
    
    if ((resto == 10) || (resto == 11))  resto = 0;
    if (resto != parseInt(cpf.substring(9, 10)) ) return false;
    
    soma = 0;
    for (let i = 1; i <= 10; i++) soma = soma + parseInt(cpf.substring(i-1, i)) * (12 - i);
    resto = (soma * 10) % 11;
    
    if ((resto == 10) || (resto == 11))  resto = 0;
    if (resto != parseInt(cpf.substring(10, 11) ) ) return false;
    
    return true;

  }

  public confirmacaoEmailValidator(confirmPasswordInput: string) {
    let confirmPasswordControl: FormControl;
    let passwordControl: FormControl;
  
    return (control: FormControl) => {
      if (!control.parent) {
        return null;
      }
  
      if (!confirmPasswordControl) {
        confirmPasswordControl = control;
        passwordControl = control.parent.get(confirmPasswordInput) as FormControl;
        passwordControl.valueChanges.subscribe(() => {
          confirmPasswordControl.updateValueAndValidity();
        });
      }
  
      if (
        passwordControl.value !==
        confirmPasswordControl.value
      ) {
        return {
          notMatch: true
        };
      }
      return null;
    };
  
  }

  private _somenteNumerosIguais(cpf: string): boolean {
    const result = /0{11}|1{11}|2{11}|3{11}|4{11}|5{11}|6{11}|7{11}|8{11}|9{11}/g.exec(cpf) !== null;
    return result;
  }

  private _somenteNumeros(cpf: string): boolean {
    const result = /\D+/.exec(cpf) === null;
    return result;
  }
}
