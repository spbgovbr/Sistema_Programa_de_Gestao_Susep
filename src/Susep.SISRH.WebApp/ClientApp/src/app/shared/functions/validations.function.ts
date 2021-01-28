import { ValidatorFn, FormControl, ValidationErrors, AbstractControl } from "@angular/forms";

export function maxDateValidator(maxDate: Date): ValidatorFn {
  return (control: FormControl) => {
    if (!maxDate) return null;
    if (!control.value) return null;
    if (control.value <= maxDate) return null;
    return { 'maiorQueDataMaxima': maxDate };
  }
}

export function minDateValidator(minDate: Date): ValidatorFn {
  return (control: FormControl) => {
    if (!minDate) return null;
    if (!control.value) return null;
    if (control.value >= minDate) return null;
    return { 'menorQueDataMinima': minDate };
  }
}

export function validacaoCondicional(predicate: () => boolean, validator: (AbstractControl) => ValidationErrors | ValidatorFn) {
  return (formControl => {
    if (!formControl.parent) return null;
    if (predicate()) return validator(formControl);
    return null;
  });
}