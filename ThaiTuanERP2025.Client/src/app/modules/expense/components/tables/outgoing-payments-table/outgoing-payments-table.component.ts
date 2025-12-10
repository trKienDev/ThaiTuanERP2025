import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { OutgoingPaymentBriefDto } from "../../../models/outgoing-payment.model";
import { OutgoingPaymentStatusPipe } from "../../../pipes/outgoing-payment-status.pipe";

@Component({
      selector: 'outgoing-payments-table',
      standalone: true,
      imports: [CommonModule, OutgoingPaymentStatusPipe],
      templateUrl: './outgoing-payments-table.component.html',
})
export class OutgoingPaymentsTableComponent {
      // Danh sách các khoản chi đã được tạo.
      @Input() outgoings: OutgoingPaymentBriefDto[] | null | undefined = [];

      // Tổng tiền khoản chi đã ghi nhận.
      @Input() totalPaid: number | null | undefined = 0;
}