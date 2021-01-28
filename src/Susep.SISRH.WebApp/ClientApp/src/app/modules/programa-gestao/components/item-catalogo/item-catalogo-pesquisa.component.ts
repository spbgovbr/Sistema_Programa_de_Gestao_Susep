import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { DominioDataService } from '../../../../shared/services/dominio.service';
import { ItemCatalogoDataService } from '../../services/item-catalogo.service';
import { IDadosPaginados } from '../../../../shared/models/pagination.model';
import { IItemCatalogo } from '../../models/item-catalogo.model';
import { IDominio } from '../../../../shared/models/dominio.model';
import { IItemCatalogoPesquisa } from '../../models/item-catalogo.pesquisa.model';
import { PerfilEnum } from '../../enums/perfil.enum';

@Component({
  selector: 'item-catalogo-pesquisa',
  templateUrl: './item-catalogo-pesquisa.component.html',
})
export class ItemCatalogoPesquisaComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  form: FormGroup;
  dadosUltimaPesquisa: IItemCatalogoPesquisa = {};
  formaCalculoTempo: IDominio[];
  dadosEncontrados: IDadosPaginados<IItemCatalogo>;

  paginacao = new BehaviorSubject<IDadosPaginados<IItemCatalogo>>(null);

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private itemCatalogoDataService: ItemCatalogoDataService,
    private dominioDataService: DominioDataService
  ) { }

  ngOnInit() {

    this.dominioDataService.ObterFormaCalculoTempoItemCatalogo().subscribe(
      appResult => {
        this.formaCalculoTempo = appResult.retorno;
      }
    );

    this.form = this.formBuilder.group({
      titulo: ['', []],
      formaCalculoTempoId: [null, []],
      permiteTrabalhoRemoto: [null, []],
    });

    this.pesquisar(1);
  }

  pesquisar(pagina: number) {
    
    this.dadosUltimaPesquisa = this.form.value;
    this.dadosUltimaPesquisa.page = pagina;

    this.itemCatalogoDataService.ObterPagina(this.dadosUltimaPesquisa)
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
