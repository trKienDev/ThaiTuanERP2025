import { ExpensePaymentDetailDto } from './../../models/expense-payment.model';
import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ExpensePaymentApiService } from '../../services/api/expense-payment.service';
import { firstValueFrom } from 'rxjs';
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";
import { trigger, transition, style, animate } from '@angular/animations';


@Component({
      selector: 'expense-payment-detail-dialog',
      standalone: true,
      imports: [CommonModule, AvatarUrlPipe],
      templateUrl: './expense-payment-detail-dialog.component.html',
      styleUrl: './expense-payment-detail-dialog.component.scss',
      animations: [
            trigger('tabSwitchFade', [
                  transition(':enter', [
                        style({ opacity: 0, transform: 'translateX(20px)' }),
                        animate('250ms cubic-bezier(0.25, 0.1, 0.25, 1)', style({ opacity: 1, transform: 'translateX(0)' }))
                  ]),
                  transition(':leave', [
                        animate('200ms cubic-bezier(0.25, 0.1, 0.25, 1)', style({ opacity: 0, transform: 'translateX(-20px)' }))
                  ])
            ]),
      ]
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

      // === TAB NAVIGATION ===
      activeTab: 'items' | 'outgoings' = 'items';
      setActiveTab(tab: 'items' | 'outgoings') {
            this.activeTab = tab;
      }

      close(isSuccess: boolean = false) {
            this.dialogRef.close(isSuccess);
      }
}