import { CommonModule } from "@angular/common";
import { Component, Inject, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { InvoiceDto } from "../../../models/invoice.model";
import { InvoiceStatusPipe } from "../../../pipes/invoice-status.pipe";
import { EmptyPlaceholderPipe } from "../../../../../shared/pipes/empty-placeholder.pipe";
import { environment } from "../../../../../../environments/environment";
import { SafeUrlPipe } from "../../../../../shared/pipes/safe-url.pipe";

@Component({
      selector: 'my-invoices-dialog',
      standalone: true,
      imports: [CommonModule, InvoiceStatusPipe, EmptyPlaceholderPipe, SafeUrlPipe],
      templateUrl: './invoice-detail-dialog.component.html',
      styleUrls: ['./invoice-detail-dialog.component.scss']
})
export class InvoiceDetailDialogComponent{
      private dialogRef = inject(MatDialogRef<InvoiceDetailDialogComponent>);
      invoiceDetail!: InvoiceDto;
      fileUrl: string | null = null;

      constructor(@Inject(MAT_DIALOG_DATA) public data:  { invoice: InvoiceDto }) {
            this.invoiceDetail = this.data.invoice;
            console.log('invoice detail: ', this.invoiceDetail);
            if (this.invoiceDetail.invoiceFiles?.length > 0) {
                  this.fileUrl = `${environment.baseUrl}/files/public/${this.invoiceDetail.invoiceFiles[0].objectKey}`;
            }
      }



      close(): void {
            this.dialogRef.close();
      }
}