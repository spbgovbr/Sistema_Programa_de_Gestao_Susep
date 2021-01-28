import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSlideToggleChange } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { IDadosPaginados } from '../../../../../shared/models/pagination.model';
import { ICatalogo, ICatalogoItemCatalogo } from '../../../models/catalogo.model';
import { IItemCatalogo } from '../../../models/item-catalogo.model';
import { IItemCatalogoPesquisa } from '../../../models/item-catalogo.pesquisa.model';
import { CatalogoDataService } from '../../../services/catalogo.service';
import { ItemCatalogoDataService } from '../../../services/item-catalogo.service';
import { PerfilEnum } from '../../../enums/perfil.enum';

@Component({
  selector: 'catalogo-edicao',
  templateUrl: './catalogo-edicao.component.html',  
})
export class CatalogoEdicaoComponent implements OnInit {

  PerfilEnum = PerfilEnum;


  form: FormGroup;
  dadosUltimaPesquisa: IItemCatalogoPesquisa = {};

  abaVisivel = 'unidade';
  
  dadosCatalogo: ICatalogo;
  dadosEncontrados: IDadosPaginados<IItemCatalogo>;
  itensUnidade: IItemCatalogo[];
  paginacao = new BehaviorSubject<IDadosPaginados<IItemCatalogo>>(null);
  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private catalogoDataService: CatalogoDataService,
    private itemCatalogoDataService: ItemCatalogoDataService,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      titulo: ['', []],
    });

    this.carregarDados();
    this.pesquisar(1);
  }

  carregarDados() {
    const unidadeId = this.activatedRoute.snapshot.paramMap.get('id');
    if (unidadeId) {
      this.catalogoDataService.ObterPorUnidade(Number(unidadeId)).subscribe(
        resultado => {
          this.dadosCatalogo = resultado.retorno;         
        }
      );
    }
  }

  pesquisar(pagina: number) {
    this.dadosUltimaPesquisa = this.form.value;
    this.dadosUltimaPesquisa.page = pagina;
    this.itemCatalogoDataService.ObterPagina(this.dadosUltimaPesquisa).subscribe(
      resultado => {
        this.dadosEncontrados = resultado.retorno;
        
      }
    );
  }

  onSubmit() {
    this.pesquisar(1);
  }

  marcarItem(itemCatalogoId: string) {
    return !!this.dadosCatalogo.itens.find(element => element.itemCatalogoId === itemCatalogoId);
  }

  associarItem(itemCatalogoCheck: MatSlideToggleChange, itemCatalogoId: string) {

    //Se item for marcado ele será associado ao catalogo
    if (itemCatalogoCheck.checked) {
      const dados: ICatalogoItemCatalogo = { 'catalogoId': this.dadosCatalogo.catalogoId, 'itemCatalogoId': itemCatalogoId };
      this.catalogoDataService.CadastrarItem(dados).subscribe(
        resultado => {
          
            this.carregarDados();
        }
      );
    } else { //se o item for desmarcado então será desassociado do catálogo
      this.catalogoDataService.ExcluirItem(this.dadosCatalogo.catalogoId, itemCatalogoId).subscribe(
        resultado => {
       
            this.carregarDados();        
        }
      );
    }  
  } 
 
  alterarAba(aba: string) {
    this.abaVisivel = aba;
  }
}
