import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IDadosPaginados } from '../../../../../shared/models/pagination.model';
import { IPactoTrabalho } from '../../../models/pacto-trabalho.model';
import { PactoTrabalhoDataService } from '../../../services/pacto-trabalho.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IDominio } from '../../../../../shared/models/dominio.model';
import { IDadosCombo } from '../../../../../shared/models/dados-combo.model';
import { BehaviorSubject } from 'rxjs';
import { UnidadeDataService } from '../../../../unidade/services/unidade.service';
import { DominioDataService } from '../../../../../shared/services/dominio.service';
import { IPactoTrabalhoPesquisa } from '../../../models/pacto-trabalho.pesquisa.model';
import { TokenHelper } from '../../../../../shared/helpers/token.helper';
import { ApplicationStateService } from '../../../../../shared/services/application.state.service';
import { IUsuario } from '../../../../../shared/models/perfil-usuario.model';
import { PlanoTrabalhoDataService } from '../../../services/plano-trabalho.service';
import { IPlanoTrabalho } from '../../../models/plano-trabalho.model';

@Component({
  selector: 'atividades-servidor-historico',
  templateUrl: './atividades-servidor-historico.component.html',  
})
export class AtividadesServidorHistoricoComponent implements OnInit {

  form: FormGroup;
  dadosUltimaPesquisa: IPactoTrabalhoPesquisa = {};
  situacoes: IDominio[];
  unidades: IDadosCombo[];
  formasExecucao: IDominio[];
  dadosEncontrados: IDadosPaginados<IPactoTrabalho>;
  planoAtual: IPlanoTrabalho;

  perfil: IUsuario;

  paginacao = new BehaviorSubject<IDadosPaginados<IPactoTrabalho>>(null);

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private pactoTrabalhoDataService: PactoTrabalhoDataService,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,
    private unidadeDataService: UnidadeDataService,
    private dominioDataService: DominioDataService,
    private applicationState: ApplicationStateService
  ) { }

  ngOnInit() {

    this.unidadeDataService.ObterAtivasDadosCombo().subscribe(
      appResult => {
        this.unidades = appResult.retorno;
      }
    );

    this.dominioDataService.ObterModalidadesExecucao().subscribe(
      appResult => {
        this.formasExecucao = appResult.retorno;
      }
    );

    this.dominioDataService.ObterSituacaoPactoTrabalho().subscribe(
      appResult => {
        this.situacoes = appResult.retorno;
      }
    );

    this.applicationState.perfilUsuario.subscribe(appResult => {
      this.perfil = appResult;
    });

    this.planoTrabalhoDataService.ObterAtual().subscribe(
      appResult => {
        this.planoAtual = appResult.retorno;
      }
    );

    this.form = this.formBuilder.group({
      unidadeId: [null, []],
      situacaoId: [null, []],
      formaExecucaoId: [null, []],
      dataInicio: [null, [Validators.required]],
      dataFim: [null, [Validators.required]],
    });

    this.pesquisar(1);
  }

  pesquisar(pagina: number) {

    this.dadosUltimaPesquisa = this.form.value;
    this.dadosUltimaPesquisa.page = pagina;
    this.dadosUltimaPesquisa.pessoaId = this.perfil.pessoaId;

    this.pactoTrabalhoDataService.ObterPagina(this.dadosUltimaPesquisa)
      .subscribe(
        resultado => {
          this.dadosEncontrados = resultado.retorno;
          this.paginacao.next(this.dadosEncontrados);
        }
      );
  }

  proporPactoTrabalho() {
    const dadosPacto: IPactoTrabalho = {}
    dadosPacto.planoTrabalhoId = this.planoAtual.planoTrabalhoId;
    dadosPacto.unidadeId = this.planoAtual.unidadeId;
    dadosPacto.unidade = this.planoAtual.unidade;
    dadosPacto.pessoaId = this.perfil.pessoaId;

    this.router.navigate(['/programagestao/pactotrabalho/cadastro'],
      {
        state: dadosPacto
      });
  }

  onSubmit() {
    this.pesquisar(1);
  }

}
