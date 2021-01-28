import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { IPactoTrabalho, IPactoTrabalhoAtividade } from '../../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../../services/pacto-trabalho.service';
import { PerfilEnum } from '../../../../enums/perfil.enum';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'pacto-cabecalho',
  templateUrl: './cabecalho.component.html',  
})
export class PactoCabecalhoComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  form: FormGroup;
  minDate = new Date();

  minDataInicio: any;
  minDataConclusao: any;

  @ViewChild('modalDatas', { static: true }) modalDatas;

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;
  @Input() atividades: BehaviorSubject<IPactoTrabalhoAtividade[]>;
  @Input() unidade: BehaviorSubject<number>;
  @Input() servidor: BehaviorSubject<number>;

  constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private pactoTrabalhoDataService: PactoTrabalhoDataService,) { }

  ngOnInit() {

    this.form = this.formBuilder.group({
      dataInicio: [null, [Validators.required]],
      dataFim: [null, [Validators.required]],
    });

    this.dadosPacto.subscribe(pacto => this.fillForm());
  }

  editarPeriodo() {
    this.modalService.open(this.modalDatas, { size: 'sm' });
  }

  fillForm() {
    this.form.patchValue({
      dataInicio: this.dadosPacto.value.dataInicio,
      dataFim: this.dadosPacto.value.dataFim,
    });
  }

  confirmarAlteracaoDatas() {
    if (this.form.valid) {
      const dados: IPactoTrabalho = this.form.value;
      dados.pactoTrabalhoId = this.dadosPacto.value.pactoTrabalhoId;
      this.pactoTrabalhoDataService.AlterarPeriodo(dados).subscribe(
        r => {
          this.dadosPacto.value.dataInicio = this.form.get('dataInicio').value;
          this.dadosPacto.value.dataFim = this.form.get('dataFim').value;
          this.dadosPacto.next(this.dadosPacto.value);
          this.modalService.dismissAll();
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

  getDataInicio() {
    if (this.form.get('dataInicio').value)
      return new Date(this.form.get('dataInicio').value);
    return new Date();
  }

  alterarDataInicio() {
    const dataInicio = new Date(this.getDataInicio());
    this.minDataConclusao = this.formatarData(dataInicio);

    if (this.form.get('dataFim').value) {
      const dataFim = new Date(this.form.get('dataFim').value);
      if (dataFim < dataInicio)
        this.form.get('dataFim').setValue(null);
    }
  }

  formatarData(data: Date) {
    return {
      'year': data.getFullYear(),
      'month': data.getMonth() + 1,
      'day': data.getDate()
    };
  }

  fecharModal() {
    this.modalService.dismissAll();
  }
}
