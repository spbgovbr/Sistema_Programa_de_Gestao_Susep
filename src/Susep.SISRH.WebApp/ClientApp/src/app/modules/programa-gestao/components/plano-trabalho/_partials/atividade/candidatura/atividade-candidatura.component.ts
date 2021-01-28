import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { IPlanoTrabalhoAtividade, IPlanoTrabalho } from '../../../../../models/plano-trabalho.model';
import { PlanoTrabalhoDataService } from '../../../../../services/plano-trabalho.service';
import { MatSlideToggleChange, MatSlideToggle } from '@angular/material';

@Component({
  selector: 'plano-atividade-candidatura',
  templateUrl: './atividade-candidatura.component.html',  
})
export class PlanoAtividadeCandidaturaComponent implements OnInit {

  @ViewChild("chaveTermos", { static: true }) chaveTermos: MatSlideToggle;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;
  @Input() atividadeEdicao: BehaviorSubject<IPlanoTrabalhoAtividade>;

  form: FormGroup;
  aceitouTermos: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,) { }

  ngOnInit() {
  }

  candidatar() {
    const dados = {
      'planoTrabalhoId': this.atividadeEdicao.value.planoTrabalhoId,
      'planoTrabalhoAtividadeId': this.atividadeEdicao.value.planoTrabalhoAtividadeId,
    };
    this.planoTrabalhoDataService.Candidatar(dados).subscribe(
      r => {
        this.dadosPlano.next(this.dadosPlano.value);
        this.fecharModal();
      });
  }

  toggleSituacao(event: MatSlideToggleChange) {
    this.aceitouTermos = event.checked;
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

}
