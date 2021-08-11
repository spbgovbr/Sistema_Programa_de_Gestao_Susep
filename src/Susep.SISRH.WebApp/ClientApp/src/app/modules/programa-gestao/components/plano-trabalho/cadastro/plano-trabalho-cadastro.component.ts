import { Component, OnInit } from '@angular/core';
import { UnidadeDataService } from '../../../../unidade/services/unidade.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IDadosCombo } from '../../../../../shared/models/dados-combo.model';
import { IPlanoTrabalho } from '../../../models/plano-trabalho.model';
import { PlanoTrabalhoDataService } from '../../../services/plano-trabalho.service';
import { Router } from '@angular/router';
import { PerfilEnum } from '../../../enums/perfil.enum';
import { DecimalValuesHelper } from '../../../../../shared/helpers/decimal-valuesr.helper';
import { ConfigurationService } from '../../../../../shared/services/configuration.service';

@Component({
  selector: 'plano-trabalho-cadastro',
  templateUrl: './plano-trabalho-cadastro.component.html',  
})
export class PlanoTrabalhoCadastroComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  form: FormGroup;
  unidades: IDadosCombo[];
  minDate = new Date();

  dadosPlano: IPlanoTrabalho = {};

  tempoComparecimentoPadrao: string;
  termosUsoPadrao: string;

  public tempoMask: any;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private configurationService: ConfigurationService,
    private decimalValuesHelper: DecimalValuesHelper,
    private unidadeDataService: UnidadeDataService,
    private planoTrabalhoDataService: PlanoTrabalhoDataService) {
  }

  ngOnInit() {
    this.tempoMask = this.decimalValuesHelper.numberMask(3, 0);

    this.unidadeDataService.ObterComCatalogoCadastrado().subscribe(
      appResult => {
        this.unidades = appResult.retorno;
      }
    );

    this.tempoComparecimentoPadrao = this.configurationService.getTempoComparecimento();
    this.termosUsoPadrao = this.configurationService.getTermosUso();

    this.form = this.formBuilder.group({
      unidadeId: ['', [Validators.required]],
      dataInicio: [new Date(), [Validators.required]],
      dataFim: [new Date(), [Validators.required]],
      tempoComparecimento: [this.tempoComparecimentoPadrao, [Validators.required]],
      tempoFaseHabilitacao: [null, [Validators.required]],    
      termoAceite: [this.termosUsoPadrao, [Validators.required, Validators.maxLength(8000)]]  
    });

    if (this.tempoComparecimentoPadrao)
      this.form.get('tempoComparecimento').disable();

    if (this.termosUsoPadrao)
      this.form.get('termoAceite').disable();
  }

  cadastrar() {
    if (this.form.valid) {
      const dados: IPlanoTrabalho = this.form.value;

      if (this.tempoComparecimentoPadrao)
        dados.tempoComparecimento = parseInt(this.tempoComparecimentoPadrao);

      if (this.termosUsoPadrao)
        dados.termoAceite = this.termosUsoPadrao;

      this.planoTrabalhoDataService.Cadastrar(dados).subscribe(
        r => {
          this.router.navigateByUrl(`/programagestao/detalhar/${r.retorno}`);
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

}
