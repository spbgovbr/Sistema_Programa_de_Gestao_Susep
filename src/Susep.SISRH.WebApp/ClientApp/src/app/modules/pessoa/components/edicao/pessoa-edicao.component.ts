import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PessoaDataService } from '../../services/pessoa.service';
import { IPessoa } from '../../models/pessoa.model';
import { PlanoTrabalhoDataService } from '../../../programa-gestao/services/plano-trabalho.service';
import { IPlanoTrabalhoAtividade, IPlanoTrabalhoAtividadeCandidato } from '../../../programa-gestao/models/plano-trabalho.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PlanoTrabalhoSituacaoCandidatoEnum } from '../../../programa-gestao/enums/plano-trabalho-situacao-candidato.enum';

@Component({
  selector: 'pessoa-edicao',
  templateUrl: './pessoa-edicao.component.html',
})
export class PessoaEdicaoComponent implements OnInit {

  form: FormGroup;
  dadosPessoa: IPessoa;
  candidaturaSelecionada: IPlanoTrabalhoAtividadeCandidato;

  planoTrabalhoSituacaoCandidatoEnum = PlanoTrabalhoSituacaoCandidatoEnum;

  @ViewChild('modalResponder', { static: true }) modalResponder;
  @ViewChild('modalJustificativa', { static: true }) modalJustificativa;

  
  constructor(
    public router: Router,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private pessoaDataService: PessoaDataService,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,

    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit() {
    
    this.carregarDados();

    this.form = this.formBuilder.group({
      descricao: ['', [Validators.required]],
    });
  }

  fillForm(descricao: string) {
    this.form.patchValue({
      descricao: descricao,
    });
  }

  carregarDados() {

    const pessoaId = this.activatedRoute.snapshot.paramMap.get('id');

    if (pessoaId) {
      this.pessoaDataService.ObterPessoa(pessoaId).subscribe(
        resultado => {
          this.dadosPessoa = resultado.retorno;
        }
      );
    }

  }

  responder(candidatura: IPlanoTrabalhoAtividadeCandidato) {
    this.fillForm('');
    this.candidaturaSelecionada = candidatura;
    this.modalService.open(this.modalResponder, { size: 'sm' });
  }

  visualizarJustificativa(candidatura: IPlanoTrabalhoAtividadeCandidato) {
    this.fillForm(candidatura.descricao);
    this.modalService.open(this.modalJustificativa, { size: 'sm' });
  }

  fecharModal() {
    this.fillForm('');
    this.candidaturaSelecionada = null;
    this.modalService.dismissAll();
  }

  rejeitar() {

    if (this.form.valid) {
      this.candidaturaSelecionada.situacaoId = this.planoTrabalhoSituacaoCandidatoEnum.LegalidadeRejeitada;
      this.candidaturaSelecionada.descricao = this.form.get('descricao').value;
      this.planoTrabalhoDataService.AtualizarCandidatura(this.candidaturaSelecionada).subscribe(
          resultado => {
            this.carregarDados();
           }
          );

        this.fecharModal();
      }
  }


  aprovar() {  

    this.candidaturaSelecionada.situacaoId = this.planoTrabalhoSituacaoCandidatoEnum.LegalidadeVerificada;
    this.candidaturaSelecionada.descricao = this.form.get('descricao').value;
    this.planoTrabalhoDataService.AtualizarCandidatura(this.candidaturaSelecionada).subscribe(
        resultado => {
          this.carregarDados();
        }
      );

      this.fecharModal();
    }
}

