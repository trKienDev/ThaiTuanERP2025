import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { InvoiceService } from "../../../services/invoice.service";
import { InvoiceDto } from "../../../models/invoice.model";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { InvoiceStatusPipe } from "../../../pipes/invoice-status.pipe";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
      selector: 'my-invoices-dialog',
      standalone: true,
      imports: [ CommonModule, InvoiceStatusPipe ],
      templateUrl: './my-invoices-dialog.component.html',
      styleUrl: './my-invoices-dialog.component.scss',
})
export class MyInvoicesDialogComponent implements OnInit {
      private invoiceService = inject(InvoiceService);
      private ref = inject(MatDialogRef<MyInvoicesDialogComponent>);

      myInvoices: InvoiceDto[] = [];
      total: number = 0;

      ngOnInit(): void {
            this.loadMyInvoices();
      }

      loadMyInvoices(): void {
            this.invoiceService.getMine(1, 20).subscribe({
                  next: (page) => {
                        this.total = page.totalCount;
                        this.myInvoices = page.items;
                  },
                  error: (err) => handleHttpError(err)
            });
      }

      close(): void {
            this.ref.close();
      }

      selectInvoice(inv: InvoiceDto) {
            this.ref.close({
                  success: true,
                  invoiceId: inv.id,
                  invoiceNumber: inv.invoiceNumber,
                  invoiceName: inv.invoiceName
            });
      }
}