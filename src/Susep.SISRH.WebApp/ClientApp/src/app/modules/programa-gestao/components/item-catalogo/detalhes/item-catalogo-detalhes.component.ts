import { Component, OnInit } from '@angular/core';
import { ItemCatalogoDataService } from '../../../services/item-catalogo.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemCatalogo, IItemCatalogoAssunto } from '../../../models/item-catalogo.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DominioDataService } from '../../../../../shared/services/dominio.service';
import { IDominio } from '../../../../../shared/models/dominio.model';
import { PerfilEnum } from '../../../enums/perfil.enum';
import { DecimalValuesHelper } from '../../../../../shared/helpers/decimal-valuesr.helper';
import { ConfigurationService } from 'src/app/shared/services/configuration.service';
import { TipoModo } from 'src/app/shared/models/configuration.model';

@Component({
  selector: 'item-catalogo-detalhes',
  templateUrl: './item-catalogo-detalhes.component.html',
})
export class ItemCatalogoDetalhesComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  modoExibicao: TipoModo;

  assuntos: IItemCatalogoAssunto[] = [];

  formaCalculoTempo: IDominio[];
  dadosItemCatalogo: IItemCatalogo;

  tempoCalculadoPreviamente?: boolean;
  exibeDescricaoComplexidade?: boolean;
  permiteTrabalhoRemoto?: boolean = true;

  ganhoProdutividade: number;
  excluir: boolean;

  public tempoMask: any;

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private decimalValuesHelper: DecimalValuesHelper,
    private itemCatalogoDataService: ItemCatalogoDataService,
    private dominioDataService: DominioDataService,
    private configuracaoService: ConfigurationService,
  ) { }

  ngOnInit() {

    this.modoExibicao = this.configuracaoService.getModo();

    this.tempoMask = this.decimalValuesHelper.numberMask(3, 1);

    this.dominioDataService.ObterFormaCalculoTempoItemCatalogo().subscribe(
      appResult => {
        this.formaCalculoTempo = appResult.retorno;
      }
    );

    if (this.router.url.indexOf('/excluir/') > -1) {
      this.excluir = true;
    }

    this.carregarDados();    
  }

  carregarDados() {
    const itemCatalogoId = this.activatedRoute.snapshot.paramMap.get('id');
    if (itemCatalogoId) {
      this.itemCatalogoDataService.ObterItem(itemCatalogoId).subscribe(
        resultado => {
          this.dadosItemCatalogo = resultado.retorno;
          this.assuntos = this.dadosItemCatalogo.assuntos;
        }
      );
    }
  }

  excluirItem() {
    const itemCatalogoId = this.activatedRoute.snapshot.paramMap.get('id');
    this.itemCatalogoDataService.Excluir(itemCatalogoId).subscribe(
      r => {
        this.router.navigateByUrl(`/programagestao/catalogo/item`);
      });
  }

}
