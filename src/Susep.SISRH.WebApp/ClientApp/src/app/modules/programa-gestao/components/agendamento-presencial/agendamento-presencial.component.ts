import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { EventInput } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction'; // for dateClick
import timeGrigPlugin from '@fullcalendar/timegrid';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { IDadosCombo } from '../../../../shared/models/dados-combo.model';
import { IUsuario } from '../../../../shared/models/perfil-usuario.model';
import { ApplicationStateService } from '../../../../shared/services/application.state.service';
import { PessoaDataService } from '../../../pessoa/services/pessoa.service';
import { IAgendamento } from '../../models/agendamento.model';
import { AgendamentoDataService } from '../../services/agendamento.service';


@Component({
  selector: 'agendamento-presencial',
  templateUrl: './agendamento-presencial.component.html',
})
export class AgendamentoPresencialComponent implements OnInit {

  dataAtual = new Date();

  ids = 5;
  today = new Date(Date.UTC(new Date().getFullYear(), new Date().getMonth(), new Date().getDate()));

  pessoas: IDadosCombo[];
  form: FormGroup;

  abaVisivel: string;

  @ViewChild('calendar', { static: true }) calendarComponent: FullCalendarComponent;

  calendarPlugins = [dayGridPlugin, timeGrigPlugin, interactionPlugin];
  calendarEvents: EventInput[] = [];

  dataInicio: Date;
  dataFim: Date;

  perfilUsuario: IUsuario;

  agendamentos: IAgendamento[];

  mensagemConfirmacao: string;
  tipoMensagemConfirmacao: number;
  dataAdicionarAgendamento: Date;
  agendamentoExcluir: string;

  @ViewChild('modalConfirmacaoAgendamento', { static: true }) modalConfirmacaoAgendamento;

  constructor(
    private toastr: ToastrService,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private pessoaDataService: PessoaDataService,
    private applicationState: ApplicationStateService,
    private agendamentoDataService: AgendamentoDataService
  ) { }

  ngOnInit() {
    const dataAtual = new Date();

    this.dataInicio = new Date(dataAtual.getFullYear(), dataAtual.getMonth(), 1);
    this.dataFim = new Date(dataAtual.getFullYear(), dataAtual.getMonth() + 1, 1);

    this.loadFullCalendar();

    this.applicationState.perfilUsuario.subscribe(appResult => {
      this.perfilUsuario = appResult;
    });

    this.pessoaDataService.ObterComPactoTrabalhoDadosCombo(true).subscribe(
      appResult => {
        this.pessoas = appResult.retorno;
      }
    );

    this.form = this.formBuilder.group({
      pessoaId: [null, []],
      dataInicio: [this.dataInicio, [Validators.required]],
      dataFim: [this.dataFim, [Validators.required]],
    });

  }


  onSubmit() {
    this.pesquisar();
  }


  pesquisar() {
    this.abaVisivel = 'calendario';

    this.agendamentoDataService.ObterPorFiltro(this.form.value).subscribe(res => {
      const calendarApi = this.calendarComponent.getApi();
      calendarApi.removeAllEvents();

      this.agendamentos = res.retorno;

      res.retorno.forEach(item => {

        const eventColor = (item.pessoaId === this.perfilUsuario.pessoaId ? "#cccccc" : "#f8f8f8");
        const eventBorderColor = (item.pessoaId === this.perfilUsuario.pessoaId ? "#17a2b8" : "#cccccc");

        const event = {
          id: item.agendamentoPresencialId,
          title: item.pessoa,
          start: item.dataAgendada,
          end: item.dataAgendada,
          color: eventColor,
          borderColor: eventBorderColor
        };

        calendarApi.addEvent(event);
      });

      // calendarApi.setOption()

    });
    //this.dadosUltimaPesquisa = this.form.value;
    //this.dadosUltimaPesquisa.page = pagina;

    //if (!this.gestorUnidade)
    //  this.dadosUltimaPesquisa.pessoaId = this.perfilUsuario.pessoaId;

    //this.pactoTrabalhoDataService.ObterPagina(this.dadosUltimaPesquisa)
    //  .subscribe(
    //    resultado => {
    //      this.dadosEncontrados = resultado.retorno;
    //      this.paginacao.next(this.dadosEncontrados);
    //    }
    //  );
  }

  loadFullCalendar() {
    this.calendarComponent.locale = 'pt-br';
    ////this.calendarComponent.events = this.events;
    this.calendarComponent.allDaySlot = true;
    this.calendarComponent.displayEventTime = false;
    this.calendarComponent.themeSystem = 'bootstrap4';
    this.calendarComponent.height = 'auto';
    this.calendarComponent.header = {
      left: 'prev,next',
      center: '',
      right: 'title'
    };

  }

  handleDayRender(date) {
    if (date.date < this.today &&
      date.date.getDay() !== 0 &&
      date.date.getDay() !== 6) {

      date.el.classList.add('fc-disabled');
    }

  }

  handleDateClick(calDate) {
    
    const proxDia = new Date();
    proxDia.setDate(proxDia.getDate() + 1);
    if (calDate.date < proxDia) {
      this.toastr.warning('Não é possível realizar um agendamento para data anterior ou igual à atual');
      return false;
    }

    if (calDate.date.getDay() === 0 || calDate.date.getDay() === 6) {
      this.toastr.warning('Não é possível realizar agendamentos em fins de semana');
      return false;
    }
    
    this.dataAdicionarAgendamento = calDate.date;
    this.tipoMensagemConfirmacao = 1;
    this.mensagemConfirmacao = 'Tem certeza que deseja realizar um agendamento para o dia ' + this.formatDate(calDate.date) + '?';
    this.modalService.open(this.modalConfirmacaoAgendamento, { size: 'sm' })
    
  }

  eventClicked(eventClicked) {
    
    const agendamento = this.agendamentos.filter(a => a.agendamentoPresencialId === eventClicked.event.id)[0];
    this.abrirModalAgendamento(agendamento);
        
  }

  abrirModalAgendamento(agendamento: IAgendamento) {
    
    if (agendamento.pessoaId !== this.perfilUsuario.pessoaId) {
      this.toastr.warning('Não é possível excluir um agendamento que não seja seu');
      return;
    }
    if (new Date(agendamento.dataAgendada) < this.dataAtual) {
      this.toastr.warning('Não é possível excluir um agendamento feito para data anterior à atual');
      return;
    }

    this.agendamentoExcluir = agendamento.agendamentoPresencialId;
    this.tipoMensagemConfirmacao = 2;
    this.mensagemConfirmacao = 'Tem certeza que deseja remover seu agendamento para o dia ' + this.formatDate(new Date(agendamento.dataAgendada)) + '?';
    this.modalService.open(this.modalConfirmacaoAgendamento, { size: 'sm' })
    

  }

  concluirAgendamento() {

    this.agendamentoDataService.Cadastrar(this.dataAdicionarAgendamento).subscribe(res => {
      const event = {
        id: this.newGuid(), // You must use a custom id generator
        title: this.perfilUsuario.nome,
        start: this.dataAdicionarAgendamento,
        allDay: true // If there's no end date, the event will be all day of start date
      }

      const api = this.calendarComponent.getApi();
      api.addEvent(event);
      this.modalService.dismissAll();

      this.pesquisar();
    });    
  }

  removerAgendamento() {

    this.agendamentoDataService.Excluir(this.agendamentoExcluir).subscribe(res => {
      const api = this.calendarComponent.getApi();
      api.getEventById(this.agendamentoExcluir).remove();
      this.modalService.dismissAll();

      this.pesquisar();
    });
    
  }

  alterarAba(aba: string) {
    this.abaVisivel = aba;
  }

  podeExcluirItem(item) {
    return item.pessoaId === this.perfilUsuario.pessoaId &&
      new Date(item.dataAgendada) > this.dataAtual;
  }

  fecharModal() {
    this.modalService.dismissAll();
  }

  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      const r = Math.random() * 16 | 0,
        v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  formatDate(date) {
    return [
      this.padTo2Digits(date.getDate()),
      this.padTo2Digits(date.getMonth() + 1),
      date.getFullYear(),
    ].join('/');
  }
  padTo2Digits(num) {
    return num.toString().padStart(2, '0');
  }
}


