import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IDadosCombo } from '../../../../../shared/models/dados-combo.model';
import { UnidadeDataService } from '../../../../unidade/services/unidade.service';
import { PerfilEnum } from '../../../enums/perfil.enum';
import { IPactoTrabalho } from '../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../services/pacto-trabalho.service';
import { PlanoTrabalhoDataService } from '../../../services/plano-trabalho.service';
import { IPlanoTrabalhoPessoaModalidade, IPlanoTrabalho } from '../../../models/plano-trabalho.model';
import { ApplicationStateService } from '../../../../../shared/services/application.state.service';
import { IUsuario } from '../../../../../shared/models/perfil-usuario.model';

@Component({
  selector: 'pacto-trabalho-cadastro',
  templateUrl: './pacto-trabalho-cadastro.component.html',  
})
export class PactoTrabalhoCadastroComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  form: FormGroup;
  unidades: IDadosCombo[];  

  dadosPacto: IPactoTrabalho;

  pessoas: IDadosCombo[];
  modalidades: IPlanoTrabalhoPessoaModalidade[];
  modalidade: IPlanoTrabalhoPessoaModalidade;

  remoto: boolean;
  minDate = new Date();

  minDataInicio: any;
  maxDataInicio: any;
  minDataConclusao: any;
  maxDataConclusao: any;

  perfilUsuario: IUsuario;
  gestorUnidade: boolean;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private unidadeDataService: UnidadeDataService,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,
    private pactoTrabalhoDataService: PactoTrabalhoDataService,
    private applicationState: ApplicationStateService) {


    const nav = this.router.getCurrentNavigation();
    if (!nav || !nav.extras || !nav.extras.state) {
      this.router.navigate(['/']);
    }
    else {
      this.dadosPacto = nav.extras.state;
    }
  }

  ngOnInit() {

    this.unidadeDataService.ObterPessoas(this.dadosPacto.unidadeId).subscribe(
      appResult => {
        this.pessoas = appResult.retorno;
        this.ajustarPessoas();
      }
    );

    this.applicationState.perfilUsuario.subscribe(appResult => {
      this.perfilUsuario = appResult;
      this.gestorUnidade = this.perfilUsuario.perfis.filter(p =>
        p.perfil === PerfilEnum.Gestor ||
        p.perfil === PerfilEnum.Administrador ||
        p.perfil === PerfilEnum.Diretor ||
        p.perfil === PerfilEnum.CoordenadorGeral ||
        p.perfil === PerfilEnum.ChefeUnidade).length > 0;
      this.ajustarPessoas();
    });

    this.planoTrabalhoDataService.ObterPessoasModalidades(this.dadosPacto.planoTrabalhoId).subscribe(
      appResult => {
        this.modalidades = appResult.retorno;
      }
    );

    this.form = this.formBuilder.group({
      pessoaId: [null, [Validators.required]],
      dataInicio: [null, [Validators.required]],
      dataFim: [null, [Validators.required]],
    }); 

    this.minDataInicio = this.formatarData(new Date(this.getDataInicio()));
    this.maxDataInicio = this.formatarData(new Date(this.dadosPacto.maxDataFim));

    this.minDataConclusao = this.formatarData(new Date(this.getDataInicio()));
    this.maxDataConclusao = this.formatarData(new Date(this.dadosPacto.maxDataFim));
  }

  ajustarPessoas() {
    if (!this.gestorUnidade) {
      this.pessoas = this.pessoas.filter(p => p.id === this.perfilUsuario.pessoaId.toString());
    }
  }

  selecionarPessoa(pessoaId: number) {
    const modalidadePessoa = this.modalidades.filter(m => +m.pessoaId === +pessoaId);
    if (modalidadePessoa.length > 0) {
      this.modalidade = modalidadePessoa[0];
    }
    else {
      this.modalidade = { modalidadeExecucaoId: 101, modalidadeExecucao: 'Presencial' }
    }
  }

  getDataInicio() {
    if (this.form.get('dataInicio').value)
      return new Date(this.form.get('dataInicio').value);
    if (new Date() < new Date(this.dadosPacto.minDataInicio))
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


  cadastrar() {
    if (this.form.valid) {
      const dados: IPactoTrabalho = this.form.value;
      dados.planoTrabalhoId = this.dadosPacto.planoTrabalhoId;
      dados.pactoTrabalhoId = this.dadosPacto.pactoTrabalhoId;
      dados.unidadeId = this.dadosPacto.unidadeId;
      dados.formaExecucaoId = +this.modalidade.modalidadeExecucaoId;
      dados.termoAceite = this.modalidade.termoAceite;
      this.pactoTrabalhoDataService.Cadastrar(dados).subscribe(
        r => {
          this.router.navigateByUrl(`/programagestao/pactotrabalho/detalhar/${r.retorno}`);
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

  voltarParaPlano() {
    this.router.navigateByUrl(`/programagestao/detalhar/${this.dadosPacto.planoTrabalhoId}`);
  }

}
