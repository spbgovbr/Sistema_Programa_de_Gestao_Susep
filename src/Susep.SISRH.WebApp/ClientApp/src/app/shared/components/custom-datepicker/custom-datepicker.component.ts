import { Component, EventEmitter, forwardRef, Input, Output, Provider } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors, Validator } from '@angular/forms';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { CustomDateParserFormatter } from './custom-dataparser.formatter';

const CUSTOM_NG_VALUE_ACCESSOR: Provider = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CustomDatepickerComponent),
  multi: true
}

const CUSTOM_NGB_DATEPICKER_ADAPTER: Provider = {
  provide: NgbDateAdapter, 
  useClass: NgbDateNativeAdapter
}

const CUSTOM_NGB_DATEPICKER_FORMATTER: Provider = {
  provide: NgbDateParserFormatter, 
  useClass: CustomDateParserFormatter
}

const CUSTOM_VALIDATOR: Provider = { 
  provide: NG_VALIDATORS, 
  useExisting: forwardRef(() => CustomDatepickerComponent),
  multi: true 
}

@Component({
  selector: 'custom-datepicker',
  templateUrl: './custom-datepicker.component.html',
  styleUrls: ['./custom-datepicker.component.css'],
  providers: [
    CUSTOM_NG_VALUE_ACCESSOR,
    CUSTOM_NGB_DATEPICKER_ADAPTER,
    CUSTOM_NGB_DATEPICKER_FORMATTER,
    CUSTOM_VALIDATOR
  ]
})
export class CustomDatepickerComponent implements ControlValueAccessor, Validator {

  @Output() change: EventEmitter<any> = new EventEmitter();

  @Input() timePicker = false;
  @Input() minDate: any;
  @Input() maxDate: any;

  time = { hour: null, minute: null };

  private _value = new BehaviorSubject<Date>(null);

  get value(): Date {
    const date = this._value.value;
    if (date && date.setHours) {
      date.setHours(this.time.hour);
      date.setMinutes(this.time.minute);
    }
    return date;
  }

  set value(v: Date) {
    this._value.next(v);
    if (!v) v = new Date();
    if (!this.time.hour) this.time.hour = v.getHours();
    if (!this.time.minute) this.time.minute = v.getMinutes();
    if (this.change)
      this.change.emit();
  }

  disabled: boolean;

  // Implementando CustomValueAccessor
  writeValue(v: Date): void {
    if (typeof(v) === 'string')
      v = new Date(v);
    this.value = v;
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

  // Implementando Validator
  validate(control: AbstractControl): ValidationErrors {
    const data = control.value;
    return this.isValid(data) ? null: { invalidData: data }
  }

  registerOnValidatorChange?(fn: () => void): void {
    this._value.subscribe(fn);
  }

  private isValid(date: any): boolean {    
    if (!date) return false;
    if (typeof (date) === 'string') {
      return /\d{4}-\d{2}-\d{2}\D\d{2}\:\d{2}\:\d{2}/.test(date);
    }
    const parsed = Date.parse(date);
    if (!parsed || isNaN(parsed)) return false;
    const newDate = new Date(parsed);
    if (!newDate) return false;
    const fullYear = date.getFullYear();
    return fullYear && fullYear.toString().length === 4;
  }
}
