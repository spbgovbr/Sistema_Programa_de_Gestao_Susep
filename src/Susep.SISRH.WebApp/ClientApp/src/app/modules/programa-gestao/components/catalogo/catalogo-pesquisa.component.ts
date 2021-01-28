import { Component, OnInit } from '@angular/core';
import { IDadosPaginados } from '../../../../shared/models/pagination.model';
import { ICatalogo } from '../../models/catalogo.model';
import { Router } from '@angular/router';
import { CatalogoDataService } from '../../services/catalogo.service';
import { ICatalogoPesquisa } from '../../models/catalogo.pesquisa.model';
import { FormGroup, FormBuilder } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { UnidadeDataService } from '../../../unidade/services/unidade.service';
import { IUnidade } from '../../../unidade/models/unidade.model';
import { IDadosCombo } from '../../../../shared/models/dados-combo.model';
import { PerfilEnum } from '../../enums/perfil.enum';

@Component({
  selector: 'catalogo-pesquisa',
  templateUrl: './catalogo-pesquisa.component.html',  
})
export class CatalogoPesquisaComponent implements OnInit {

  PerfilEnum = PerfilEnum;


  form: FormGroup;
  dadosEncontrados: IDadosPaginados<ICatalogo>;
  dadosUltimaPesquisa: ICatalogoPesquisa = {};
  unidadesAtivasCombo: IDadosCombo[];
  paginacao = new BehaviorSubject<IDadosPaginados<ICatalogo>>(null);

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private catalogoDataService: CatalogoDataService,
    private unidadeDataService: UnidadeDataService
  ) { }

  ngOnInit() {

    this.unidadeDataService.ObterAtivasDadosCombo().subscribe(
      appResult => {
        this.unidadesAtivasCombo = appResult.retorno;
      }
    );
    this.form = this.formBuilder.group({
      unidadeId: ['', []],
      
    });

    this.pesquisar(1);
  }

  pesquisar(pagina: number) {

    this.dadosUltimaPesquisa = this.form.value;
    this.dadosUltimaPesquisa.page = pagina;

    this.catalogoDataService.ObterPagina(this.dadosUltimaPesquisa)
      .subscribe(
        resultado => {
          this.dadosEncontrados = resultado.retorno;
          this.paginacao.next(this.dadosEncontrados);
        }
      );
  }

  onSubmit() {
    this.pesquisar(1);
  }

}
