import { CommonModule } from "@angular/common";
import { Component, Inject, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ExpensePaymentDetailDto } from "../../../../models/expense-payment.model";
import { firstValueFrom } from "rxjs";
import { ExpensePaymentService } from "../../../../services/expense-payment.service";
import { ExpensePaymentStatusPipe } from "../../../../pipes/expense-payment-status.pipe";
import { AvatarUrlPipe } from "../../../../../../shared/pipes/avatar-url.pipe";

@Component({
      selector: 'expense-payment-detail-dialog',
      standalone: true,
      imports: [CommonModule, ExpensePaymentStatusPipe, AvatarUrlPipe],
      templateUrl: './expense-payment-detail-dialog.component.html',
      styleUrls: ['./expense-payment-detail-dialog.component.scss']
})
export class ExpensePaymentDetailDialogComponent {
      private dialogRef = inject(MatDialogRef<ExpensePaymentDetailDialogComponent>);
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



      close(): void {
            this.dialogRef.close();
      }
}