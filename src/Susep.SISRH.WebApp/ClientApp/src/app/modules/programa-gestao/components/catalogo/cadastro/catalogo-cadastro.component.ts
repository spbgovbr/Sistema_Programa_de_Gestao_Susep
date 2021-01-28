import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { IDadosCombo } from '../../../../../shared/models/dados-combo.model';
import { Router } from '@angular/router';
import { CatalogoDataService } from '../../../services/catalogo.service';
import { UnidadeDataService } from '../../../../unidade/services/unidade.service';
import { ICatalogoPesquisa } from '../../../models/catalogo.pesquisa.model';
import { ICatalogo } from '../../../models/catalogo.model';
import { PerfilEnum } from '../../../enums/perfil.enum';

@Component({
  selector: 'catalogo-cadastro',
  templateUrl: './catalogo-cadastro.component.html',
})
export class CatalogoCadastroComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  form: FormGroup;
  unidadesCadastroCombo: IDadosCombo[];

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private catalogoDataService: CatalogoDataService,
    private unidadeDataService: UnidadeDataService
  ) { }

  ngOnInit() {

    this.unidadeDataService.ObterSemCatalogoCadastrado().subscribe(
      appResult => {
        this.unidadesCadastroCombo = appResult.retorno;
      }
    );
    this.form = this.formBuilder.group({
      unidadeId: ['', []],

    });
  }

  cadastrar() {
    if (this.form.valid) {

      const dados: ICatalogo = this.form.value;

      if (!!dados.unidadeId) {
        this.catalogoDataService.Cadastrar(dados).subscribe(
          r => {
            this.router.navigate(['/programagestao/catalogo/editar/' + dados.unidadeId])
          });
      }
    } else {
      this.getFormValidationErrors(this.form)
    }

  }

  getFormValidationErrors(form) {
    Object.keys(form.controls).forEach(field => {
      const control = form.get(field);
      control.markAsDirty({ onlySelf: true });
    });
  }

}
