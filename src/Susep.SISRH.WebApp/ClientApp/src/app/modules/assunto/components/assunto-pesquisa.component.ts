import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IAssunto } from '../models/assunto.model';
import { IDadosPaginados } from '../../../shared/models/pagination.model';
import { AssuntoDataService } from '../services/assunto.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { IAssuntoPesquisa } from '../models/assunto.pesquisa.model';
import { BehaviorSubject } from 'rxjs';
import { PerfilEnum } from '../../programa-gestao/enums/perfil.enum';

@Component({
  selector: 'assunto-pesquisa',
  templateUrl: './assunto-pesquisa.component.html',  
})
export class AssuntoPesquisaComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  dadosEncontrados: IDadosPaginados<IAssunto>;
  dadosUltimaPesquisa: IAssuntoPesquisa = {};
  paginacao = new BehaviorSubject<IDadosPaginados<IAssunto>>(null);

  form: FormGroup;

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private assuntoDataService: AssuntoDataService
  ) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      valor: [null]
    });
    this.pesquisar(1);
  }

  pesquisar(pagina: number) {

    if (this.form.valid) {
      this.dadosUltimaPesquisa = this.form.value;
      this.dadosUltimaPesquisa.page = pagina;
  
      this.assuntoDataService.ObterPagina(this.dadosUltimaPesquisa)
        .subscribe(resultado => this.dadosEncontrados = resultado.retorno);
    }

  }

  changePage(pagina: number) {
    this.pesquisar(pagina);
  }

  onSubmit() {
    this.pesquisar(1);
  }

}
