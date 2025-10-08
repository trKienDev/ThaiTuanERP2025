import { CommonModule } from "@angular/common";
import { Component, Inject, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { InvoiceDto } from "../../../models/invoice.model";
import { InvoiceStatusPipe } from "../../../pipes/invoice-status.pipe";

@Component({
      selector: 'my-invoices-dialog',
      standalone: true,
      imports: [CommonModule, InvoiceStatusPipe],
      templateUrl: './invoice-detail-dialog.component.html',
      styleUrls: ['./invoice-detail-dialog.component.scss']
})
export class InvoiceDetailDialogComponent{
      private dialogRef = inject(MatDialogRef<InvoiceDetailDialogComponent>);
      invoiceDetail!: InvoiceDto;

      constructor(@Inject(MAT_DIALOG_DATA) public data:  { invoice: InvoiceDto }) {
            this.invoiceDetail = this.data.invoice;
            console.log('invoice detail', this.invoiceDetail);
      }



      close(result?: any): void {
            this.dialogRef.close(result);
      }
}