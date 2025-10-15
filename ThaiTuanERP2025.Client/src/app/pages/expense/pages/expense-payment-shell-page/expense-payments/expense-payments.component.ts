import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { ExpensePaymentDetailDto, ExpensePaymentDto, ExpensePaymentSummaryDto } from "../../../models/expense-payment.model";
import { firstValueFrom } from "rxjs";
import { ExpensePaymentService } from "../../../services/expense-payment.service";
import { ExpensePaymentStatusPipe } from "../../../pipes/expense-payment-status.pipe";
import { AvatarUrlPipe } from "../../../../../shared/pipes/avatar-url.pipe";

@Component({
      selector: 'expense-payments-panel',
      standalone: true,
      templateUrl: './expense-payments.component.html',
      styleUrls: ['./expense-payments.component.scss'],
      imports: [CommonModule, ExpensePaymentStatusPipe, AvatarUrlPipe ]
})
export class ExpensePaymentsPanelComponent implements OnInit {
      public expensePayments: ExpensePaymentSummaryDto[] = [];
      private expensePaymentService = inject(ExpensePaymentService);

      async ngOnInit(): Promise<void> {
            await this.loadExpensePayments();
      }

      private async loadExpensePayments() {
            this.expensePayments = await firstValueFrom(this.expensePaymentService.getFollowingPayments());
            console.log(this.expensePayments);
      }
}