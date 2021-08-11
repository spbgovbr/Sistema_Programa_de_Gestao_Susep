import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { PlanoTrabalhoSituacaoEnum } from '../../../../enums/plano-trabalho-situacao.enum';
import { IPlanoTrabalho, IPlanoTrabalhoAtividadeCandidato, IPlanoTrabalhoObjeto } from '../../../../models/plano-trabalho.model';
import { PlanoTrabalhoDataService } from '../../../../services/plano-trabalho.service';
import { IDadosCombo } from '../../../../../../shared/models/dados-combo.model';
import { IItemCatalogo } from '../../../../models/item-catalogo.model';
import { IDominio } from '../../../../../../shared/models/dominio.model';
import { PerfilEnum } from '../../../../enums/perfil.enum';
import { IUsuario } from '../../../../../../shared/models/perfil-usuario.model';
import { ApplicationStateService } from '../../../../../../shared/services/application.state.service';
import { PlanoTrabalhoSituacaoCandidatoEnum } from '../../../../enums/plano-trabalho-situacao-candidato.enum';
import { EnumHelper, IItemEnum } from 'src/app/shared/helpers/enum.helper';
import { TipoObjetoEnum } from 'src/app/modules/objeto/enums/tipo-objeto.enum';
import { IAssunto } from 'src/app/modules/assunto/models/assunto.model';
import { IObjeto } from 'src/app/modules/objeto/models/objeto.model';

@Component({
  selector: 'plano-lista-objeto',
  templateUrl: './objeto-lista.component.html',  
})
export class PlanoListaObjetoComponent implements OnInit {

  PerfilEnum = PerfilEnum;
  TipoObjetoEnum = TipoObjetoEnum;
  perfilUsuario: IUsuario;

  PlanoTrabalhoSituacaoEnum = PlanoTrabalhoSituacaoEnum;
  PlanoTrabalhoSituacaoCandidatoEnum = PlanoTrabalhoSituacaoCandidatoEnum;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;

  objetos: IPlanoTrabalhoObjeto[];
  tiposObjeto: IItemEnum[] = EnumHelper.toList(TipoObjetoEnum);

  @ViewChild('modalCadastro', { static: true }) modalCadastro;

  objetoEdicao = new BehaviorSubject<IPlanoTrabalhoObjeto>(null);
  modalidades = new BehaviorSubject<IDadosCombo[]>(null);
  itensUnidade = new BehaviorSubject<IItemCatalogo[]>(null);
  criterios = new BehaviorSubject<IDominio[]>(null);
  candidatos = new BehaviorSubject<IPlanoTrabalhoAtividadeCandidato[]>(null);
  assuntos = new BehaviorSubject<IAssunto[]>(null);
  todosOsObjetos = new BehaviorSubject<IObjeto[]>(null);
  unidade = new BehaviorSubject<number>(null);

  totalColaboradores = 0;
  totalDisponivelColaboradores = 0;

  itemSelecionado: boolean;

  isReadOnly: boolean;

  constructor(
    private modalService: NgbModal,
    private applicationState: ApplicationStateService,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,
  ) { }

  ngOnInit() {

    this.dadosPlano.subscribe(val => this.carregarObjetos());
    this.applicationState.perfilUsuario.subscribe(perfis => {
      this.perfilUsuario = perfis;
    });

  }

  carregarObjetos() {
    if (this.dadosPlano.value) {
      this.unidade.next(this.dadosPlano.value.unidadeId);
      this.planoTrabalhoDataService.ObterObjetos(this.dadosPlano.value.planoTrabalhoId).subscribe(
        resultado => {
          this.objetos = this.dadosPlano.value.objetos = resultado.retorno;
        }
      );
      this.isReadOnly = this.dadosPlano.value.situacaoId === PlanoTrabalhoSituacaoEnum.Executado ||
        this.dadosPlano.value.situacaoId === PlanoTrabalhoSituacaoEnum.Concluído;
    }
  }

  abrirTelaCadastro() {
   this.objetoEdicao.next({ objetoId: null, assuntos: [], custos: [], reunioes: [] });
   this.modalService.open(this.modalCadastro, { size: 'xl' });       
  }

  editar(planoTrabalhoObjetoId: string) {
    this.planoTrabalhoDataService.ObterObjeto(planoTrabalhoObjetoId).subscribe(result => {
      this.objetoEdicao.next(result.retorno);
      this.modalService.open(this.modalCadastro, { size: 'xl' });       
    });
  }
  
  excluir(planoTrabalhoObjetoId: string) {
    this.planoTrabalhoDataService.ExcluirObjeto(this.dadosPlano.value.planoTrabalhoId, planoTrabalhoObjetoId).subscribe(
      appResult => {
        this.carregarObjetos();
      }
    );
  }

  fecharModal() {
    this.objetoEdicao.next({});
    this.modalService.dismissAll();
  }

  descricaoTipo(tipoId: number) {
    const tipo = this.tiposObjeto.find(tipo => tipo.id === tipoId);
    return tipo ? tipo.descricao : '';
  }
}
