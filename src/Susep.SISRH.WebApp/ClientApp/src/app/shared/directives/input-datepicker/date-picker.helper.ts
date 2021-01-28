import { MatDatepickerInputEvent } from "@angular/material";

export class DatepickerHelper {

	private constructor() {}

	static getInstance(): DatepickerHelper {
		return new DatepickerHelper();
	}

	onDateInput(event: MatDatepickerInputEvent<any>): void {
//		let _value = <string>event.targetElement['value'];
	}
	  
	onKeyDown(event: KeyboardEvent):void {
		
		let _value = event.target['value'];
		
		if (!this._teclasPermitidas(event) && !this._coincide(/\d/, event.key)) {
		  event.preventDefault();
		  return;
		}
	
		if (this._coincide(/\d/, event.key)) {
	
		  if ((_value.length === 2 || _value.length === 5)) {
			event.target['value'] += '/';
			_value = event.target['value'];
		  }
	
		  if ( _value.length >= 10) {
			event.preventDefault();
			return;
		  }
	
		  if (_value.length === 0 && !this._coincide(/[0-3]/, event.key)) {
			event.preventDefault();
			return;
		  }
	
		  if (_value.length === 3 && !this._coincide(/[0-1]/, event.key)) {
			event.preventDefault();
			return;
		  }
	
		  if (_value.length === 6 && !this._coincide(/[1-2]/, event.key)) {
			event.preventDefault();
			return;
		  }
	
		  if (_value.length === 7 && !this._coincide(/[0,9]/, event.key)) {
			event.preventDefault();
			return;
		  }
	
		}
	}

	private _teclasPermitidas(event: KeyboardEvent) {
    return event.code === 'Backspace' 
        || event.code === 'Tab' 
        || event.code === 'Delete';
  }

  private _coincide(padrao: RegExp, caracter: string): boolean {
    const resultadoPadrao = padrao.exec(caracter);
    return (resultadoPadrao && resultadoPadrao.length > 0);
  }
	
}