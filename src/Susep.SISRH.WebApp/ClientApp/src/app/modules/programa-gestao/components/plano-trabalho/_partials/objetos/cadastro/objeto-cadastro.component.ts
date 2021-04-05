import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject, of } from 'rxjs';
import { IPlanoTrabalho, IPlanoTrabalhoObjeto, IPlanoTrabalhoCusto, IPlanoTrabalhoObjetoAssunto, IPlanoTrabalhoReuniao } from '../../../../../models/plano-trabalho.model';
import { PerfilEnum } from '../../../../../enums/perfil.enum';
import { IObjeto } from 'src/app/modules/objeto/models/objeto.model';
import { PlanoTrabalhoDataService } from 'src/app/modules/programa-gestao/services/plano-trabalho.service';
import { map, tap } from 'rxjs/operators';
import { IDatasourceAutocompleteAsync } from 'src/app/shared/components/input-autocomplete-async/input-autocomplete-async.component';
import { ObjetoDataService } from 'src/app/modules/objeto/services/objeto.service';
import { AssuntoDataService } from 'src/app/modules/assunto/services/assunto.service';
import { Guid } from 'src/app/shared/helpers/guid.helper';
import { IAssunto } from 'src/app/modules/assunto/models/assunto.model';

@Component({
  selector: 'plano-objeto-cadastro',
  templateUrl: './objeto-cadastro.component.html',
  styleUrls: ['./objeto-cadastro.component.css']  
})
export class PlanoObjetoCadastroComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  @Input() dadosPlano: BehaviorSubject<IPlanoTrabalho>;
  @Input() objetoEdicao: BehaviorSubject<IPlanoTrabalhoObjeto>;

  formObjeto: FormGroup;
  formAssunto: FormGroup;
  formCusto: FormGroup;
  formReuniao: FormGroup;

  objetoDatasource: IDatasourceAutocompleteAsync;
  assuntoDatasource: IDatasourceAutocompleteAsync;

  abaVisivel = 'assuntos';

  private assuntosCarregados: Map<Guid, IAssunto> = new Map();

  constructor(
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,
    private objetoDataService: ObjetoDataService,
    private assuntoDataService: AssuntoDataService,
    ) { }

  ngOnInit() {

    this.formObjeto = this.formBuilder.group({
      objetoId: [null, [Validators.required]]
    });

    this.formAssunto = this.formBuilder.group({
      assuntoId: ['', [Validators.required]],
    });

    this.formCusto = this.formBuilder.group({
      valor: ['', [Validators.required]],
      descricao: ['', [Validators.required]]
    });

    this.formReuniao = this.formBuilder.group({
      titulo: ['', [Validators.required]],
      data: ['', [Validators.required]],
      descricao: ['', []]
    });

    this.objetoEdicao.subscribe(val => this.carregarDados());

    this.objetoDatasource = this.criarObjetoDatasource();
    this.assuntoDatasource = this.criarAssuntoDatasource();
  }

  private criarObjetoDatasource(): IDatasourceAutocompleteAsync {
    return {
      buscarPorChave: v => of(this.objetoEdicao.value ? this.objetoEdicao.value : null),
      buscarPorValor: (v: string) => this.objetoDataService.ObterPorTexto(v)
        .pipe(map(r => r.retorno)),
      modelToValue: (v: IObjeto) => { return { chave: v.objetoId, descricao: v.descricao }; }
    };
  }

  private criarAssuntoDatasource(): IDatasourceAutocompleteAsync {
    return {
      buscarPorChave: v => of(null),
      buscarPorValor: (v: string) => this.assuntoDataService.ObterAssuntosPorTexto(v).pipe(
        map(r => r.retorno),
        tap(a => a.forEach(a => this.assuntosCarregados.set(a.assuntoId, a))),
      ),
      modelToValue: (v: IAssunto) => { return { chave: v.assuntoId, descricao: v.hierarquia }; }
    };
  }

  get assuntosJaEscolhidos(): Guid[] {
    return this.objetoEdicao.value.assuntos.map(a => a.assuntoId);
  }

  get objetosJaEscolhidos(): Guid[] {
    return this.dadosPlano.value.objetos.map(o => o.objetoId);
  }

  fillForm() {
    
    if (this.objetoEdicao.value) {
      this.formObjeto.patchValue({
        objetoId: this.objetoEdicao.value.objetoId,
      });

      if (this.objetoEdicao.value.planoTrabalhoObjetoId) {
        this.formObjeto.controls['objetoId'].disable();
      }
    }
    
  }

  carregarDados() {    
    this.carregarAssuntosDoObjetoEdicao();
    this.fillForm();
  }

  private carregarAssuntosDoObjetoEdicao(): void {
    if (this.objetoEdicao.value) {
      this.objetoEdicao.value.assuntos.forEach(a => {
        const assunto: IAssunto = {
          assuntoId: a.assuntoId,
          hierarquia: a.hierarquia,
          //filhos: [],
          nivel: null
        }
        this.assuntosCarregados.set(a.assuntoId, assunto);
        this.assuntosJaEscolhidos.push(a.hierarquia);
      });
    }
  }

  cadastrarObjeto() {
    if (this.botaoSalvarHabilitado()) {
      const dados: IPlanoTrabalhoObjeto = this.objetoEdicao.value;
      dados.assuntos = dados.assuntos && dados.assuntos.length ? dados.assuntos : null;
      dados.custos = dados.custos && dados.custos.length ? dados.custos : null;
      dados.reunioes = dados.reunioes && dados.reunioes.length ? dados.reunioes : null;
      dados.planoTrabalhoId = this.dadosPlano.value.planoTrabalhoId;
      if (this.objetoEdicao.value && this.objetoEdicao.value.planoTrabalhoObjetoId) {
        dados.planoTrabalhoObjetoId = this.objetoEdicao.value.planoTrabalhoObjetoId;
        dados.objetoId = this.objetoEdicao.value.planoTrabalhoObjetoId;
        this.planoTrabalhoDataService.AlterarObjeto(dados).subscribe(
          r => {
            this.dadosPlano.next(this.dadosPlano.value);
          });
      }
      else {
        dados.objetoId = this.formObjeto.value.objetoId;
        this.planoTrabalhoDataService.CadastrarObjeto(dados)
        .subscribe(
          r => {
            this.dadosPlano.next(this.dadosPlano.value);
          });
      }
    }
    else {
      this.getFormValidationErrors(this.formObjeto)
    }
  }

  getFormValidationErrors(form) {
    Object.keys(form.controls).forEach(field => {
      const control = form.get(field);
      control.markAsDirty({ onlySelf: true });
    });
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

  adicionarAssunto() {
    if (this.formAssunto.valid) {
      const assuntoId = this.formAssunto.value.assuntoId;
      if (assuntoId) {
        this.objetoEdicao.value.assuntos.push({
          assuntoId: assuntoId,
          planoTrabalhoObjetoId: this.objetoEdicao ? this.objetoEdicao.value.planoTrabalhoObjetoId : null,
          hierarquia: this.assuntosCarregados.get(assuntoId).hierarquia,           
        });
        this.formAssunto.patchValue({ assuntoId: '' });
      }
    }
  }

  adicionarCusto() {
    if (this.formCusto.valid) {
      const custo: IPlanoTrabalhoCusto = this.formCusto.value;
      custo.planoTrabalhoId = this.dadosPlano.value.planoTrabalhoId;
      this.objetoEdicao.value.custos.push(custo);
      this.formCusto.patchValue({ 
        valor: '',
        descricao: ''  
      });
    }
  }

  adicionarReuniao() {
    if (this.formReuniao.valid) {
      const reuniao: IPlanoTrabalhoReuniao = this.formReuniao.value;
      reuniao.planoTrabalhoId = this.dadosPlano.value.planoTrabalhoId;
      this.objetoEdicao.value.reunioes.push(reuniao);
      this.formReuniao.patchValue({
        titulo: '',
        data: '',
        descricao: ''  
      });
    }
  }

  excluirAssunto(assunto: IPlanoTrabalhoObjetoAssunto) {
    const idx = this.objetoEdicao.value.assuntos.findIndex(a => a.assuntoId === assunto.assuntoId);
    if (idx >= 0) {
      this.objetoEdicao.value.assuntos.splice(idx, 1);
    }
  }

  excluirCusto(custo: IPlanoTrabalhoCusto) {
    const idx = this.objetoEdicao.value.custos.findIndex(a => a.planoTrabalhoCustoId === custo.planoTrabalhoCustoId);
    if (idx >= 0) {
      this.objetoEdicao.value.custos.splice(idx, 1);
    }
  }

  excluirReuniao(reuniao: IPlanoTrabalhoReuniao) {
    const idx = this.objetoEdicao.value.reunioes.findIndex(a => a.planoTrabalhoReuniaoId === reuniao.planoTrabalhoReuniaoId);
    if (idx >= 0) {
      this.objetoEdicao.value.reunioes.splice(idx, 1);
    }
  }

  alterarAba(aba: string) {
    this.abaVisivel = aba;  
  }

  botaoSalvarHabilitado(): boolean {
    return this.formObjeto.valid || this.formObjeto.controls['objetoId'].disabled;
  }
}
