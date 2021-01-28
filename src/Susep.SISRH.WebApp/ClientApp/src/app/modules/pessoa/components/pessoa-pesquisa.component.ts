import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IPessoa } from '../models/pessoa.model';
import { IDadosPaginados } from '../../../shared/models/pagination.model';
import { PessoaDataService } from '../services/pessoa.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { IPessoaPesquisa } from '../models/pessoa.pesquisa.model';
import { BehaviorSubject } from 'rxjs';
import { UnidadeDataService } from '../../unidade/services/unidade.service';
import { IDadosCombo } from '../../../shared/models/dados-combo.model';
import { DominioDataService } from '../../../shared/services/dominio.service';
import { IDominio } from '../../../shared/models/dominio.model';
import { PlanoTrabalhoSituacaoCandidatoEnum } from '../../programa-gestao/enums/plano-trabalho-situacao-candidato.enum';

@Component({
  selector: 'pessoa-pesquisa',
  templateUrl: './pessoa-pesquisa.component.html',  
})
export class PessoaPesquisaComponent implements OnInit {

  planoTrabalhoSituacaoCandidatoEnum = PlanoTrabalhoSituacaoCandidatoEnum;

  form: FormGroup;
  dadosEncontrados: IDadosPaginados<IPessoa>;
  dadosUltimaPesquisa: IPessoaPesquisa = {};
  paginacao = new BehaviorSubject<IDadosPaginados<IPessoa>>(null);
  unidadesAtivasCombo: IDadosCombo[];
  situacaoPlanoTrabalhoCombo: IDominio[];

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private pessoaDataService: PessoaDataService,
    private unidadeDataService: UnidadeDataService,
    private dominioDataService: DominioDataService,
  ) { }

  ngOnInit() {

    this.montarComboUnidades();

    this.configurarForm();

    this.pesquisar(1);
  }

  configurarForm() {
    this.form = this.formBuilder.group({
      nome: [''],
      unidadeId: ['', []],
      catalogoDominioId: ['', []],
    });
  }  

  montarComboUnidades() {
    this.unidadeDataService.ObterAtivasDadosCombo().subscribe(
      appResult => {
        this.unidadesAtivasCombo = appResult.retorno;
      }
    );

  }

  pesquisar(pagina: number) {

    this.dadosUltimaPesquisa = this.form.value;
    this.dadosUltimaPesquisa.page = pagina;

    this.pessoaDataService.ObterPagina(this.dadosUltimaPesquisa)
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

}
