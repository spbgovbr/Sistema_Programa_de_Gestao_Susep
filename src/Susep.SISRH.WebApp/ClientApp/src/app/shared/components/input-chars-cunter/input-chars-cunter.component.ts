import { AfterContentInit, Component, ElementRef, Input, OnInit } from '@angular/core';

@Component({
  selector: 'input-chars-cunter',
  templateUrl: './input-chars-cunter.component.html',
  styleUrls: ['./input-chars-cunter.component.css']
})
export class InputCharsCunterComponent implements OnInit {

  @Input() element: any;

  constructor(private elementRef: ElementRef) {
  }

  ngOnInit() {

  }

}
