import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { IDadosCombo } from '../../../../shared/models/dados-combo.model';
import { IDominio } from '../../../../shared/models/dominio.model';
import { IDadosPaginados } from '../../../../shared/models/pagination.model';
import { DominioDataService } from '../../../../shared/services/dominio.service';
import { UnidadeDataService } from '../../../unidade/services/unidade.service';
import { IPactoTrabalho } from '../../models/pacto-trabalho.model';
import { IPactoTrabalhoPesquisa } from '../../models/pacto-trabalho.pesquisa.model';
import { PactoTrabalhoDataService } from '../../services/pacto-trabalho.service';
import { PerfilEnum } from '../../enums/perfil.enum';
import { PessoaDataService } from '../../../pessoa/services/pessoa.service';
import { ApplicationStateService } from '../../../../shared/services/application.state.service';
import { IUsuario } from '../../../../shared/models/perfil-usuario.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'pacto-trabalho-pesquisa',
  templateUrl: './pacto-trabalho-pesquisa.component.html',  
})
export class PactoTrabalhoPesquisaComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  @ViewChild('modalConfirmacaoExclusao', { static: true }) modalConfirmacaoExclusao;

  form: FormGroup;
  dadosUltimaPesquisa: IPactoTrabalhoPesquisa = {};
  situacoes: IDominio[];
  unidades: IDadosCombo[];
  pessoas: IDadosCombo[];
  formasExecucao: IDominio[];
  dadosEncontrados: IDadosPaginados<IPactoTrabalho>;
  pactoExcluir: IPactoTrabalho;

  perfilUsuario: IUsuario;
  gestorUnidade: boolean;

  paginacao = new BehaviorSubject<IDadosPaginados<IPactoTrabalho>>(null);

  constructor(
    public router: Router,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private pactoTrabalhoDataService: PactoTrabalhoDataService,
    private unidadeDataService: UnidadeDataService,
    private pessoaDataService: PessoaDataService,
    private dominioDataService: DominioDataService,
    private applicationState: ApplicationStateService
  ) { }

  ngOnInit() {

    this.unidadeDataService.ObterComPlanotrabalhoDadosCombo(false).subscribe(
      appResult => {
        this.unidades = appResult.retorno;
      }
    );

    this.pessoaDataService.ObterComPactoTrabalhoDadosCombo(false).subscribe(
      appResult => {
        this.pessoas = appResult.retorno;
      }
    );

    this.dominioDataService.ObterModalidadesExecucao(false).subscribe(
      appResult => {
        this.formasExecucao = appResult.retorno;
      }
    );

    this.dominioDataService.ObterSituacaoPactoTrabalho(false).subscribe(
      appResult => {
        this.situacoes = appResult.retorno;
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
    });

    this.form = this.formBuilder.group({
      unidadeId: [null, []],
      pessoaId: [null, []],
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

    if (!this.gestorUnidade)
      this.dadosUltimaPesquisa.pessoaId = this.perfilUsuario.pessoaId;

    this.pactoTrabalhoDataService.ObterPagina(this.dadosUltimaPesquisa)
      .subscribe(
        resultado => {
          this.dadosEncontrados = resultado.retorno;
          this.paginacao.next(this.dadosEncontrados);
        }
      );
  }

  onSubmit() {
    this.pesquisar(1);
  }

  cadastrarPactoTrabalho(dadosPactoCopiar: IPactoTrabalho) {    
    const dadosPacto: IPactoTrabalho = {}
    dadosPacto.planoTrabalhoId = dadosPactoCopiar.planoTrabalhoId;
    dadosPacto.unidadeId = dadosPactoCopiar.unidadeId;
    dadosPacto.unidade = dadosPactoCopiar.unidade;

    if (dadosPactoCopiar)
      dadosPacto.pactoTrabalhoId = dadosPactoCopiar.pactoTrabalhoId;

    this.router.navigate(['/programagestao/pactotrabalho/cadastro'],
      {
        state: dadosPacto
      });
  }

  excluirPactoTrabalho(pactoTrabalhoId: string) {
    this.pactoExcluir = this.dadosEncontrados.registros.filter(p => p.pactoTrabalhoId === pactoTrabalhoId)[0];
    this.modalService.open(this.modalConfirmacaoExclusao, { size: 'sm' });
  }

  confirmarExclusaoPlano() {
    this.pactoTrabalhoDataService.ExcluirPacto(this.pactoExcluir.pactoTrabalhoId)
      .subscribe(r => { this.dadosEncontrados.registros = this.dadosEncontrados.registros.filter(p => p.pactoTrabalhoId !== this.pactoExcluir.pactoTrabalhoId); });
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

}
