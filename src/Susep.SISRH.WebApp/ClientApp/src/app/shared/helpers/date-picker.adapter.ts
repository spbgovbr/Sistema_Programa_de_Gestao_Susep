import { NativeDateAdapter } from '@angular/material';
import { MatDateFormats } from '@angular/material/core';
import { DateUtils } from './date-utils.helper';

export class AppDateAdapter extends NativeDateAdapter {

    
  format(date: Date, displayFormat: Object): string {
    if (displayFormat === 'input') {
      return DateUtils.formatar(date);
    }
    return date.toDateString();
  }

  parse(value: string): Date | null {
    return DateUtils.parse(value);
  }

}

export const APP_DATE_FORMATS: MatDateFormats = {
  parse: {
    dateInput: { month: 'short', year: 'numeric', day: 'numeric' },
  },
  display: {
    dateInput: 'input',
    monthYearLabel: { year: 'numeric', month: 'numeric' },
    dateA11yLabel: { year: 'numeric', month: 'long', day: 'numeric'
    },
    monthYearA11yLabel: { year: 'numeric', month: 'long' },
  }
};
