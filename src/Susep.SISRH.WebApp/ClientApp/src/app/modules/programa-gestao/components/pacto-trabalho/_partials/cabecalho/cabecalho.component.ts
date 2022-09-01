import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { IPactoTrabalho, IPactoTrabalhoAtividade } from '../../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../../services/pacto-trabalho.service';
import { PerfilEnum } from '../../../../enums/perfil.enum';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DecimalValuesHelper } from '../../../../../../shared/helpers/decimal-valuesr.helper';
import { DominioDataService } from '../../../../../../shared/services/dominio.service';
import { IDominio } from '../../../../../../shared/models/dominio.model';
import { ApplicationStateService } from '../../../../../../shared/services/application.state.service';
import { IUsuario } from '../../../../../../shared/models/perfil-usuario.model';

@Component({
  selector: 'pacto-cabecalho',
  templateUrl: './cabecalho.component.html',  
})
export class PactoCabecalhoComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  form: FormGroup;
  minDate = new Date();

  minDataInicio: any;
  minDataConclusao: any;

  perfilUsuario: IUsuario;
  declaracao: IDominio;

  public tempoMask: any;


  @ViewChild('modalDatas', { static: true }) modalDatas;
  @ViewChild('modalDeclaracao', { static: true }) modalDeclaracao;
  @ViewChild('modalFrequenciaPresencial', { static: true }) modalFrequenciaPresencial;

  @Input() dadosPacto: BehaviorSubject<IPactoTrabalho>;
  @Input() atividades: BehaviorSubject<IPactoTrabalhoAtividade[]>;
  @Input() unidade: BehaviorSubject<number>;
  @Input() servidor: BehaviorSubject<number>;

  tipoFrequenciaTeletrabalhoParcial: IDominio[];
  formTipoFrequenciaTeletrabalhoParcial: FormGroup;

  constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private dominioDataService: DominioDataService,
    private decimalValuesHelper: DecimalValuesHelper,
    private applicationState: ApplicationStateService,
    private pactoTrabalhoDataService: PactoTrabalhoDataService,) { }

  ngOnInit() {

    this.tempoMask = this.decimalValuesHelper.numberMask(2, 0);

    this.form = this.formBuilder.group({
      dataInicio: [null, [Validators.required]],
      dataFim: [null, [Validators.required]],
    });

    this.formTipoFrequenciaTeletrabalhoParcial = this.formBuilder.group({
      tipoFrequenciaTeletrabalhoParcialId: [null, [Validators.required]],
      quantidadeDiasFrequenciaTeletrabalhoParcial: [null, [Validators.required]],
    });

    this.applicationState.perfilUsuario.subscribe(appResult => {
      this.perfilUsuario = appResult;
      
    });

    this.dadosPacto.subscribe(pacto => {
      this.fillForm();

      this.verificarDeclaracoes();
    });
  }

  editarPeriodo() {
    this.modalService.open(this.modalDatas, { size: 'sm' });
  }

  fillForm() {
    this.form.patchValue({
      dataInicio: this.dadosPacto.value.dataInicio,
      dataFim: this.dadosPacto.value.dataFim,
    });

    this.formTipoFrequenciaTeletrabalhoParcial.patchValue({
      tipoFrequenciaTeletrabalhoParcialId: this.dadosPacto.value.tipoFrequenciaTeletrabalhoParcialId,
      quantidadeDiasFrequenciaTeletrabalhoParcial: this.dadosPacto.value.quantidadeDiasFrequenciaTeletrabalhoParcial,
    });
  }

  confirmarAlteracaoDatas() {
    if (this.form.valid) {
      const dados: IPactoTrabalho = this.form.value;
      dados.pactoTrabalhoId = this.dadosPacto.value.pactoTrabalhoId;
      this.pactoTrabalhoDataService.AlterarPeriodo(dados).subscribe(
        r => {
          this.pactoTrabalhoDataService.ObterPacto(dados.pactoTrabalhoId).subscribe(res => {
            this.dadosPacto.next(res.retorno);
            this.modalService.dismissAll();
          });          
          
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

  getDataInicio() {
    if (this.form.get('dataInicio').value)
      return new Date(this.form.get('dataInicio').value);
    return new Date();
  }

  alterarDataInicio() {
    const dataInicio = new Date(this.getDataInicio());
    this.minDataConclusao = this.formatarData(dataInicio);

    if (this.form.get('dataFim').value) {
      const dataFim = new Date(this.form.get('dataFim').value);
      if (dataFim < dataInicio)
        this.form.get('dataFim').setValue(null);
    }
  }

  formatarData(data: Date) {
    return {
      'year': data.getFullYear(),
      'month': data.getMonth() + 1,
      'day': data.getDate()
    };
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

  editarFrequenciaPresencial() {
    this.dominioDataService.ObterTipoFrequenciaTeletrabalhoParcial().subscribe(ret => {
      this.tipoFrequenciaTeletrabalhoParcial = ret.retorno.sort(it => it.id);
      this.modalService.open(this.modalFrequenciaPresencial, { size: 'sm' });
    });
  }

  salvarFrequenciaTeletrabalhoParcial() {
    if (this.formTipoFrequenciaTeletrabalhoParcial.valid) {
      const dados: IPactoTrabalho = this.formTipoFrequenciaTeletrabalhoParcial.value;
      dados.pactoTrabalhoId = this.dadosPacto.value.pactoTrabalhoId;
      this.pactoTrabalhoDataService.AlterarFrequenciaTeletrabalhoParcial(dados).subscribe(
        r => {
          this.pactoTrabalhoDataService.ObterPacto(dados.pactoTrabalhoId).subscribe(res => {
            this.dadosPacto.next(res.retorno);
            this.modalService.dismissAll();
          });

        });
    }
    else {
      this.getFormValidationErrors(this.form)
    }
  }

  obterDescricaoTipoFrequenciaTeletrabalhoParcial(): string {
    switch (this.dadosPacto.value.tipoFrequenciaTeletrabalhoParcialId) {
      case 10101: return ' durante todo o período do plano de trabalho'
      case 10102: return ' a cada semana';
      case 10103: return ' a cada quinzena';
      case 10104: return ' a cada mês';
    }

    return "";
  }

  verificarDeclaracoes() {
    if (this.perfilUsuario.pessoaId === this.dadosPacto.value.pessoaId &&
      this.dadosPacto.value.situacaoId === 405) {

      const pactoTrabalhoId = this.dadosPacto.value.pactoTrabalhoId;
      this.pactoTrabalhoDataService.ObterDeclaracoesNaoRealizadas(pactoTrabalhoId)
        .subscribe(res => {
          if (res.retorno.length > 0) {
            this.declaracao = res.retorno[0];

            this.applicationState.isLoading.subscribe(isLoading => {
              if (!isLoading && this.declaracao) {
                this.modalService.open(this.modalDeclaracao, {
                  size: 'sm',
                  centered: true,
                  windowClass: 'modalLoading',
                  backdrop: 'static',
                  keyboard: false
                });
              }
            });

            this.pactoTrabalhoDataService.RegistrarVisualizacaoDeclaracao(pactoTrabalhoId, this.declaracao.id)
              .subscribe(res => { });
          }
        });
    }
  }

  realizarDeclaracao(declaracaoId: number) {
    this.pactoTrabalhoDataService.RegistrarRealizacaoDeclaracao(this.dadosPacto.value.pactoTrabalhoId, declaracaoId)
      .subscribe(res => {
        this.declaracao = null;
        this.modalService.dismissAll();
      });
  }


}
