import { CommonModule } from "@angular/common";
import { Component, Inject, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ExpensePaymentDetailDto } from "../../../../models/expense-payment.model";
import { ExpensePaymentStatusPipe } from "../../../../pipes/expense-payment-status.pipe";
import { AvatarUrlPipe } from "../../../../../../shared/pipes/avatar-url.pipe";
import { Router } from "@angular/router";
import { usePaymentDetail } from "../../../../composables/use-payment-detail";
import { TextareaNoSpellcheckDirective } from "../../../../../../shared/directives/money/textarea/textarea-no-spellcheck.directive";

@Component({
      selector: 'expense-payment-detail-dialog',
      standalone: true,
      imports: [CommonModule, ExpensePaymentStatusPipe, AvatarUrlPipe, TextareaNoSpellcheckDirective],
      templateUrl: './expense-payment-detail-dialog.component.html',
      styleUrls: ['./expense-payment-detail-dialog.component.scss']
})
export class ExpensePaymentDetailDialogComponent {
      private dialogRef = inject(MatDialogRef<ExpensePaymentDetailDialogComponent>);
      private router = inject(Router);

      private paymentLogic = usePaymentDetail();
      loading = this.paymentLogic.isLoading;
      err = this.paymentLogic.error;
      
      constructor(@Inject(MAT_DIALOG_DATA) public data: string) {
            if(data) this.paymentLogic.load(data);
      }

      get paymentDetail(): ExpensePaymentDetailDto | null {
            return this.paymentLogic.paymentDetail();
      }

      trackByIndex = (index: number) => index;



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