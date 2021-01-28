import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { IControlePaginacao } from '../../../models/pagination.model';

@Component({
  moduleId: module.id,
  selector: 'app-pagination',
  templateUrl: './pagination.component.html' 
})

export class PaginationComponent implements OnInit {
  @Input() controlePaginacao: IControlePaginacao;  
  @Output() changePage = new EventEmitter<any>();

  ngOnInit() {   
  }

  paginar(pagina: number) {
    if (pagina >= 1 && pagina <= this.controlePaginacao.totalPaginas && pagina !== this.controlePaginacao.paginaAtual)
      this.changePage.emit(pagina);
  }

  counter(i: number) {
    return new Array(i);
  }

}
