import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { IDominio } from '../../../../../../shared/models/dominio.model';
import { DominioDataService } from '../../../../../../shared/services/dominio.service';
import { PerfilEnum } from '../../../../enums/perfil.enum';
import { IItemCatalogo } from '../../../../models/item-catalogo.model';
import { IPactoTrabalho, IPactoTrabalhoInformacao, IPactoTrabalhoSolicitacao } from '../../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../../services/pacto-trabalho.service';

@Component({
  selector: 'pacto-lista-observacao',
  templateUrl: './observacao-lista.component.html',
})
export class PactoListObservacaoComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;
  informacoes: IPactoTrabalhoInformacao[];
  
  @ViewChild('modalCadastro', { static: true }) modalCadastro;

  isReadOnly: boolean;

  tipoSolicitacao: number;
  
  constructor(
    private modalService: NgbModal,
    private pactoTrabalhoDataService: PactoTrabalhoDataService,
    private dominioDataService: DominioDataService, ) { }

  ngOnInit() {

    this.dadosPacto.subscribe(val => {
      this.carregarObservacoes();
    });

  }

  carregarObservacoes() {
    this.pactoTrabalhoDataService.ObterInformacoes(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.informacoes =
        this.dadosPacto.value.informacoes = resultado.retorno;
      }
    );

    this.isReadOnly = this.dadosPacto.value.situacaoId !== 405;
    this.fecharModal();
  }

  abrirTelaCadastroObservacao() {
    this.tipoSolicitacao = 101;
    this.modalService.open(this.modalCadastro, { size: 'sm' });
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

}
