import { CommonModule } from "@angular/common";
import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
      selector: 'expense-payment-items-table',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './expense-payment-items-table.component.html',
})
export class ExpensePaymentItemsTableComponent {
      @Input() items: any[] | null | undefined = [];
      @Input() totalAmount: number | null | undefined = 0;
      @Input() totalTax: number | null | undefined = 0;
      @Input() totalWithTax: number | null | undefined = 0;

      // xem hóa đơn
      @Input() showInvoiceButton: boolean = true;

      @Output() viewInvoice = new EventEmitter<any>();

      onViewInvoice(item: any) {
            this.viewInvoice.emit(item);
      }
}