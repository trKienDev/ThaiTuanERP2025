import { CommonModule } from "@angular/common";
import { Component, Inject, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ExpensePaymentDetailDto } from "../../../../models/expense-payment.model";
import { firstValueFrom } from "rxjs";
import { ExpensePaymentService } from "../../../../services/expense-payment.service";
import { ExpensePaymentStatusPipe } from "../../../../pipes/expense-payment-status.pipe";
import { AvatarUrlPipe } from "../../../../../../shared/pipes/avatar-url.pipe";
import { Router } from "@angular/router";

@Component({
      selector: 'expense-payment-detail-dialog',
      standalone: true,
      imports: [CommonModule, ExpensePaymentStatusPipe, AvatarUrlPipe],
      templateUrl: './expense-payment-detail-dialog.component.html',
      styleUrls: ['./expense-payment-detail-dialog.component.scss']
})
export class ExpensePaymentDetailDialogComponent {
      private dialogRef = inject(MatDialogRef<ExpensePaymentDetailDialogComponent>);
       private router = inject(Router);
      private paymentId: string = '';
      public paymentDetail: ExpensePaymentDetailDto | null = null;
      private expensePaymentService = inject(ExpensePaymentService);
      
      constructor(
            @Inject(MAT_DIALOG_DATA) public data: string
      ) {
            this.paymentId = data;
            this.getPaymentDetails();
      }

      trackByIndex = (index: number) => index;

      async getPaymentDetails() {
            this.paymentDetail = await firstValueFrom(this.expensePaymentService.getDetailById(this.paymentId));
      }

      navigateToOutgoingPaymentRequest() {
            if (!this.paymentDetail) return;
            this.close(); // đóng dialog
            this.router.navigate([
                  '/expense/outgoing-payment-shell/outgoing-payment-request',
                  this.paymentDetail.id
            ]);
      }

      close(): void {
            this.dialogRef.close();
      }
}