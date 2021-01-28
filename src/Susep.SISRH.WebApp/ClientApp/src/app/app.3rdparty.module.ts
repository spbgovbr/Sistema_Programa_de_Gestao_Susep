import { NgModule, LOCALE_ID } from '@angular/core';

import { NgBrazil } from 'ng-brazil'
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

import { MatButtonModule, MatCardModule, MatDatepickerModule, MatFormFieldModule, MatIconModule, MatInputModule, MatListModule, MatNativeDateModule, MatProgressBarModule, MatRadioModule, MatSelectModule, MatSidenavModule, MatStepperModule, MatToolbarModule, MatTreeModule, MatTableModule, MatPaginatorModule, MatSortModule, MatDialogModule, MatProgressSpinnerModule, MatGridListModule, MatTooltipModule } from '@angular/material';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ToastrModule } from 'ngx-toastr';
import { NgxMaskModule, IConfig } from 'ngx-mask'
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { Ng2GoogleChartsModule } from 'ng2-google-charts';
import { FullCalendarModule } from '@fullcalendar/angular';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { TextMaskModule } from 'angular2-text-mask';

export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

registerLocaleData(localePt)

@NgModule({  
  imports: [
    FullCalendarModule,
    MatSlideToggleModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatNativeDateModule,
    MatProgressBarModule,
    MatRadioModule,
    MatSelectModule,
    MatSidenavModule,
    MatStepperModule,
    MatToolbarModule,
    MatTreeModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatGridListModule,
    MatTooltipModule,
    PdfViewerModule,
    Ng2GoogleChartsModule,
    DragDropModule,
    TextMaskModule,
    ToastrModule.forRoot({
      preventDuplicates: true,
    }),
    NgxMaskModule.forRoot(options)
  ],
  exports: [
    MatSlideToggleModule,
    FullCalendarModule,
    DragDropModule,
    TextMaskModule,
    NgxMaskModule,
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt-BR' },       
  ],  
})
export class App3rdPartyModule { }
