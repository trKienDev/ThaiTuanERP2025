import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { MatInputModule } from "@angular/material/input";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { provideMondayFirstDateAdapter } from "../../../../../../shared/date/provide-monday-first-date-adapter";
import { DateAdapter } from "@angular/material/core";

@Component({
      selector: 'mini-invoice-request-dialog',
      standalone: true,
      imports: [CommonModule, MatInputModule, MatDatepickerModule  ],
      templateUrl: './mini-invoice-request-dialog.component.html',
      styleUrl: './mini-invoice-request-dialog.component.scss',
      providers: [...provideMondayFirstDateAdapter() ]
})
export class MiniInvoiceRequestDialogComponent {
      constructor(
            private ref: MatDialogRef<MiniInvoiceRequestDialogComponent>,
            private adapter: DateAdapter<Date>
      ) {
            console.log('firstDayOfWeek =', this.adapter.getFirstDayOfWeek());           // kỳ vọng 1
            console.log('DoW 2025-09-01 =', this.adapter.getDayOfWeek(new Date(2025,8,1))); // kỳ vọng 1 (Mon)
            console.log('DoW 2025-10-01 =', this.adapter.getDayOfWeek(new Date(2025,9,1))); // kỳ vọng 3 (Wed)
      }

      close() { this.ref.close(); }
}