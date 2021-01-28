import { Component, OnInit } from '@angular/core';
import { IDadosPaginados } from '../../../../shared/models/pagination.model';
import { IPlanoTrabalho } from '../../models/plano-trabalho.model';
import { Router } from '@angular/router';
import { PlanoTrabalhoDataService } from '../../services/plano-trabalho.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IPlanoTrabalhoPesquisa } from '../../models/plano-trabalho.pesquisa.model';
import { IDominio } from '../../../../shared/models/dominio.model';
import { IDadosCombo } from '../../../../shared/models/dados-combo.model';
import { BehaviorSubject } from 'rxjs';
import { DominioDataService } from '../../../../shared/services/dominio.service';
import { UnidadeDataService } from '../../../unidade/services/unidade.service';
import { PerfilEnum } from '../../enums/perfil.enum';

@Component({
  selector: 'plano-trabalho-pesquisa',
  templateUrl: './plano-trabalho-pesquisa.component.html',  
})
export class PlanoTrabalhoPesquisaComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  form: FormGroup;
  dadosUltimaPesquisa: IPlanoTrabalhoPesquisa = {};
  situacoes: IDominio[];
  unidades: IDadosCombo[];
  dadosEncontrados: IDadosPaginados<IPlanoTrabalho>;

  paginacao = new BehaviorSubject<IDadosPaginados<IPlanoTrabalho>>(null);

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,
    private unidadeDataService: UnidadeDataService,
    private dominioDataService: DominioDataService
  ) { }

  ngOnInit() {
    this.dominioDataService.ObterSituacaoPlanoTrabalho(false).subscribe(
      appResult => {
        this.situacoes = appResult.retorno;
      }
    );

    this.unidadeDataService.ObterComPlanotrabalhoDadosCombo(false).subscribe(
      appResult => {
        this.unidades = appResult.retorno;
      }
    );

    this.form = this.formBuilder.group({
      unidadeId: [null, []],
      situacaoId: [null, []],
      dataInicio: [null, [Validators.required]],
      dataFim: [null, [Validators.required]],
    });

    this.pesquisar(1);
  }

  pesquisar(pagina: number) {

    this.dadosUltimaPesquisa = this.form.value;
    this.dadosUltimaPesquisa.page = pagina;

    this.planoTrabalhoDataService.ObterPagina(this.dadosUltimaPesquisa)
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
