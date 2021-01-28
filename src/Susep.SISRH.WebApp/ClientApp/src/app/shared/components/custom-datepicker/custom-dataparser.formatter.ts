import { NgbDateParserFormatter, NgbDateStruct } from "@ng-bootstrap/ng-bootstrap";

export class CustomDateParserFormatter extends NgbDateParserFormatter {

    readonly DELIMITER = '/';
  
    parse(value: string): NgbDateStruct | null {
      if (value) {
        let date = value.split(this.DELIMITER);
        return {
          day : parseInt(date[0], 10),
          month : parseInt(date[1], 10),
          year : parseInt(date[2], 10)
        };
      }
      return null;
    }
  
    format(date: NgbDateStruct | null): string {
      if (!date) return '';
      const day = date.day < 10 ? `0${ date.day }` : date.day;
      const month = date.month < 10 ? `0${ date.month }` : date.month;
      return day + this.DELIMITER + month + this.DELIMITER + date.year;
    }
  }
  