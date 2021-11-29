import { Component, OnInit, ViewChild } from '@angular/core';
import { FullCalendarComponent } from '@fullcalendar/angular';
import bootstrapPlugin from '@fullcalendar/bootstrap';
import dayGridPlugin from '@fullcalendar/daygrid';
import listPlugin from '@fullcalendar/list';
import timeGridPlugin from '@fullcalendar/timegrid';
import timeGrigPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction'; // for dateClick
import { EventInput } from '@fullcalendar/core';


@Component({
  selector: 'agendamento-presencial',
  templateUrl: './agendamento-presencial.component.html',
  styleUrls: ['./agendamento-presencial.component.css'],
})
export class AgendamentoPresencialComponent implements OnInit {

  ids = 5;
  today = new Date(Date.UTC(new Date().getFullYear(), new Date().getMonth(), new Date().getDate()));

  @ViewChild('calendar', { static: true }) calendarComponent: FullCalendarComponent;

  calendarPlugins = [dayGridPlugin, timeGrigPlugin, interactionPlugin];
  calendarEvents: EventInput[] = [
    { id: 1, title: 'Event Now', start: new Date() }
  ];


  ngOnInit() {

    this.loadFullCalendar();

  }

  loadFullCalendar() {
    this.calendarComponent.locale = 'pt-br';
    ////this.calendarComponent.events = this.events;
    this.calendarComponent.allDaySlot = true;
    this.calendarComponent.displayEventTime = false;
    this.calendarComponent.themeSystem = 'bootstrap4';
    this.calendarComponent.height = 'auto';
    this.calendarComponent.header = {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,dayGridWeek,dayGridDay'
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
    if (calDate.date < this.today || calDate.date.getDay() === 0 || calDate.date.getDay() === 6) {
      return false;
    }

    this.ids++;

    const event = {
      id: this.ids, // You must use a custom id generator
      title: 'Agendado',
      start: calDate.date,
      allDay: true // If there's no end date, the event will be all day of start date
    }

    const api = this.calendarComponent.getApi();
    api.addEvent(event);
  }

  eventClicked(eventClicked) {
    const api = this.calendarComponent.getApi();
    api.getEventById(eventClicked.event.id).remove();
  }

}


