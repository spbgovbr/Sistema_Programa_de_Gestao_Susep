import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IUnidade } from '../models/unidade.model';
import { IDadosPaginados } from '../../../shared/models/pagination.model';
import { UnidadeDataService } from '../services/unidade.service';

@Component({
  selector: 'unidade-pesquisa',
  templateUrl: './unidade-pesquisa.component.html',  
})
export class UnidadePesquisaComponent implements OnInit {

  dadosEncontrados: IDadosPaginados<IUnidade>;

  constructor(
    public router: Router,
    private unidadeDataService: UnidadeDataService
  ) { }

  ngOnInit() {
    this.pesquisar(1);
  }

  pesquisar(pagina: number) {
    //this.unidadeDataService.ObterPagina(pagina)
    //  .subscribe(
    //    resultado => {
    //      this.dadosEncontrados = resultado.retorno;
    //    }
    //  );
  }

}
