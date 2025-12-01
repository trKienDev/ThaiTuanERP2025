import { ExpensePaymentDetailDto } from './../../models/expense-payment.model';
import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ExpensePaymentApiService } from '../../services/api/expense-payment.service';
import { firstValueFrom, map, Observable, shareReplay } from 'rxjs';
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";
import { trigger, transition, style, animate } from '@angular/animations';
import { ExpensePaymentStatusPipe } from "../../pipes/expense-payment-status.pipe";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { ExpenseWorkflowInstanceApiService } from '../../services/api/expense-workflow-instance.service';
import { ToastService } from '../../../../shared/components/kit-toast-alert/kit-toast-alert.service';
import {  ExpenseStepInstanceDetailDto } from '../../models/expense-step-instance.model';
import { CountdownService } from '../../../../shared/services/countdown.service';
import { HttpErrorHandlerService } from '../../../../core/services/http-errror-handler.service';
import { Router } from '@angular/router';
import { UserFacade } from '../../../account/facades/user.facade';
import { KitFlipCountdownComponent } from "../../../../shared/components/kit-flip-countdown/kit-flip-countdown.component";

@Component({
      selector: 'expense-payment-detail-dialog',
      standalone: true,
      imports: [CommonModule, AvatarUrlPipe, ExpensePaymentStatusPipe, KitSpinnerButtonComponent, KitFlipCountdownComponent],
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
      currentStepStatus$!: Observable<{ seconds: number, expired: boolean }>;
      private readonly countdown = inject(CountdownService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly router = inject(Router);
      private readonly currentUser$ = inject(UserFacade).currentUser$;
      approving = false;
      rejecting = false;
      submitting = false;
      canApproveOrReject = false;
      
      paymentId: string;
      paymentDetail: ExpensePaymentDetailDto | null = null;

      constructor(@Inject(MAT_DIALOG_DATA) public data: string) {
            this.paymentId = data;
            this.getPaymentDetail(this.paymentId);
      }

      async getPaymentDetail(id: string) {
            this.paymentDetail = await firstValueFrom(this.expensePaymentApi.getDetailById(id));

            const step = this.currentStep;
            const userId = (await firstValueFrom(this.currentUser$)).id;
            this.canApproveOrReject = step?.approverIds?.includes(userId) ?? false;

            if (!step?.dueAt) {
                  console.warn("Current step has no dueAt → skip countdown");
                  return;
            }
            if (step) { 
                  const due = new Date(step.dueAt);

                  this.currentStepStatus$ = this.countdown.createCountdown(due).pipe(
                        map(seconds => ({
                              seconds,
                              expired: seconds <= 0
                        })),
                        shareReplay(1)
                  );
            }
      }

      // ==== CURRENT STEP ====
      get currentStep(): ExpenseStepInstanceDetailDto | undefined {
            const wf = this.paymentDetail?.workflowInstance;
            if (!wf) return undefined;
            return wf.steps.find(s => s.order === wf.currentStepOrder);
      }
      get currentStepSafe(): ExpenseStepInstanceDetailDto | null {
            const wf = this.paymentDetail?.workflowInstance;
            if (!wf || !wf.steps || wf.steps.length === 0) return null;
            return wf.steps[wf.currentStepOrder] || null;
      }


      // === TAB NAVIGATION ===     
      activeTab: 'items' | 'outgoings' = 'items';
      setActiveTab(tab: 'items' | 'outgoings') {
            this.activeTab = tab;
      }

      // actions
      async approve() {
            this.approving = true;
            this.submitting = true;
            if (!this.paymentDetail?.workflowInstance?.id) {
                  this.toast.errorRich("Không thể truy vấn luồng duyệt của thanh toán này");
                  console.error("workflowInstance.id is missing");
                  return;
            }

            try {
                  await firstValueFrom(this.expenseWorkflowInstanceApi.approve(this.paymentDetail?.workflowInstance.id));
                  this.toast.successRich("Duyệt thanh toán thành công");
                  this.close(true);
                  this.router.navigateByUrl('expense/expense-payment-shell/following-payments');
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Duyệt thất bại");
            } finally {
                  this.approving = false;
                  this.submitting = false;
            }
      }


      close(isSuccess: boolean = false) {
            this.dialogRef.close(isSuccess);
      }
}