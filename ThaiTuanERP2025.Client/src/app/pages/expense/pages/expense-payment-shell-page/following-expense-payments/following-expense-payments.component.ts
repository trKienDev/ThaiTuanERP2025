import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { ExpensePaymentDetailDto, ExpensePaymentDto, ExpensePaymentSummaryDto } from "../../../models/expense-payment.model";
import { firstValueFrom } from "rxjs";
import { ExpensePaymentService } from "../../../services/expense-payment.service";
import { ExpensePaymentStatusPipe } from "../../../pipes/expense-payment-status.pipe";
import { AvatarUrlPipe } from "../../../../../shared/pipes/avatar-url.pipe";
import { MatDialog } from "@angular/material/dialog";
import { ExpensePaymentDetailDialogComponent } from "../expense-payment-detail/expense-payment-detail-dialog/expense-payment-detail-dialog.component";

@Component({
      selector: 'expense-payments-panel',
      standalone: true,
      templateUrl: './following-expense-payments.component.html',
      styleUrls: ['./following-expense-payments.component.scss'],
      imports: [CommonModule, ExpensePaymentStatusPipe, AvatarUrlPipe ]
})
export class FollowingExpensePaymentsPanelComponent implements OnInit {
      private dialog = inject(MatDialog);
      public expensePayments: ExpensePaymentSummaryDto[] = [];
      private expensePaymentService = inject(ExpensePaymentService);

      async ngOnInit(): Promise<void> {
            await this.loadExpensePayments();
      }

      private async loadExpensePayments() {
            this.expensePayments = await firstValueFrom(this.expensePaymentService.getFollowingPayments());
      }

      openExpensePaymentDetailDialog(paymentId: string) {
            const dialogRef = this.dialog.open(ExpensePaymentDetailDialogComponent, {
                  data: paymentId,
            });

            dialogRef.afterClosed().subscribe((result: any) => {
                  if (result?.success) {
                        // Handle success result if needed
                  }     
            });
      }
}