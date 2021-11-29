import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { IDominio } from '../../../../../../../shared/models/dominio.model';
import { DominioDataService } from '../../../../../../../shared/services/dominio.service';
import { IPactoTrabalho, IPactoTrabalhoAtividade } from '../../../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../../../services/pacto-trabalho.service';
import { CatalogoDataService } from '../../../../../services/catalogo.service';
import { IItemCatalogo } from '../../../../../models/item-catalogo.model';

@Component({
  selector: 'alteracao-prazo-pacto',
  templateUrl: './alteracao-prazo.component.html',
})
export class AlteracaoPrazoPactoComponent implements OnInit {

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;

  form: FormGroup;
  minDataFim: any;

  constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private pactoTrabalhoDataService: PactoTrabalhoDataService,
    private catalogoDataService: CatalogoDataService,
    private dominioDataService: DominioDataService) { }

  ngOnInit() {  

    this.form = this.formBuilder.group({
      dataFim: [null, []],
      descricao: [null, []],
    });

    this.dadosPacto.subscribe(p => {
      this.minDataFim = this.formatarData(new Date(this.dadosPacto.value.dataInicio));
    });
  }

  alterarPrazo() {
    if (this.form.valid) {
      const dados: IPactoTrabalhoAtividade = this.form.value;
      dados.pactoTrabalhoId = this.dadosPacto.value.pactoTrabalhoId;
      this.pactoTrabalhoDataService.ProporAlteracaoPrazo(dados).subscribe(
        r => {
          this.dadosPacto.next(this.dadosPacto.value);
        });
    }
    else {
      this.getFormValidationErrors(this.form)
    }
  }

  getFormValidationErrors(form) {
    Object.keys(form.controls).forEach(field => {
      const control = form.get(field);
      control.markAsDirty({ onlySelf: true });
    });
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

  formatarData(data: Date) {
    return {
      'year': data.getFullYear(),
      'month': data.getMonth() + 1,
      'day': data.getDate()
    };
  }
}
