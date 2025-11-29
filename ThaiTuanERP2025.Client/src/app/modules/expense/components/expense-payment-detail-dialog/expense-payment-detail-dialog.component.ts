import { ExpensePaymentDetailDto } from './../../models/expense-payment.model';
import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ExpensePaymentApiService } from '../../services/api/expense-payment.service';
import { firstValueFrom } from 'rxjs';
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";


@Component({
      selector: 'expense-payment-detail-dialog',
      standalone: true,
      imports: [CommonModule, AvatarUrlPipe],
      templateUrl: './expense-payment-detail-dialog.component.html',
      styleUrl: './expense-payment-detail-dialog.component.scss'
})
export class ExpensePaymentDetailDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<ExpensePaymentDetailDialogComponent>);
      private readonly expensePaymentApi = inject(ExpensePaymentApiService);

      paymentId: string;
      paymentDetail: ExpensePaymentDetailDto | null = null;

      constructor(@Inject(MAT_DIALOG_DATA) public data: string) {
            this.paymentId = data;
            this.getPaymentDetail(this.paymentId);
      }

      async getPaymentDetail(id: string) {
            this.paymentDetail = await firstValueFrom(this.expensePaymentApi.getDetailById(id));
            console.log('detail: ', this.paymentDetail);
      }

      close(isSuccess: boolean = false) {
            this.dialogRef.close(isSuccess);
      }
}