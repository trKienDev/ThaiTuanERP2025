import { trigger, transition, style, animate, state } from "@angular/animations";
import { CommonModule } from "@angular/common";
import { Component, ElementRef, inject, ViewChild } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { AutoResizeDirective } from "../../../../../shared/directives/money/textarea/textarea-auto-resize.directive";
import { TextareaNoSpellcheckDirective } from "../../../../../shared/directives/money/textarea/textarea-no-spellcheck.directive";
import { AvatarUrlPipe } from "../../../../../shared/pipes/avatar-url.pipe";
import { ExpensePaymentStatusPipe } from "../../../pipes/expense-payment-status.pipe";
import { MatDialog } from "@angular/material/dialog";
import { ActivatedRoute } from "@angular/router";
import { firstValueFrom } from "rxjs";
import { environment } from "../../../../../../environments/environment";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { UserFacade } from "../../../../account/facades/user.facade";
import { UserDto } from "../../../../account/models/user.model";
import { ApproveStepRequest, ApprovalStepInstanceDetailDto, StepStatus } from "../../../models/approval-step-instance.model";
import { ExpensePaymentCommentDto, ExpensePaymentCommentRequest } from "../../../models/expense-payment-comment.model";
import { ExpensePaymentDetailDto, ExpensePaymentStatus } from "../../../models/expense-payment.model";
import { InvoiceDto } from "../../../models/invoice.model";
import { ApprovalWorkflowInstanceService } from "../../../services/approval-workflow-instance.service";
import { ExpensePaymentCommentService } from "../../../services/expense-payment-comment.service";
import { ExpensePaymentService } from "../../../services/expense-payment.service";
import { InvoiceDetailDialogComponent } from "../../invoices/invoice-detail-dialog/invoice-detail-dialog.component";

@Component({
      selector: 'expense-payment-detail',
      standalone: true,
      templateUrl: './expense-payment-detail.component.html',
      styleUrls: ['./expense-payment-detail.component.scss'],
      imports: [CommonModule, FormsModule, ExpensePaymentStatusPipe, AvatarUrlPipe, AutoResizeDirective, TextareaNoSpellcheckDirective],
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
            ])
      ],
})
export class ExpensePaymentDetailPanelComponent { 
      private route = inject(ActivatedRoute);
      private expensePaymentService = inject(ExpensePaymentService);
      private epCommentService = inject(ExpensePaymentCommentService);
      private userFacade = inject(UserFacade);
      currentUser$ = this.userFacade.currentUser$;
      private readonly matDialog = inject(MatDialog);
      currentUser: UserDto | null = null;
      private readonly workflowInstanceService = inject(ApprovalWorkflowInstanceService);
      private toastService = inject(ToastService);

      paymentId: string = '';
      paymentDetail: ExpensePaymentDetailDto | null = null;
      baseUrl: string = environment.baseUrl;    
      comments: ExpensePaymentCommentDto[] = []; 

      trackByIndex = (index: number) => index;
      @ViewChild('ta') textareaRef?: ElementRef<HTMLTextAreaElement>;
      
      async ngOnInit(): Promise<void> {
            this.route.queryParamMap.subscribe(map => {
                  const paymentId = map.get('paymentId');
                  if (paymentId) {
                        this.paymentId = paymentId;
                  }
            });
            this.currentUser = await firstValueFrom(this.currentUser$);
            this.getPaymentDetails();

            this.loadComments();
      }

      async getPaymentDetails() {
            this.paymentDetail = await firstValueFrom(this.expensePaymentService.getDetailById(this.paymentId));
            console.log('payment detail: ', this.paymentDetail);
      }

      trackByComment = (_: number, c: ExpensePaymentCommentDto) => c.id;

      // comment
      isCommenting: boolean = false;
      commentText: string = '';

      async loadComments() {
            this.comments = await firstValueFrom(this.epCommentService.getByPayment(this.paymentId));
      }

      startCommenting() {
            this.isCommenting = true;
            setTimeout(() => this.textareaRef?.nativeElement.focus(), 0);
      }
      cancelComment() {
            this.commentText = '';
            this.isCommenting = false;
      }
      async submitComment() {
            const content = this.commentText.trim();
            if(!content) return;
            
            const payload: ExpensePaymentCommentRequest = {
                  expensePaymentId: this.paymentId,
                  content: content,
            };
            const result = await firstValueFrom(this.epCommentService.submitComment(this.paymentId, payload));
            
            if (result) {
                  // Nếu là comment cha
                  if (!result.parentCommentId) {
                        // thêm lên đầu danh sách cho “cảm giác realtime”
                        this.comments = [{ ...result, replies: result.replies ?? [] }, ...this.comments];
                  } else {
                        // Nếu là reply 1 cấp: tìm cha và chèn vào replies
                        const parentIdx = this.comments.findIndex(c => c.id === result.parentCommentId);
                        if (parentIdx >= 0) {
                        const parent = this.comments[parentIdx];
                        const newReplies = [...(parent.replies ?? []), result];
                        // immutable update để Angular detect change
                        const newParent = { ...parent, replies: newReplies };
                        this.comments = [
                              ...this.comments.slice(0, parentIdx),
                              newParent,
                              ...this.comments.slice(parentIdx + 1)
                        ];
                        } else {
                        // không tìm thấy cha (trường hợp hiếm) ⇒ fallback: reload
                        await this.loadComments();
                        }
                  }
            } else {
                  // ---- CÁCH (2): nếu API trả về Unit (hoặc null) ⇒ reload
                  await this.loadComments();
            }

            this.commentText = '';
            this.isCommenting = false;
      }

      async openInvoiceDetail(invoice?: InvoiceDto | null) {
            if (!invoice) return;
            const dialogRef = this.matDialog.open(InvoiceDetailDialogComponent, {
                  data: { invoice }
            });

      }

      async onApprove() {
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
                  await this.getPaymentDetails();
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
                  await this.getPaymentDetails();
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

            const isCurrentUserApprover =
            !!userId && (step?.resolvedApproverCandidateIds ?? []).includes(userId);

            return !!(isPaymentPending && isStepWaiting && isCurrentUserApprover);
      }
}