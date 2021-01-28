import { AfterContentInit, Directive, ViewChild, OnInit, HostListener, Renderer2 } from '@angular/core';
import { fromEvent } from 'rxjs';
import { MatInput } from '@angular/material';
import { DatepickerHelper } from './date-picker.helper';

@Directive({
  selector: '[mask-datepicker]'
})
export class InputDatepickerDirective {

  private _helper: DatepickerHelper;

  constructor() {
    this._helper = DatepickerHelper.getInstance();
  }

  @HostListener('keydown', ['$event'])
  keyDown() {
    this._helper.onKeyDown(<KeyboardEvent>event);
  }

}