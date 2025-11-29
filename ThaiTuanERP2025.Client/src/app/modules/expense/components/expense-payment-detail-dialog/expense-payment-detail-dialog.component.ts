import { ExpensePaymentDetailDto } from './../../models/expense-payment.model';
import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ExpensePaymentApiService } from '../../services/api/expense-payment.service';
import { firstValueFrom } from 'rxjs';
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";
import { trigger, transition, style, animate } from '@angular/animations';
import { ExpensePaymentStatusPipe } from "../../pipes/expense-payment-status.pipe";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { ExpenseWorkflowInstanceApiService } from '../../services/api/expense-workflow-instance.service';
import { ToastService } from '../../../../shared/components/kit-toast-alert/kit-toast-alert.service';

@Component({
      selector: 'expense-payment-detail-dialog',
      standalone: true,
      imports: [CommonModule, AvatarUrlPipe, ExpensePaymentStatusPipe, KitSpinnerButtonComponent],
      templateUrl: './expense-payment-detail-dialog.component.html',
      styleUrl: './expense-payment-detail-dialog.component.scss',
      animations: [
            trigger('statusChangeFade', [
                  transition('* => *', [
                        style({ opacity: 0, transform: 'scale(0.98)' }),
                        animate('280ms ease-out', style({ opacity: 1, transform: 'scale(1)' }))
                  ])
            ]),
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
      private readonly expenseWorkflowInstanceApi = inject(ExpenseWorkflowInstanceApiService);
      private readonly toast = inject(ToastService);

      paymentId: string;
      paymentDetail: ExpensePaymentDetailDto | null = null;

      constructor(@Inject(MAT_DIALOG_DATA) public data: string) {
            this.paymentId = data;
            this.getPaymentDetail(this.paymentId);
      }

      async getPaymentDetail(id: string) {
            this.paymentDetail = await firstValueFrom(this.expensePaymentApi.getDetailById(id));
      }

      // === TAB NAVIGATION ===
      activeTab: 'items' | 'outgoings' = 'items';
      setActiveTab(tab: 'items' | 'outgoings') {
            this.activeTab = tab;
      }

      // actions
      async approve() {
            if (!this.paymentDetail?.workflowInstance?.id) {
                  this.toast.errorRich("Không thể truy vấn luồng duyệt của thanh toán này");
                  console.error("workflowInstance.id is missing");
                  return;
            }
            const resut = await firstValueFrom(this.expenseWorkflowInstanceApi.approve(this.paymentDetail?.workflowInstance.id));
      }


      close(isSuccess: boolean = false) {
            this.dialogRef.close(isSuccess);
      }
}