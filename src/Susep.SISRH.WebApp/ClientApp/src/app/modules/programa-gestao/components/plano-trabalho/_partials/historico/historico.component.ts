import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { IPlanoTrabalhoAtividade, IPlanoTrabalho, IPlanoTrabalhoAtividadeItem } from '../../../../models/plano-trabalho.model';
import { BehaviorSubject } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DominioDataService } from '../../../../../../shared/services/dominio.service';
import { IDominio } from '../../../../../../shared/models/dominio.model';
import { CatalogoDataService } from '../../../../services/catalogo.service';
import { PlanoTrabalhoDataService } from '../../../../services/plano-trabalho.service';
import { IItemCatalogo } from '../../../../models/item-catalogo.model';
import { UnidadeDataService } from '../../../../../unidade/services/unidade.service';
import { IDadosCombo } from '../../../../../../shared/models/dados-combo.model';
import { MatSlideToggleChange } from '@angular/material';

@Component({
  selector: 'plano-historico',
  templateUrl: './historico.component.html',  
})
export class PlanoHistoricoComponent implements OnInit {

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;

  form: FormGroup;

  constructor(
    private planoTrabalhoDataService: PlanoTrabalhoDataService,) { }

  ngOnInit() {

    this.dadosPlano.subscribe(val => this.carregarHistorico());
  }

  carregarHistorico() {
    this.planoTrabalhoDataService.ObterHistorico(this.dadosPlano.value.planoTrabalhoId).subscribe(
      resultado => {
        this.dadosPlano.value.historico = resultado.retorno;     
      }
    );
  }

}
