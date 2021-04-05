import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { IPactoTrabalho, IPactoTrabalhoSolicitacao, IPactoTrabalhoAtividade } from '../../../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../../../services/pacto-trabalho.service';
import { PerfilEnum } from '../../../../../enums/perfil.enum';
import { ApplicationStateService } from '../../../../../../../shared/services/application.state.service';
import { IUsuario } from '../../../../../../../shared/models/perfil-usuario.model';

@Component({
  selector: 'pacto-detalhes-solicitacao',
  templateUrl: './detalhes-solicitacao.component.html',
})
export class PactoDetalhesSolicitacaoComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;
  unidade = new BehaviorSubject<number>(null);
  servidor = new BehaviorSubject<number>(null);

  @Input() dadosSolicitacao: BehaviorSubject<IPactoTrabalhoSolicitacao>;

  solicitacao: IPactoTrabalhoAtividade;
  solicitadoPorOutraPessoa: boolean;

  perfilUsuario: IUsuario;

  form: FormGroup;

  constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private pactoTrabalhoDataService: PactoTrabalhoDataService,
    private applicationStateService: ApplicationStateService) { }

  ngOnInit() {  

    this.form = this.formBuilder.group({      
      descricao: [null, []],
      ajustarPrazo: [false, []]
    });

    this.applicationStateService.perfilUsuario.subscribe(p => { this.perfilUsuario = p });
    this.dadosPacto.subscribe(val => {
      this.unidade.next(val.unidadeId);
      this.servidor.next(val.pessoaId);
    });
    this.dadosSolicitacao.subscribe(val => this.carregarDadosSolicitacao());
  }

  carregarDadosSolicitacao() {
    if (this.dadosSolicitacao.value) {

      this.solicitacao = JSON.parse(this.dadosSolicitacao.value.dadosSolicitacao);

      if (this.dadosSolicitacao.value.tipoSolicitacaoId === 603 || this.dadosSolicitacao.value.tipoSolicitacaoId === 604) {
        const descricao = this.solicitacao.justificativa;
        this.solicitacao = this.dadosPacto.value.atividades.filter(a => a.pactoTrabalhoAtividadeId === this.solicitacao.pactoTrabalhoAtividadeId)[0];
        this.solicitacao.descricao = descricao;

      }

      this.solicitadoPorOutraPessoa = this.dadosSolicitacao.value.solicitanteId !== this.perfilUsuario.pessoaId;
      
    }
  }

  rejeitar() {
    this.responder(false);
  }

  aprovar() {
    this.responder(true);
  }

  responder(resposta: boolean, ajustarPrazo: boolean = false) {
    if (this.form.valid) {
      const dados: IPactoTrabalhoSolicitacao = this.form.value;
      dados.pactoTrabalhoId = this.dadosPacto.value.pactoTrabalhoId;
      dados.pactoTrabalhoSolicitacaoId = this.dadosSolicitacao.value.pactoTrabalhoSolicitacaoId;
      dados.aprovado = resposta;
      this.pactoTrabalhoDataService.ResponderSolicitacao(dados).subscribe(
        r => {
          this.dadosPacto.next(this.dadosPacto.value);
        });
    }
    else {
      this.getFormValidationErrors(this.form)
    }
  }

  getFormValidationErrors(form) {
    Object.keys(form.controls).forEach(field => {
      const control = form.get(field);
      control.markAsDirty({ onlySelf: true });
    });
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

}
