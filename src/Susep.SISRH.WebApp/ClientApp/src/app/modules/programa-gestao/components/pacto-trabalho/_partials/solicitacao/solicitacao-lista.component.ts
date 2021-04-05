import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { IDominio } from '../../../../../../shared/models/dominio.model';
import { DominioDataService } from '../../../../../../shared/services/dominio.service';
import { PerfilEnum } from '../../../../enums/perfil.enum';
import { IItemCatalogo } from '../../../../models/item-catalogo.model';
import { IPactoTrabalho, IPactoTrabalhoSolicitacao } from '../../../../models/pacto-trabalho.model';
import { CatalogoDataService } from '../../../../services/catalogo.service';
import { PactoTrabalhoDataService } from '../../../../services/pacto-trabalho.service';

@Component({
  selector: 'pacto-lista-solicitacao',
  templateUrl: './solicitacao-lista.component.html',
})
export class PactoListaSolicitacaoComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;
  unidade = new BehaviorSubject<number>(null);
  servidor = new BehaviorSubject<number>(null);

  solicitacaoEditar = new BehaviorSubject<IPactoTrabalhoSolicitacao>(null);
  situacoes = new BehaviorSubject<IDominio[]>(null);
  itensUnidade = new BehaviorSubject<IItemCatalogo[]>(null);
  solicitacoes: IPactoTrabalhoSolicitacao[];
  
  @ViewChild('modalCadastro', { static: true }) modalCadastro;

  isReadOnly: boolean;

  tipoSolicitacao: number;
  
  constructor(
    private router: Router,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private catalogoDataService: CatalogoDataService,
    private pactoTrabalhoDataService: PactoTrabalhoDataService,
    private dominioDataService: DominioDataService, ) { }

  ngOnInit() {

    this.dadosPacto.subscribe(val => {
      this.carregarSolicitacoes();
      this.carregarAtividades();
    });

  }

  carregarAtividades() {
    this.servidor.next(this.dadosPacto.value.pessoaId);
    this.pactoTrabalhoDataService.ObterAtividades(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.dadosPacto.value.atividades = resultado.retorno;
      }
    );
  }

  carregarSolicitacoes() {
    this.unidade.next(this.dadosPacto.value.unidadeId);
    this.servidor.next(this.dadosPacto.value.pessoaId);

    this.pactoTrabalhoDataService.ObterSolicitacoes(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.solicitacoes = resultado.retorno;
      }
    );

    this.dominioDataService.ObterSituacaoAtividadePactoTrabalho().subscribe(
      appResult => {
        this.situacoes.next(appResult.retorno);
      }
    );

    this.catalogoDataService.ObterItensPorUnidade(this.dadosPacto.value.unidadeId).subscribe(
      appResult => {
        this.itensUnidade.next(appResult.retorno);
      }
    );

    this.isReadOnly = this.dadosPacto.value.situacaoId !== 405;
    this.fecharModal();
  }

  abrirTelaCadastroAtividade() {
    this.tipoSolicitacao = 101;
    this.modalService.open(this.modalCadastro, { size: 'sm' });
  }

  abrirTelaAlteracaoPrazo() {
    this.tipoSolicitacao = 102;
    this.modalService.open(this.modalCadastro, { size: 'sm' });
  }

  abrirTelaJustificarEstouroPrazo() {
    this.tipoSolicitacao = 103;
    this.modalService.open(this.modalCadastro, { size: 'sm' });
  }

  abrirTelaExclusaoAtividade() {
    this.tipoSolicitacao = 104;
    this.modalService.open(this.modalCadastro, { size: 'sm' });
  }

  verSolicitacao(pactoTrabalhoSolicitacaoId: string) {
    this.solicitacaoEditar.next(this.solicitacoes.filter(s => s.pactoTrabalhoSolicitacaoId === pactoTrabalhoSolicitacaoId)[0]);
    this.tipoSolicitacao = 199;
    this.modalService.open(this.modalCadastro, { size: 'sm' });
  }

  fecharModal() {
    this.solicitacaoEditar.next(null);
    this.tipoSolicitacao = null;
    this.modalService.dismissAll();
  }

}
