import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { IPlanoTrabalho } from '../../../models/plano-trabalho.model';
import { PlanoTrabalhoDataService } from '../../../services/plano-trabalho.service';

@Component({
  selector: 'plano-habilitacao',
  templateUrl: './plano-habilitacao.component.html',

})
export class PlanoHabilitacaoComponent implements OnInit {  

  abaVisivel = 'atividades';
  dadosPlano = new BehaviorSubject<IPlanoTrabalho>(null);

  constructor(
    public router: Router,
    private planoTrabalhoDataService: PlanoTrabalhoDataService) { }

  ngOnInit() {
    this.planoTrabalhoDataService.ObterHabilitacao().subscribe(
      resultado => {
        this.dadosPlano.next(resultado.retorno);
      }
    );
  }

  alterarAba(aba: string) {
    this.abaVisivel = aba;
  }

}
