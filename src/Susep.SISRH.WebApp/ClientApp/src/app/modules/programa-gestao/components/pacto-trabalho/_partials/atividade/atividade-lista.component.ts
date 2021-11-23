import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { PlanoTrabalhoDataService } from 'src/app/modules/programa-gestao/services/plano-trabalho.service';
import { TipoModo } from 'src/app/shared/models/configuration.model';
import { ConfigurationService } from 'src/app/shared/services/configuration.service';
import { DecimalValuesHelper } from '../../../../../../shared/helpers/decimal-valuesr.helper';
import { PerfilEnum } from '../../../../enums/perfil.enum';
import { IItemCatalogo } from '../../../../models/item-catalogo.model';
import { IPactoTrabalho, IPactoTrabalhoAtividade, IPactoTrabalhoObjeto } from '../../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../../services/pacto-trabalho.service';

@Component({
  selector: 'pacto-lista-atividade',
  templateUrl: './atividade-lista.component.html',
})
export class PactoListaAtividadeComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;
  unidade = new BehaviorSubject<number>(null);

  @Input() atividades: BehaviorSubject<IPactoTrabalhoAtividade[]>;
  @Input() itensCatalogo: BehaviorSubject<IItemCatalogo[]>;

  @ViewChild('modalCadastro', { static: true }) modalCadastro;

  form: FormGroup;

  atividadeEdicao: IPactoTrabalhoAtividade = { quantidade: 1 };

  isReadOnly: boolean;

  itemSelecionado: IItemCatalogo;
  tempoTotalAtividade: number;

  tempoPrevistoTotal = 0;
  saldoHoras = 0;

  permiteMaisDeUma: boolean;
  posDefinido: boolean;
  teletrabalhoParcial: boolean;

  objetosDoPlanoTrabalho: IPactoTrabalhoObjeto[];

  public tempoMask: any;

  abaVisivel = 'objetos';
  modo: TipoModo;

  constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private decimalValuesHelper: DecimalValuesHelper,
    private pactoTrabalhoDataService: PactoTrabalhoDataService, 
    private planoTrabalhoDataService: PlanoTrabalhoDataService,
    private configurationService: ConfigurationService) { }

  ngOnInit() {

    this.modo = this.configurationService.getModo();

    this.tempoMask = this.decimalValuesHelper.numberMask(3, 1);

    this.dadosPacto.subscribe(val => this.carregarDados());

    this.form = this.formBuilder.group({
      itemCatalogoId: [null, [Validators.required]],
      execucaoRemota: [null],
      quantidade: [1, [Validators.required]],
      tempoPrevistoPorItem: [null, []],
      descricao: [null, []],
    });
  }

  private carregarDados() {
    this.carregarAtividades();
    this.carregarObjetos();

    this.teletrabalhoParcial = this.dadosPacto.value.formaExecucaoId === 102;
    if (this.teletrabalhoParcial)
      this.form.get('execucaoRemota').setValidators(Validators.required);
}

  private carregarObjetos() {
    const val = this.dadosPacto.value;
    if (val && val.planoTrabalhoId) {
      const pactoTrabalhoAtividadeId = this.atividadeEdicao ? this.atividadeEdicao.pactoTrabalhoAtividadeId : null;
      this.planoTrabalhoDataService.ObterObjetosAssociadosOuNaoAAtividadeDoPacto(val.planoTrabalhoId, pactoTrabalhoAtividadeId).subscribe(r => {
        this.objetosDoPlanoTrabalho = r.retorno;
      });
    }
  }

  fillFormCadastro() {
    this.form.patchValue({
      itemCatalogoId: this.atividadeEdicao.itemCatalogoId,
      execucaoRemota: this.atividadeEdicao.execucaoRemota,
      quantidade: this.atividadeEdicao.quantidade,
      tempoPrevistoPorItem: this.atividadeEdicao.tempoPrevistoPorItem,
      descricao: this.atividadeEdicao.descricao,
    });
  }

  carregarAtividades() {
    if (this.dadosPacto.value) {
      this.unidade.next(this.dadosPacto.value.unidadeId);

      this.pactoTrabalhoDataService.ObterAtividades(this.dadosPacto.value.pactoTrabalhoId).subscribe(
        resultado => {
          this.atividades.next(resultado.retorno);
          this.tempoPrevistoTotal = this.atividades.value.reduce((a, b) => a + b.tempoPrevistoTotal, 0);
          this.saldoHoras = this.dadosPacto.value.tempoTotalDisponivel - this.tempoPrevistoTotal;
        }
      );

      this.isReadOnly = this.dadosPacto.value.situacaoId !== 401;
    }
  }  

  abrirTelaCadastro() {
    this.posDefinido = false;
    this.permiteMaisDeUma = false;
    this.fillFormCadastro();
    this.modalService.open(this.modalCadastro, { size: 'xl' });
  }

  cadastrarAtividade() {
    if (this.form.valid) {
      const dados: IPactoTrabalhoAtividade = this.form.value;
      dados.pactoTrabalhoId = this.dadosPacto.value.pactoTrabalhoId;
      const objetosSelecionados = this.objetosDoPlanoTrabalho.filter(o => o.associado).map(o => o.planoTrabalhoObjetoId);
      dados.objetosId = objetosSelecionados && objetosSelecionados.length ? objetosSelecionados : null;
      if (this.atividadeEdicao && this.atividadeEdicao.pactoTrabalhoAtividadeId) {
        dados.pactoTrabalhoAtividadeId = this.atividadeEdicao.pactoTrabalhoAtividadeId;
        this.pactoTrabalhoDataService.AlterarAtividade(dados).subscribe(
          r => {
            this.carregarDados();
            this.fecharModal();
          });
      }
      else {
        this.pactoTrabalhoDataService.CadastrarAtividade(dados).subscribe(
          r => {
            this.carregarDados();
            this.fecharModal();
          });
      }
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

  adicionar() {
    this.carregarObjetos();
    this.abrirTelaCadastro();
  }

  editar(pactoTrabalhoAtividadeId: string) {
    this.atividadeEdicao = this.atividades.value.filter(a => a.pactoTrabalhoAtividadeId === pactoTrabalhoAtividadeId)[0];
    this.carregarObjetos();
    this.abrirTelaCadastro();
    this.mudarItemCatalogo(this.atividadeEdicao.itemCatalogoId);
    this.fillFormCadastro();
  }

  excluir(pactoTrabalhoAtividadeId: string) {
    this.pactoTrabalhoDataService.ExcluirAtividade(this.dadosPacto.value.pactoTrabalhoId, pactoTrabalhoAtividadeId).subscribe(
      appResult => {
        this.carregarDados();
      }
    );
  }

  mudarItemCatalogo(value) {

    this.abaVisivel = 'objetos';

    this.itemSelecionado = this.itensCatalogo.value.filter(i => i.itemCatalogoId === value)[0];
    this.permiteMaisDeUma = this.itemSelecionado.formaCalculoTempoItemCatalogoId === 202;
    this.posDefinido = this.itemSelecionado.formaCalculoTempoItemCatalogoId === 203;    

    this.form.get('quantidade').setValue(1);
    if (this.posDefinido) {
      this.form.get('tempoPrevistoPorItem').setValidators(Validators.required);
      this.form.get('tempoPrevistoPorItem').enable();
    }
    else {
      const tempoItem = this.dadosPacto.value.formaExecucaoId === 101 ?
        this.itemSelecionado.tempoExecucaoPresencial :
        this.itemSelecionado.tempoExecucaoRemoto;
      this.form.get('tempoPrevistoPorItem').setValue(tempoItem);
      this.form.get('tempoPrevistoPorItem').clearValidators();
      this.form.get('tempoPrevistoPorItem').disable();
    }
    this.form.get('quantidade').updateValueAndValidity();
    this.form.get('tempoPrevistoPorItem').updateValueAndValidity();

    this.calcularTempoTotalAtividade();

  }

  calcularTempoTotalAtividade() {
    const quantidade = this.form.get('quantidade').value;
    const totalPorItem = this.form.get('tempoPrevistoPorItem').value;
    this.tempoTotalAtividade = (quantidade < 0 ? 0 : quantidade) * (totalPorItem < 0 ? 0 : totalPorItem);
  }


  fecharModal() {
    this.atividadeEdicao = {};
    this.modalService.dismissAll();
  }

  alterarAba(aba: string) {
    this.abaVisivel = aba;  
  }

}
