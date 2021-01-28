import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'unidade-edicao',
  templateUrl: './unidade-edicao.component.html',
})
export class UnidadeEdicaoComponent implements OnInit {

  constructor(
    public router: Router,    
  ) { }

  ngOnInit() {
  }
}
