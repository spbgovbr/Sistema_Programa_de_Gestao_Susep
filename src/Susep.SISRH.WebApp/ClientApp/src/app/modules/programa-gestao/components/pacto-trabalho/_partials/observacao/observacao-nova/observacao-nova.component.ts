import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { IPactoTrabalho, IPactoTrabalhoInformacao } from '../../../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../../../services/pacto-trabalho.service';

@Component({
  selector: 'observacao-pacto-nova',
  templateUrl: './observacao-nova.component.html',
})
export class ObservacoesPactoNovaComponent implements OnInit {

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;

  pactoTrabalhoId: string;

  form: FormGroup;

  constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private pactoTrabalhoDataService: PactoTrabalhoDataService) { }

  ngOnInit() {

    this.form = this.formBuilder.group({
      informacao: [null, []],
    });

    this.dadosPacto.subscribe(val => {
      this.pactoTrabalhoId = this.dadosPacto.value.pactoTrabalhoId;
    });

  }

  cadastrarObservacao() {
    if (this.form.valid) {
      const dados: IPactoTrabalhoInformacao = this.form.value;

      this.pactoTrabalhoDataService.RegistrarInformacao(this.pactoTrabalhoId, dados).subscribe(
        r => {
          this.dadosPacto.value.informacoes.push(r.retorno);
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
}
