import { Component, OnInit, Input, ContentChild, AfterContentInit, Renderer2, ViewChild, ElementRef } from '@angular/core';
import { NgModel, FormControlName } from '@angular/forms';

@Component({
  selector: 'field-validation',
  templateUrl: './input-validation.component.html',
  styleUrls: ['./input-validation.component.css']
})
export class InputValidationComponent implements OnInit, AfterContentInit {

  input: any

  @Input() label: string;

  @Input() errorMessage: string;

  @Input() classList: string[];

  @ContentChild(NgModel, { static: false }) model: NgModel;

  @ContentChild(FormControlName, { static: true }) control: FormControlName;

  @ViewChild('divInput', { static: true }) div: ElementRef;

  constructor(private renderer: Renderer2) { }

  ngOnInit() {}

  ngAfterContentInit() {
    this.input = this.model || this.control;
    if (!this.input)
      throw new Error('A diretiva ngModel ou formControlName deve ser informada.');
  }

  hasSuccess(): boolean {
    const success = this.input.valid && (this.input.dirty || this.input.touched);
//    if (success) this._removeBorder();
   	return success;
  }

  hasError(): boolean {
		const error = this.input.invalid && (this.input.dirty || this.input.touched);
//		if (error) this._addBorder();
    return error;
  }

  private _addBorder() {
		const _elInput: ElementRef = this.div.nativeElement.children[1];
    this.renderer.addClass(_elInput, 'border');
    this.renderer.addClass(_elInput, 'border-danger');
  }

  private _removeBorder() {
    const _elInput = this.div.nativeElement.children[1];
    this.renderer.removeClass(_elInput, 'border');
    this.renderer.removeClass(_elInput, 'border-danger');
  }

}
