import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IObjeto } from '../models/objeto.model';
import { IDadosPaginados } from '../../../shared/models/pagination.model';
import { ObjetoDataService } from '../services/objeto.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { IObjetoPesquisa } from '../models/objeto.pesquisa.model';
import { BehaviorSubject } from 'rxjs';
import { PerfilEnum } from '../../programa-gestao/enums/perfil.enum';
import { TipoObjetoEnum } from '../enums/tipo-objeto.enum';
import { EnumHelper } from 'src/app/shared/helpers/enum.helper';

@Component({
  selector: 'objeto-pesquisa',
  templateUrl: './objeto-pesquisa.component.html',  
})
export class ObjetoPesquisaComponent implements OnInit {

  PerfilEnum = PerfilEnum;
  TipoObjetoEnum = TipoObjetoEnum;

  dadosEncontrados: IDadosPaginados<IObjeto>;
  dadosUltimaPesquisa: IObjetoPesquisa = {};
  paginacao = new BehaviorSubject<IDadosPaginados<IObjeto>>(null);

  tipos = EnumHelper.toList(TipoObjetoEnum);

  form: FormGroup;

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private objetoDataService: ObjetoDataService
  ) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      descricao: [null],
      tipo: [null],
      chave: [null],
    });
    this.pesquisar(1);
  }

  pesquisar(pagina: number) {

    if (this.form.valid) {
      this.dadosUltimaPesquisa = this.form.value;
      this.dadosUltimaPesquisa.page = pagina;
  
      this.objetoDataService.ObterPagina(this.dadosUltimaPesquisa)
        .subscribe(resultado => this.dadosEncontrados = resultado.retorno);
    }

  }

  onSubmit() {
    this.pesquisar(1);
  }

  obterDescricaoTipo(id: string): string {
    const tipo = this.tipos.find(t => t.id === Number.parseInt(id));
    return tipo ? tipo.descricao : '';
  }

}
