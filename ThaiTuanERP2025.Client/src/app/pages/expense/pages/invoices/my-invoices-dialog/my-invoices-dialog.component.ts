import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { InvoiceService } from "../../../services/invoice.service";
import { invoiceDto } from "../../../models/invoice.model";
import { handleHttpError } from "../../../../../core/utils/handle-http-errors.util";

@Component({
      selector: 'my-invoices-dialog',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './my-invoices-dialog.component.html',
})
export class MyInvoicesDialogComponent implements OnInit {
      private invoiceService = inject(InvoiceService);
      invoices: invoiceDto[] = [];
      item: invoiceDto[] = [];
      total: number = 0;

      ngOnInit(): void {
            this.invoiceService.getMine(1, 20).subscribe({
                  next: (page) => {
                        console.log('page: ', page);
                        this.item = page.items;
                        this.total = page.totalCount;
                  },
                  error: (err) => handleHttpError(err)
            });
      }
      
}