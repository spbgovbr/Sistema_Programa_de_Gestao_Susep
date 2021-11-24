import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSlideToggleChange } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { FullCalendarComponent } from '@fullcalendar/angular';
import bootstrapPlugin from '@fullcalendar/bootstrap';
import dayGridPlugin from '@fullcalendar/daygrid';
import listPlugin from '@fullcalendar/list';
import timeGridPlugin from '@fullcalendar/timegrid';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs';
import { PerfilEnum } from '../../../enums/perfil.enum';
import { IItemCatalogo } from '../../../models/item-catalogo.model';
import { IPactoTrabalho, IPactoTrabalhoAtividade, IPactoTrabalhoDataCalendario } from '../../../models/pacto-trabalho.model';
import { CatalogoDataService } from '../../../services/catalogo.service';
import { PactoTrabalhoDataService } from '../../../services/pacto-trabalho.service';
import { ApplicationStateService } from '../../../../../shared/services/application.state.service';
import { IUsuario } from '../../../../../shared/models/perfil-usuario.model';
import { PessoaDataService } from '../../../../pessoa/services/pessoa.service';
import { DataService } from '../../../../../shared/services/data.service';
import { PlanoTrabalhoDataService } from '../../../services/plano-trabalho.service';
import { IPlanoTrabalhoObjeto } from '../../../models/plano-trabalho.model';


@Component({
  selector: 'pacto-trabalho-detalhes',
  templateUrl: './pacto-trabalho-detalhes.component.html',
  styleUrls: ['./pacto-trabalho-detalhes.component.css'],
})
export class PactoTrabalhoDetalhesComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  calendarPlugins = [bootstrapPlugin, dayGridPlugin, timeGridPlugin, listPlugin];

  @ViewChild('calendar', { static: true }) calendarComponent: FullCalendarComponent;

  @ViewChild('modalEnviarAceite', { static: true }) modalEnviarAceite;
  @ViewChild('modalAceite', { static: true }) modalAceite;
  @ViewChild('modalAceiteComTermoAceite', { static: true }) modalAceiteComTermoAceite;
  @ViewChild('modalFluxo', { static: true }) modalFluxo;
  @ViewChild('modalConcluirExecucao', { static: true }) modalConcluirExecucao;
  @ViewChild('modalReabrirExecucao', { static: true }) modalReabrirExecucao;  

  horaInicio = 0;
  horaFim = 8;
  abaVisivel = 'atividades';
  classeTextoSituacao = 'text-danger';

  dadosPacto = new BehaviorSubject<IPactoTrabalho>(null);
  unidade = new BehaviorSubject<number>(null);
  servidor = new BehaviorSubject<number>(null);

  atividades = new BehaviorSubject<IPactoTrabalhoAtividade[]>(null);  
  itensCatalogo = new BehaviorSubject<IItemCatalogo[]>(null);

  atividadesNaoAdicionadas = false;

  termoAceite: string;
  aceitouTermos: boolean;
  usuarioPodeAceitar: boolean;
  usuarioPodeReabrir: boolean;

  tempoPrevistoTotal = 0;
  saldoHoras = 0;

  perfilUsuario: IUsuario;
  gestorUnidade: boolean;

  periodoExecucao: boolean;

  feriados: Date[];

  form: FormGroup;

  events: any[] = [];

  constructor(
    public router: Router,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private dataService: DataService,
    private catalogoDataService: CatalogoDataService,
    private pactoTrabalhoDataService: PactoTrabalhoDataService,
    private planoTrabalhoDataService: PlanoTrabalhoDataService,
    private applicationState: ApplicationStateService,
    private pessoaDataService: PessoaDataService
  ) { }

  ngOnInit() {
    this.loadFullCalendar();
    this.carregarDados();

    this.applicationState.perfilUsuario.subscribe(appResult => {
      this.perfilUsuario = appResult;
      this.gestorUnidade = this.perfilUsuario.perfis.filter(p =>
        p.perfil === PerfilEnum.Gestor ||
        p.perfil === PerfilEnum.Administrador ||
        p.perfil === PerfilEnum.Diretor ||
        p.perfil === PerfilEnum.CoordenadorGeral ||
        p.perfil === PerfilEnum.ChefeUnidade).length > 0;

      this.verificarSeUsuarioPodeAceitar();
    });

    this.form = this.formBuilder.group({
      descricao: ['', [Validators.required]],
    });
  }

  fillForm(descricao: string) {
    this.form.patchValue({
      descricao: descricao,
    });
  }

  carregarDados() {
    const pactoTrabalhoId = this.activatedRoute.snapshot.paramMap.get('id');
    this.dadosPacto.next({ 'pactoTrabalhoId': pactoTrabalhoId });

    this.carregarDadosPacto();
  }

  carregarDadosPacto() {
    this.pactoTrabalhoDataService.ObterPacto(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.dadosPacto.next(resultado.retorno);
        this.unidade.next(this.dadosPacto.value.unidadeId);
        this.servidor.next(this.dadosPacto.value.pessoaId);

        this.periodoExecucao = this.dataService.formatAsDate(new Date()) >= this.dataService.formatAsDate(this.dadosPacto.value.dataInicio);
        
        this.carregarItensCatalogo();        

        this.pessoaDataService.ObterFeriados(this.dadosPacto.value.pessoaId, this.dadosPacto.value.dataInicio, this.dadosPacto.value.dataFim).subscribe(
          result => {
            this.feriados = result.retorno;
            this.carregarEventos();
          });        

        this.alterarAba('atividades');
        this.definirClasseTextoSituacao();
        this.verificarSeUsuarioPodeAceitar();
      }
    );
  }

  verificarSeUsuarioPodeAceitar() {
    if (this.perfilUsuario.pessoaId && this.dadosPacto.value.responsavelEnvioAceite) {
      //O usuário pode aceitar o plano se o plano é dele, mas não foi ele que enviou para aceite
      //                              ou se o plano não é dele, mas quem enviou para aceite foi a pessoa do plano

      this.usuarioPodeAceitar =
        (this.perfilUsuario.pessoaId === this.dadosPacto.value.pessoaId &&
         this.perfilUsuario.pessoaId.toString() !== this.dadosPacto.value.responsavelEnvioAceite.toString()) ||
        (this.perfilUsuario.pessoaId !== this.dadosPacto.value.pessoaId &&
         this.dadosPacto.value.pessoaId.toString() === this.dadosPacto.value.responsavelEnvioAceite.toString());
    }

    this.usuarioPodeReabrir = this.dadosPacto.value.pessoaId !== this.perfilUsuario.pessoaId;
  }

  carregarItensCatalogo() {
    this.catalogoDataService.ObterItensPorUnidade(this.dadosPacto.value.unidadeId).subscribe(
      appResult => {
        let itens = appResult.retorno;
        itens = itens.filter(i => this.dadosPacto.value.formaExecucaoId === 101 || i.permiteTrabalhoRemoto);
        this.itensCatalogo.next(itens);
        this.atividades.subscribe(a => this.carregarEventos());
      }
    );
  }

  alterarAba(aba: string) {
    this.abaVisivel = aba;
  }

  carregarEventos() {
    if (this.atividades.value && this.feriados) {      
      
      this.tempoPrevistoTotal = this.atividades.value.reduce((a, b) => a + b.tempoPrevistoTotal, 0);
      this.saldoHoras = this.dadosPacto.value.tempoTotalDisponivel - this.tempoPrevistoTotal;

      //Obtém as datas do pacto
      const datas = this.getWorkingDays(this.dadosPacto.value.dataInicio, this.dadosPacto.value.dataFim);

      //Verifica a hora fim do dia
      this.horaFim = +this.horaInicio + +this.dadosPacto.value.cargaHorariaDiaria;

      //Cria os eventos 
      this.criarEventosPorDia(datas);
      this.criarEventosPorTarefa(datas);      

      //Adiciona os eventos ao calendário
      const calendarApi = this.calendarComponent.getApi();
      calendarApi.removeAllEvents();
      datas.forEach(d => d.events.forEach(e => calendarApi.addEvent(e)));
    }
  }

  criarEventosPorDia(datas: IPactoTrabalhoDataCalendario[]) {
    //Obtém os eventos que se repetem todos os dias    
    const eventosPorDia = this.obterEventosPorTipo(201);

    ////Adiciona o intervalo de almoço
    //if (this.dadosPacto.value.cargaHorariaDiaria > 7) {
    //  eventosPorDia.splice(0, 0, { itemCatalogo: 'Intervalo de almoço', 'tempoPrevistoPorItem': 1, 'adicionadoCalendario' : true });
    //  this.horaFim++;
    //}

    //Prepara os eventos por dia
    eventosPorDia.forEach(e => {
      const cor = this.obterCor();
      datas.forEach(d => {
        this.criarEvento(d, e.itemCatalogo, new Date(d.date), d.ultimoHorarioOcupado, new Date(d.date), d.ultimoHorarioOcupado + e.tempoPrevistoPorItem, cor);
        e.adicionadoCalendario = true;
        d.ultimoHorarioOcupado += e.tempoPrevistoPorItem;        
      })  
    });

    this.verificarNaoAdicionados(eventosPorDia);    
  }

  criarEventosPorTarefa(datas: IPactoTrabalhoDataCalendario[]) {
    this.criarEventosPorDiaPorTipo(datas, 202);
    this.criarEventosPorDiaPorTipo(datas, 203);
  }

  criarEventosPorDiaPorTipo(datas: IPactoTrabalhoDataCalendario[], tipo: number) {
    //Prepara os eventos por tarefa pós definida
    const eventosPorTarefa = this.obterEventosPorTipo(tipo);    
    eventosPorTarefa.forEach(e => {
      let tempoAlocar = e.tempoPrevistoPorItem;

      const cor = this.obterCor();

      for (let indexData = 0; indexData < datas.length; indexData++) {
        const d = datas[indexData];

        const tempoDisponivelDia = this.horaFim - d.ultimoHorarioOcupado;
        if (tempoDisponivelDia > 0) {
          const tempoAlocado = tempoAlocar > tempoDisponivelDia ? tempoDisponivelDia : tempoAlocar;
          
          this.criarEvento(d, e.itemCatalogo, new Date(d.date), d.ultimoHorarioOcupado, new Date(d.date), d.ultimoHorarioOcupado + tempoAlocado, cor);
          e.adicionadoCalendario = true;
          d.ultimoHorarioOcupado += tempoAlocado;
          tempoAlocar -= tempoAlocado;
          if (tempoAlocar <= 0) indexData = datas.length;
        }
      }
    });
    this.verificarNaoAdicionados(eventosPorTarefa);
  }

  verificarNaoAdicionados(lista: IPactoTrabalhoAtividade[]) {
    const naoAdicionados = lista.filter(a => !a.adicionadoCalendario);
    naoAdicionados.forEach(na => {
      this.atividades.value.filter(a => a.pactoTrabalhoAtividadeId === na.pactoTrabalhoAtividadeId)
        .forEach(ana => ana.adicionadoCalendario = false)
    })
    
  }

  indexCor = 0;
  obterCor(): string {
    const cores = ['#CCD4BF', '#E7CBA9', '#EEBAB2', '#F5F3E7', '#F5E2E4', '#B8A390', '#E6D1D2', '#DAD5D6', '#B2B5B9', '#8FA2A6', '#CAE7E3', '#B2B2B2', '#EEB8C5', '#DCDBD9', '#FEC7BC', '#C2D9E1', '#D29F8C', '#D9D3D2', '#81B1CC','#FFD9CF'];

    this.indexCor++;
    if (this.indexCor > cores.length) {
      this.indexCor = 0;
    }
    return cores[this.indexCor];
  }

  criarEvento(data: IPactoTrabalhoDataCalendario, titulo: string, dataInicio: Date, horaInicio: number, dataFim: Date, horaFim: number, cor: string) {
    dataInicio.setHours(horaInicio);
    dataFim.setHours(horaFim);

    data.events.push({
      color: cor,
      title: titulo,
      start: dataInicio.toISOString(),
      end: dataFim.toISOString()
    });
  }

  obterEventosPorTipo(tipo: number) {
    const eventosTipo = this.atividades.value && this.itensCatalogo.value ? this.atividades.value.filter(a => this.itensCatalogo.value.filter(i => i.formaCalculoTempoItemCatalogoId === tipo && i.itemCatalogoId === a.itemCatalogoId).length > 0) : [];
    const eventosQuantMaiorQueUm = eventosTipo.filter(a => a.quantidade > 1);
    eventosQuantMaiorQueUm.forEach(e => {
      for (let quantidade = 1; quantidade < e.quantidade; quantidade++)
        eventosTipo.push({ pactoTrabalhoAtividadeId: e.pactoTrabalhoAtividadeId, itemCatalogo: e.itemCatalogo, 'tempoPrevistoPorItem': e.tempoPrevistoPorItem });
    });
    return eventosTipo;
  }

  getWorkingDays(startDate: any, endDate: any) {
    const days: IPactoTrabalhoDataCalendario[] = [];
    const curDate = new Date(startDate);
    
    while (curDate <= new Date(endDate)) {
      const dayOfWeek = curDate.getDay();
      const feriado = this.feriados.filter(f => curDate.getTime() === new Date(f).getTime()).length > 0;
      const fimSemana = (dayOfWeek === 6) || (dayOfWeek === 0);
      
      if (!feriado && !fimSemana)
        days.push({ date: new Date(curDate), ultimoHorarioOcupado: this.horaInicio, events: []});
      curDate.setDate(curDate.getDate() + 1);
    }
    return days;
  }

  voltarParaPlano() {
    this.router.navigateByUrl(`/programagestao/detalhar/${this.dadosPacto.value.planoTrabalhoId}`);
  }

  loadFullCalendar() {
    this.calendarComponent.locale = 'pt-br';
    this.calendarComponent.events = this.events;
    this.calendarComponent.allDaySlot = false;
    this.calendarComponent.displayEventTime = true;
    this.calendarComponent.themeSystem = 'bootstrap4';
    this.calendarComponent.height = 'auto';
    this.calendarComponent.header = {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,dayGridWeek,dayGridDay'
    };
  }

  definirClasseTextoSituacao() {
    switch (this.dadosPacto.value.situacaoId) {
      case 401: this.classeTextoSituacao = 'text-gray-5'; break;
      case 402: this.classeTextoSituacao = 'text-warning'; break;
      case 403: this.classeTextoSituacao = 'text-success'; break;
      case 404: this.classeTextoSituacao = 'text-orange'; break;
      case 405: this.classeTextoSituacao = 'text-teal'; break;
      case 406: this.classeTextoSituacao = 'text-primary-lighten-25'; break;
      case 407: this.classeTextoSituacao = 'text-primary-darken-25'; break;
    }
  }

  toggleSituacao(event: MatSlideToggleChange) {
    this.aceitouTermos = event.checked;
  }

  aceitar() {
    if (this.dadosPacto.value.pessoaId === this.perfilUsuario.pessoaId) {

      this.planoTrabalhoDataService.ObterTermoAceite(this.dadosPacto.value.planoTrabalhoId).subscribe(
        r => {
          this.termoAceite = r.retorno.termoAceite;
          this.modalService.open(this.modalAceiteComTermoAceite, { size: 'sm' });
        });
    }
    else {
      this.modalService.open(this.modalAceite, { size: 'sm' });
    }
  }

  confirmarAceite() {
    if (this.dadosPacto.value.situacaoId === 401) {
      this.pactoTrabalhoDataService.EnviarParaAceite(this.dadosPacto.value.pactoTrabalhoId).subscribe(
        resultado => {
          this.carregarDadosPacto();
        }
      );
    }
    else {
      this.pactoTrabalhoDataService.Aceitar(this.dadosPacto.value.pactoTrabalhoId).subscribe(
        resultado => {
          this.carregarDadosPacto();
        }
      );
    }
  }

  rejeitar() {
    this.fillForm('');
    this.modalService.open(this.modalFluxo, { size: 'sm' });
  }

  confirmarRejeicao() {

    if (this.form.valid) {
      this.pactoTrabalhoDataService.Rejeitar(this.dadosPacto.value.pactoTrabalhoId, this.form.get('descricao').value).subscribe(
        resultado => {
          this.carregarDadosPacto();
        }
      );

      this.fecharModal();
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

  voltarParaRascunho() {
    this.pactoTrabalhoDataService.VoltarParaRascunho(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.carregarDadosPacto();
      }
    );
  }

  iniciarExecucao() {
    this.pactoTrabalhoDataService.IniciarExecucao(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.carregarDadosPacto();
      }
    );
  }

  abrirTelaConcluirExecucao() {
    this.modalService.open(this.modalConcluirExecucao, { size: 'sm' });
  }

  abrirTelaReabrirExecucao() {
    this.modalService.open(this.modalReabrirExecucao, { size: 'sm' });
  }

  concluirExecucao() {
    this.pactoTrabalhoDataService.ConcluirExecucao(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.carregarDadosPacto();
      }
    );
  }

  concluirReabertura() {
    this.pactoTrabalhoDataService.Reabrir(this.dadosPacto.value.pactoTrabalhoId).subscribe(
      resultado => {
        this.carregarDadosPacto();
      }
    );
  }

  

  fecharModal() {
    this.modalService.dismissAll();
  }

}
