import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { PlanoTrabalhoSituacaoEnum } from '../../../../enums/plano-trabalho-situacao.enum';
import { IPlanoTrabalho, IPlanoTrabalhoAtividade, IPlanoTrabalhoAtividadeCandidato, IPlanoTrabalhoAtividadeItem } from '../../../../models/plano-trabalho.model';
import { PlanoTrabalhoDataService } from '../../../../services/plano-trabalho.service';
import { IDadosCombo } from '../../../../../../shared/models/dados-combo.model';
import { IItemCatalogo } from '../../../../models/item-catalogo.model';
import { UnidadeDataService } from '../../../../../unidade/services/unidade.service';
import { CatalogoDataService } from '../../../../services/catalogo.service';
import { DominioDataService } from '../../../../../../shared/services/dominio.service';
import { IDominio } from '../../../../../../shared/models/dominio.model';
import { PerfilEnum } from '../../../../enums/perfil.enum';
import { IUsuario } from '../../../../../../shared/models/perfil-usuario.model';
import { ApplicationStateService } from '../../../../../../shared/services/application.state.service';
import { PlanoTrabalhoSituacaoCandidatoEnum } from '../../../../enums/plano-trabalho-situacao-candidato.enum';
import { ItemCatalogoDataService } from '../../../../services/item-catalogo.service';

@Component({
  selector: 'plano-lista-atividade',
  templateUrl: './atividade-lista.component.html',  
})
export class PlanoListaAtividadeComponent implements OnInit {

  PerfilEnum = PerfilEnum;
  perfilUsuario: IUsuario;

  PlanoTrabalhoSituacaoEnum = PlanoTrabalhoSituacaoEnum;
  PlanoTrabalhoSituacaoCandidatoEnum = PlanoTrabalhoSituacaoCandidatoEnum;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;
  unidade = new BehaviorSubject<number>(null);
  atividades: IPlanoTrabalhoAtividade[];
  itensCatalogoDetalhar: IItemCatalogo[];

  @ViewChild('modalCadastro', { static: true }) modalCadastro;
  @ViewChild('modalCandidatura', { static: true }) modalCandidatura;
  @ViewChild('modalCandidatos', { static: true }) modalCandidatos;
  @ViewChild('modalDetalhesAtividades', { static: true }) modalDetalhesAtividades;

  form: FormGroup;

  atividadeEdicao = new BehaviorSubject<IPlanoTrabalhoAtividade>(null);
  modalidades = new BehaviorSubject<IDadosCombo[]>(null);
  itensUnidade = new BehaviorSubject<IItemCatalogo[]>(null);
  criterios = new BehaviorSubject<IDominio[]>(null);
  candidatos = new BehaviorSubject<IPlanoTrabalhoAtividadeCandidato[]>(null);

  totalColaboradores = 0;
  totalDisponivelColaboradores = 0;

  itemSelecionado: boolean;

  constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private unidadeDataService: UnidadeDataService,
    private catalogoDataService: CatalogoDataService,
    private dominioDataService: DominioDataService,
    private itemCatalogoDataService: ItemCatalogoDataService,
    private applicationState: ApplicationStateService,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,) { }

  ngOnInit() {

    this.dadosPlano.subscribe(val => this.carregarAtividades());
    this.applicationState.modalOpen.subscribe(val => {
      if (val === 'modalCandidatos') {
        this.modalService.open(this.modalCandidatos, { size: 'xl' }); 
      }
    });

    this.applicationState.perfilUsuario.subscribe(perfis => {
      this.perfilUsuario = perfis;
    });

    this.form = this.formBuilder.group({
      modalidadeExecucaoId: ['', [Validators.required]],
      quantidadeColaboradores: [null, [Validators.required]],
      descricao: ['', []],
    });
  }

  carregarAtividades() {
    if (this.dadosPlano.value) {
      this.unidade.next(this.dadosPlano.value.unidadeId);
      this.planoTrabalhoDataService.ObterAtividades(this.dadosPlano.value.planoTrabalhoId).subscribe(
        resultado => {
          this.atividades = this.dadosPlano.value.atividades = resultado.retorno;
          this.totalColaboradores = this.dadosPlano.value.atividades.reduce((a, b) => a + b.quantidadeColaboradores, 0);
          this.totalDisponivelColaboradores = this.dadosPlano.value.totalServidoresSetor - this.totalColaboradores;
          if (!this.dadosPlano.value.atividades) {
            this.dadosPlano.value.atividades = this.atividades;
            this.dadosPlano.next(this.dadosPlano.value);
          }
        }
      );

      if (this.dadosPlano.value.situacaoId) {        
        if (this.dadosPlano.value.situacaoId >= PlanoTrabalhoSituacaoEnum.Habilitacao) {
          this.planoTrabalhoDataService.ObterCandidatos(this.dadosPlano.value.planoTrabalhoId).subscribe(
            resultado => {
              this.candidatos.next(resultado.retorno);
              this.atividades = this.dadosPlano.value.atividades;
            }
          );
        }
      }

      if (this.dadosPlano.value.unidadeId) {
        if (!this.modalidades.value) {
          this.unidadeDataService.ObterModalidadesExecucao(this.dadosPlano.value.unidadeId).subscribe(
            appResult => {
              this.modalidades.next(appResult.retorno);
            }
          );
        }

        if (!this.itensUnidade.value) {
          this.catalogoDataService.ObterItensPorUnidade(this.dadosPlano.value.unidadeId).subscribe(
            appResult => {
              this.itensUnidade.next(appResult.retorno);
            }
          );
        }
      }

      if (!this.criterios.value) {
        this.dominioDataService.ObterCriterioPerfilAtividadePlano().subscribe(
          appResult => {
            this.criterios.next(appResult.retorno);
          }
        );
      }
    }
  }

  abrirTelaCadastro() {
    this.modalService.open(this.modalCadastro, { size: 'xl' });       
  }

  editar(planoTrabalhoAtividadeId: string) {
    this.atividadeEdicao.next(this.dadosPlano.value.atividades.filter(a => a.planoTrabalhoAtividadeId === planoTrabalhoAtividadeId)[0]);
    this.atividadeEdicao.subscribe(a => {
      this.abrirTelaCadastro();
    });
  }

  excluir(planoTrabalhoAtividadeId: string) {
    this.planoTrabalhoDataService.ExcluirAtividade(this.dadosPlano.value.planoTrabalhoId, planoTrabalhoAtividadeId).subscribe(
      () => {
        this.carregarAtividades();
      }
    );
  }

  candidato(planoTrabalhoAtividadeId) {
    let situacaoId = -1;
    const candidato = this.candidatos.value &&
      this.candidatos.value.filter(c => { return c.planoTrabalhoAtividadeId === planoTrabalhoAtividadeId && c.pessoaId === this.perfilUsuario.pessoaId });
    if (candidato && candidato.length > 0)
      situacaoId = candidato[0].situacaoId;
    return situacaoId;
  }

  candidatosAtividade(planoTrabalhoAtividadeId) {
    return this.candidatos.value &&
      this.candidatos.value.filter(c => { return c.planoTrabalhoAtividadeId === planoTrabalhoAtividadeId && c.situacaoId === PlanoTrabalhoSituacaoCandidatoEnum.Aprovada });
  }

  candidatar(planoTrabalhoAtividadeId: string) {
    this.atividadeEdicao.next(this.dadosPlano.value.atividades.filter(a => a.planoTrabalhoAtividadeId === planoTrabalhoAtividadeId)[0]);
    this.planoTrabalhoDataService.ObterTermoAceite(this.dadosPlano.value.planoTrabalhoId).subscribe(
      r => {
        this.dadosPlano.value.termoAceite = r.retorno.termoAceite;        
        this.modalService.open(this.modalCandidatura, { size: 'sm' });    
      });
  }

  verCandidatos() {        
    this.modalService.open(this.modalCandidatos, { size: 'xl' });
  }  

  fecharModal() {
    this.atividadeEdicao.next({});
    this.modalService.dismissAll();
  }

  abrirTelaDetalhesAtividades(itensCatalogoDetalhar: IPlanoTrabalhoAtividadeItem[]) {
    let quantidadeRetornado = 0;
    this.itensCatalogoDetalhar = [];

    itensCatalogoDetalhar.forEach(ic => {
      this.itemCatalogoDataService.ObterItem(ic.itemCatalogoId).subscribe(ret => {
        quantidadeRetornado++;

        this.itensCatalogoDetalhar.push(ret.retorno);

        if (quantidadeRetornado === itensCatalogoDetalhar.length)
          this.modalService.open(this.modalDetalhesAtividades, { size: 'xl' });
      });
    });
  }


}
