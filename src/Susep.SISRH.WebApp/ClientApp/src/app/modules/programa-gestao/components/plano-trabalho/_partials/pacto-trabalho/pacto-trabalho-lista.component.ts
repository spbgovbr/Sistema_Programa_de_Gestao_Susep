import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { IPlanoTrabalhoMeta, IPlanoTrabalho } from '../../../../models/plano-trabalho.model';
import { BehaviorSubject } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PlanoTrabalhoDataService } from '../../../../services/plano-trabalho.service';
import { IPactoTrabalho } from '../../../../models/pacto-trabalho.model';
import { PlanoTrabalhoSituacaoEnum } from '../../../../enums/plano-trabalho-situacao.enum';
import { PerfilEnum } from '../../../../enums/perfil.enum';
import { PactoTrabalhoDataService } from '../../../../services/pacto-trabalho.service';

@Component({
  selector: 'plano-lista-pacto-trabalho',
  templateUrl: './pacto-trabalho-lista.component.html',  
})
export class PlanoListaPactoTrabalhoComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  PlanoTrabalhoSituacaoEnum = PlanoTrabalhoSituacaoEnum;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;
  unidade = new BehaviorSubject<number>(null);

  @ViewChild('modalCadastro', { static: true }) modalCadastro;
  @ViewChild('modalConfirmacaoExclusao', { static: true }) modalConfirmacaoExclusao;

  form: FormGroup;

  pactos: IPactoTrabalho[];
  pactoExcluir: IPactoTrabalho;

  isReadOnly: boolean;

  constructor(
    private router: Router,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,
    private pactoTrabalhoDataService: PactoTrabalhoDataService) { }

  ngOnInit() {
    this.dadosPlano.subscribe(val => this.carregarPactos());
  }

  carregarPactos() {
    this.unidade.next(this.dadosPlano.value.unidadeId);
    this.planoTrabalhoDataService.ObterPactosTrabalho(this.dadosPlano.value.planoTrabalhoId).subscribe(
      resultado => {
        this.pactos = resultado.retorno;
      }
    );
    this.isReadOnly = this.dadosPlano.value.situacaoId !== PlanoTrabalhoSituacaoEnum.Habilitacao &&
                      this.dadosPlano.value.situacaoId !== PlanoTrabalhoSituacaoEnum.ProntoParaExecucao &&
                      this.dadosPlano.value.situacaoId !== PlanoTrabalhoSituacaoEnum.EmExecucao;
  }

  cadastrarPactoTrabalho(pactoTrabalhoCopiarId: string) {
    const dadosPacto: IPactoTrabalho = {}
    dadosPacto.planoTrabalhoId = this.dadosPlano.value.planoTrabalhoId;
    dadosPacto.unidadeId = this.dadosPlano.value.unidadeId;
    dadosPacto.unidade = this.dadosPlano.value.unidade;
    dadosPacto.minDataInicio = this.dadosPlano.value.dataInicio;
    dadosPacto.maxDataFim = this.dadosPlano.value.dataFim;

    if (pactoTrabalhoCopiarId)
      dadosPacto.pactoTrabalhoId = pactoTrabalhoCopiarId;

    this.router.navigate(['/programagestao/pactotrabalho/cadastro'],
      {
        state: dadosPacto
      });
  }

  excluirPactoTrabalho(pactoTrabalhoId: string) {
    this.pactoExcluir = this.pactos.filter(p => p.pactoTrabalhoId === pactoTrabalhoId)[0];
    this.modalService.open(this.modalConfirmacaoExclusao, { size: 'sm' });
  }

  confirmarExclusaoPlano() {
    this.pactoTrabalhoDataService.ExcluirPacto(this.pactoExcluir.pactoTrabalhoId)
      .subscribe(r => { this.pactos = this.pactos.filter(p => p.pactoTrabalhoId !== this.pactoExcluir.pactoTrabalhoId); });
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

}
