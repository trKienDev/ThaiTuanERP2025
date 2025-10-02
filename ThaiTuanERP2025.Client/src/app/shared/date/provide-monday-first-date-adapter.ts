import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from "@angular/material/core";
import { MondayFirstDateAdapter } from "./monday-first-date-adapter";

export const VI_DDMMYYYY = {
      parse:   { dateInput: 'DD/MM/YYYY' },
      display: {
            dateInput: 'dd/MM/yyyy',
            monthYearLabel: 'MM/yyyy',
            dateA11yLabel: 'dd/MM/yyyy',
            monthYearA11yLabel: 'MM/yyyy',
      },
};

export function provideMondayFirstDateAdapter() {
      return [
            { provide: DateAdapter, useClass: MondayFirstDateAdapter },
            { provide: MAT_DATE_LOCALE, useValue: 'vi-VN' },
            { provide: MAT_DATE_FORMATS, useValue: VI_DDMMYYYY },
      ];
}