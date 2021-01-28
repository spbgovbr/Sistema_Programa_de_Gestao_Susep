import { Component, Input, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IPlanoTrabalho } from '../../../../models/plano-trabalho.model';
import { PerfilEnum } from '../../../../enums/perfil.enum';

@Component({
  selector: 'plano-avaliacao-resultados',
  templateUrl: './avaliacao-resultados.component.html',  
})
export class PlanoAvaliacaoResultadoComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;
  unidade = new BehaviorSubject<number>(null);

  constructor() { }

  ngOnInit() {
    this.dadosPlano.subscribe(val => {
      this.unidade.next(this.dadosPlano.value.unidadeId)
    });
  }


}
