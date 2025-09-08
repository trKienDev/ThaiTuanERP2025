import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
      selector: 'mini-invoice-request-dialog',
      standalone: true,
      imports: [CommonModule ],
      templateUrl: './mini-invoice-request-dialog.component.html',
      styleUrl: './mini-invoice-request-dialog.component.scss'
})
export class MiniInvoiceRequestDialogComponent {
      constructor(
            private ref: MatDialogRef<MiniInvoiceRequestDialogComponent>
      ) {}

      close() { this.ref.close(); }
}