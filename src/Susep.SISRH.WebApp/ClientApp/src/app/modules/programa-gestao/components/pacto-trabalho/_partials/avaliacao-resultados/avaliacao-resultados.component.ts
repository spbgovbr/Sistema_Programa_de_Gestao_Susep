import { Component, Input, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IPactoTrabalho } from '../../../../models/pacto-trabalho.model';
import { PerfilEnum } from '../../../../enums/perfil.enum';


@Component({
  selector: 'pacto-avaliacao-resultados',
  templateUrl: './avaliacao-resultados.component.html',  
})
export class PactoAvaliacaoResultadoComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;
  unidade = new BehaviorSubject<number>(null);
  servidor = new BehaviorSubject<number>(null);

  constructor() { }

  ngOnInit() {
    this.dadosPacto.subscribe(val => {
      this.unidade.next(this.dadosPacto.value.unidadeId);
      this.servidor.next(this.dadosPacto.value.pessoaId);
    });
  }


}
