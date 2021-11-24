import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { IPlanoTrabalhoAtividade, IPlanoTrabalhoAtividadeCandidato, IPlanoTrabalho } from '../../../../../models/plano-trabalho.model';
import { PlanoTrabalhoDataService } from '../../../../../services/plano-trabalho.service';
import { PlanoTrabalhoSituacaoCandidatoEnum } from '../../../../../enums/plano-trabalho-situacao-candidato.enum';
import { PerfilEnum } from '../../../../../enums/perfil.enum';
import { PlanoTrabalhoSituacaoEnum } from '../../../../../enums/plano-trabalho-situacao.enum';
import { MatSlideToggleChange } from '@angular/material';

@Component({
  selector: 'plano-lista-atividade-candidato',
  templateUrl: './atividade-lista-candidato.component.html',  
})
export class PlanoListaAtividadeCandidatoComponent implements OnInit {

  PerfilEnum = PerfilEnum;
  PlanoTrabalhoSituacaoCandidatoEnum = PlanoTrabalhoSituacaoCandidatoEnum;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;
  @Input() candidatos: BehaviorSubject<IPlanoTrabalhoAtividadeCandidato[]>;

  form: FormGroup;

  constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private planoTrabalhoDataService: PlanoTrabalhoDataService) { }

  ngOnInit() {

    this.candidatos.subscribe(val => this.carregarDados());

    this.form = this.formBuilder.group({
      justificativa: [null, []],
    });
  }

  carregarDados() {
  }

  candidatosAtividade(planoTrabalhoAtividadeId) {
    return this.candidatos.value &&
      this.candidatos.value.filter(c => { return c.planoTrabalhoAtividadeId === planoTrabalhoAtividadeId });
  }

  salvar() {   
    let aprovados =
      this.candidatos.value.filter(c => c.aprovado).map(a => { return a.planoTrabalhoAtividadeCandidatoId });

    if (aprovados.length === 0)
      aprovados = ['null'];

    this.planoTrabalhoDataService.AlterarFase(this.dadosPlano.value.planoTrabalhoId, PlanoTrabalhoSituacaoEnum.EmExecucao, this.form.get('justificativa').value, aprovados).subscribe(
      resultado => {
        this.dadosPlano.value.situacaoId = PlanoTrabalhoSituacaoEnum.EmExecucao;
        this.dadosPlano.value.situacao = 'Em execução';
        this.dadosPlano.next(this.dadosPlano.value);
      }
    );
  }

  salvarDeserto() {
    this.planoTrabalhoDataService.AlterarFase(this.dadosPlano.value.planoTrabalhoId, PlanoTrabalhoSituacaoEnum.EmExecucao, '', ['null'], true).subscribe(
      resultado => {
        this.dadosPlano.value.situacaoId = PlanoTrabalhoSituacaoEnum.Concluído;
        this.dadosPlano.value.situacao = 'Concluído';
        
        this.dadosPlano.next(this.dadosPlano.value);
      }
    );
  }

  toggleSituacao(check: MatSlideToggleChange, planoTrabalhoAtividadeCandidatoId: string) {
    const candidato = this.candidatos.value.filter(c => c.planoTrabalhoAtividadeCandidatoId === planoTrabalhoAtividadeCandidatoId);
    if (check.checked) {
      candidato[0].situacaoId = PlanoTrabalhoSituacaoCandidatoEnum.Aprovada;
      candidato[0].aprovado = true;

      const outrasCandidaturasMesmoCandidato = this.candidatos.value.filter(c => c.pessoaId === candidato[0].pessoaId && c.planoTrabalhoAtividadeCandidatoId !== planoTrabalhoAtividadeCandidatoId);
      outrasCandidaturasMesmoCandidato.forEach(c => { c.aprovado = false; c.situacaoId = PlanoTrabalhoSituacaoCandidatoEnum.Rejeitada; });
    }
    else {
      candidato[0].situacaoId = PlanoTrabalhoSituacaoCandidatoEnum.Rejeitada;
      candidato[0].aprovado = false;
    }
  }

  desabilitado(planoTrabalhoAtividadeCandidatoId: string) {
    const candidato = this.candidatos.value.filter(c => c.planoTrabalhoAtividadeCandidatoId === planoTrabalhoAtividadeCandidatoId)[0];
    if (!candidato.aprovado) {
      const atividade = this.dadosPlano.value.atividades.filter(a => a.planoTrabalhoAtividadeId === candidato.planoTrabalhoAtividadeId)[0];

      const candidatosAtividade = this.candidatos.value.filter(c => c.planoTrabalhoAtividadeId === atividade.planoTrabalhoAtividadeId && c.aprovado);
      if (atividade.quantidadeColaboradores === candidatosAtividade.length) {
        return true;
      }
    }
    return false;
  }

  deserto() {
    return this.candidatos.value.length === 0;
  }

  podeSalvar() {
    if (this.candidatos.value) {
      const justificativaPreenchida = this.form.get('justificativa').value;
      return (this.candidatos.value.length > 0 && !this.temPessoaSemAtividade()) || justificativaPreenchida;
    }
    return false;
  }

  temPessoaSemAtividade() {
    const mapTodos = new Map();
    const mapAprovados = new Map();

    if (this.candidatos.value) {
      this.candidatos.value.forEach((item) => {
        const todos = mapTodos.get(item.pessoaId);
        if (!todos) mapTodos.set(item.pessoaId, true);

        if (item.aprovado) {
          const aprovado = mapAprovados.get(item.pessoaId);
          if (!aprovado) mapAprovados.set(item.pessoaId, true);
        }        
      });
      return mapTodos.size !== mapAprovados.size;
    }

    return true;
  }

  temAtividadeSemAprovados() {
    const mapTodos = new Map();
    const mapAprovados = new Map();

    if (this.candidatos.value) {
      this.candidatos.value.forEach((item) => {
        const todos = mapTodos.get(item.planoTrabalhoAtividadeId);
        if (!todos) mapTodos.set(item.planoTrabalhoAtividadeId, true);

        if (item.aprovado) {
          const aprovado = mapAprovados.get(item.planoTrabalhoAtividadeId);
          if (!aprovado) mapAprovados.set(item.planoTrabalhoAtividadeId, true);
        }
      });
      return mapTodos.size !== mapAprovados.size;
    }

    return true;
  }

  fecharModal() {
    this.modalService.dismissAll();
  }


}
