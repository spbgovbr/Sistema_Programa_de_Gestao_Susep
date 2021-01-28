import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { IPactoTrabalho } from '../../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../../services/pacto-trabalho.service';

@Component({
  selector: 'pacto-historico',
  templateUrl: './historico.component.html',  
})
export class PactoHistoricoComponent implements OnInit {

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;

  form: FormGroup;

  constructor(
    private pactoTrabalhoDataService: PactoTrabalhoDataService,) { }

  ngOnInit() {

    this.dadosPacto.subscribe(val => this.carregarHistorico());
  }

  carregarHistorico() {
    this.pactoTrabalhoDataService.ObterHistorico(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.dadosPacto.value.historico = resultado.retorno;     
      }
    );
  }

}
