import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ExpensePaymentDetailDto, ExpensePaymentStatus } from "../../../../models/expense-payment.model";
import { ExpensePaymentStatusPipe } from "../../../../pipes/expense-payment-status.pipe";
import { AvatarUrlPipe } from "../../../../../../shared/pipes/avatar-url.pipe";
import { ActivatedRoute, Router } from "@angular/router";
import { usePaymentDetail } from "../../../../composables/use-payment-detail";
import { ApprovalStepInstanceDetailDto, ApproveStepRequest, StepStatus } from "../../../../models/approval-step-instance.model";
import { UserFacade } from "../../../../../account/facades/user.facade";
import { UserDto } from "../../../../../account/models/user.model";
import { ToastService } from "../../../../../../shared/components/toast/toast.service";
import { firstValueFrom } from "rxjs";
import { ApprovalWorkflowInstanceService } from "../../../../services/approval-workflow-instance.service";
import { trigger, transition, style, animate } from "@angular/animations";
import { KitLoadingSpinnerComponent } from "../../../../../../shared/components/kit-loading-spinner/kit-loading-spinner.component";
import { Kit404PageComponent } from "../../../../../../shared/components/kit-404-page/kit-404-page.component";
import { KitFlipCountdownComponent } from "../../../../../../shared/components/kit-flip-countdown/kit-flip-countdown.component";
import { KitOverlaySpinnerComponent } from "../../../../../../shared/components/kit-overlay-spinner/kit-overlay-spinner.component";
import { KitSpinnerButtonComponent } from "../../../../../../shared/components/kit-spinner-button/kit-spinner-button.component";

@Component({
      selector: 'expense-payment-detail-dialog',
      standalone: true,
      imports: [CommonModule, ExpensePaymentStatusPipe, AvatarUrlPipe, KitLoadingSpinnerComponent, Kit404PageComponent, KitFlipCountdownComponent, KitSpinnerButtonComponent],
      templateUrl: './expense-payment-detail-dialog.component.html',
      styleUrls: ['./expense-payment-detail-dialog.component.scss'],
      animations: [
            trigger('commentBox', [
                  transition(':enter', [
                        style({ opacity: 0, transform: 'translateY(-8px)', height: 0, overflow: 'hidden' }),
                        animate(
                              '180ms ease-out',
                              style({ opacity: 1, transform: 'translateY(0)', height: '*', overflow: 'visible' })
                        )
                  ]),
                  transition(':leave', [
                        animate(
                              '120ms ease-in',
                              style({ opacity: 0, transform: 'translateY(-8px)', height: 0, overflow: 'hidden' })
                        )
                  ]),
            ]),
            trigger('actionsFade', [
                  transition(':enter', [
                        style({ opacity: 0 }),
                        animate('220ms ease-out', style({ opacity: 1 }))
                  ]),
                  transition(':leave', [
                        animate('220ms ease-in', style({ opacity: 0 }))
                  ])
            ]),
      ],
})
export class ExpensePaymentDetailDialogComponent implements OnInit {
      private route = inject(ActivatedRoute);
      private dialogRef = inject(MatDialogRef<ExpensePaymentDetailDialogComponent>);
      private router = inject(Router);
      private userFacade = inject(UserFacade);
      currentUser$ = this.userFacade.currentUser$;
      currentUser: UserDto | null = null;
      private toastService = inject(ToastService);
      private readonly workflowInstanceService = inject(ApprovalWorkflowInstanceService);
      paymentId: string = '';
      private paymentLogic = usePaymentDetail();
      loading = this.paymentLogic.isLoading;
      err = this.paymentLogic.error;
      commentText: string = '';
      public currentStepOrder: number = 0;
      public approving = false;

      constructor(@Inject(MAT_DIALOG_DATA) public data: string) {
            if(data) this.paymentLogic.load(data);
            this.paymentId = data;

      }

      async ngOnInit(): Promise<void> {
            this.currentUser = await firstValueFrom(this.currentUser$);
            this.currentStepOrder = this.paymentDetail?.workflowInstanceDetail?.workflowInstance.currentStepOrder || 0;
      }

      get paymentDetail(): ExpensePaymentDetailDto | null {
            // console.log('paymentDetail: ', this.paymentLogic.paymentDetail());
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

      async onApprove() {
            if (this.approving) return;
            this.approving = true;

            try {
                  const workflowInstanceDetail = this.paymentDetail?.workflowInstanceDetail;
                  if(!workflowInstanceDetail) return;
                  const currentStep = workflowInstanceDetail.steps.find(s => s.order === workflowInstanceDetail.workflowInstance.currentStepOrder);
                  if(!currentStep) return;
                  if (!this.currentUser?.id) { alert('Không xác định được người dùng hiện tại'); return; }
                  if (!this.paymentId) { alert('Thiếu PaymentId'); return; }

                  const payload: ApproveStepRequest = {
                        userId: this.currentUser.id,
                        paymentId: this.paymentId,
                        comment: this.commentText || ''
                  };
                  
                  const result = await firstValueFrom(this.workflowInstanceService.approveStep(
                        workflowInstanceDetail.workflowInstance.id, 
                        currentStep.id,
                        payload
                  ));
                  if(result) { 
                        this.toastService.successRich('Duyệt thành công'); 
                        await this.paymentLogic.refresh();
                  }
            } catch (error) {
                  this.toastService.errorRich('Duyệt thất bại. Vui lòng thử lại.');
            } finally {
                  this.approving = false;
            }
      }

      async onReject() {
            const workflowInstanceDetail = this.paymentDetail?.workflowInstanceDetail;
            if(!workflowInstanceDetail) return;
            const currentStep = workflowInstanceDetail.steps.find(s => s.order === workflowInstanceDetail.workflowInstance.currentStepOrder);
            if(!currentStep) return;
            if (!this.currentUser?.id) { alert('Không xác định được người dùng hiện tại'); return; }
            if (!this.paymentId) { alert('Thiếu PaymentId'); return; }

            const payload: ApproveStepRequest = {
                  userId: this.currentUser.id,
                  paymentId: this.paymentId,
                  comment: this.commentText || ''
            };
            
            const result = await firstValueFrom(this.workflowInstanceService.rejectStep(
                  workflowInstanceDetail.workflowInstance.id, 
                  currentStep.id,
                  payload
            ));
            if(result) { 
                  this.toastService.successRich(result); 
                  await this.paymentLogic.refresh();
            }
      }

      get currentStep(): ApprovalStepInstanceDetailDto | undefined {
            const wf = this.paymentDetail?.workflowInstanceDetail;
            if (!wf) return undefined;
            return wf.steps.find(s => s.order === wf.workflowInstance.currentStepOrder);
      }
      get canApproveOrReject(): boolean {
            const wf = this.paymentDetail?.workflowInstanceDetail;
            const step = this.currentStep;
            const userId = this.currentUser?.id;
            
            const isPaymentPending = this.paymentDetail?.status === ExpensePaymentStatus.pending; // 2
            const isStepWaiting = step?.status === StepStatus.Waiting;
            const isCurrentUserApprover = !!userId && (step?.resolvedApproverCandidateIds ?? []).includes(userId);

            return !!(isPaymentPending && isStepWaiting && isCurrentUserApprover);
      }

      get currentStepSafe(): ApprovalStepInstanceDetailDto | null {
            const wf = this.paymentDetail?.workflowInstanceDetail;
            if (!wf || !wf.steps || wf.steps.length === 0) return null;
            return wf.steps[this.currentStepOrder] || null;
      }

      getSecondsRemaining(step: ApprovalStepInstanceDetailDto): number {
            const sec = Math.max(0, Math.floor((new Date(step.dueAt).getTime() - Date.now()) / 1000));
            return sec
      }

      isExpired(item: ApprovalStepInstanceDetailDto): boolean {
            console.log('DueAt:', item.dueAt, 'Now:', new Date().toISOString());
            return new Date(item.dueAt).getTime() <= Date.now();
      }
}