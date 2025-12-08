import { CommentDetailDto, CommentPayload } from './../../../../core/models/comment.model';
import { ExpensePaymentDetailDto } from '../../../models/expense-payment.model';
import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ExpensePaymentApiService } from '../../../services/api/expense-payment.service';
import { firstValueFrom, map, Observable, shareReplay } from 'rxjs';
import { AvatarUrlPipe } from "../../../../../shared/pipes/avatar-url.pipe";
import { trigger, transition, style, animate } from '@angular/animations';
import { ExpensePaymentStatusPipe } from "../../../pipes/expense-payment-status.pipe";
import { KitSpinnerButtonComponent } from "../../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { ExpenseWorkflowInstanceApiService } from '../../../services/api/expense-workflow-instance.service';
import { ToastService } from '../../../../../shared/components/kit-toast-alert/kit-toast-alert.service';
import {  ExpenseStepInstanceDetailDto } from '../../../models/expense-step-instance.model';
import { CountdownService } from '../../../../../shared/services/countdown.service';
import { HttpErrorHandlerService } from '../../../../../core/services/http-errror-handler.service';
import { Router } from '@angular/router';
import { UserFacade } from '../../../../account/facades/user.facade';
import { KitFlipCountdownComponent } from "../../../../../shared/components/kit-flip-countdown/kit-flip-countdown.component";
import { ExpenseWorkflowStatus } from '../../../models/expense-workflow-instance.model';
import { FilePreviewService, StoredFileMetadataDto } from '../../../../../core/services/file-preview.service';
import { ExpensePaymentItemLookupDto } from '../../../models/expense-payment-item.model';
import { OutgoingPaymentStatusPipe } from "../../../pipes/outgoing-payment-status.pipe";
import { KitShellTabsComponent } from '../../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component';
import { ExpensePaymentItemsTableComponent } from "../../tables/expense-payment-items-table/expense-payment-items-table.component";
import { OutgoingPaymentsTableComponent } from "../../tables/outgoing-payments-table/outgoing-payments-table.component";
import { CommentApiService } from '../../../../core/services/api/comment.service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { DOCUMENT_TYPE } from '../../../../../core/constants/document-types.constants';
import { CommentThreadComponent } from "../../../../core/components/comment-thread/comment-thread.component";
import { UploadItem } from '../../../../../shared/components/kit-file-uploader/upload-item.model';
import { KitFileUploaderComponent } from "../../../../../shared/components/kit-file-uploader/kit-file-uploader.component";
import { FileService } from '../../../../../shared/services/file.service';

@Component({
      selector: 'expense-payment-detail-dialog',
      standalone: true,
      imports: [CommonModule, AvatarUrlPipe, ExpensePaymentStatusPipe, KitSpinnerButtonComponent, KitFlipCountdownComponent, OutgoingPaymentStatusPipe, ExpensePaymentItemsTableComponent, OutgoingPaymentsTableComponent, ReactiveFormsModule, CommentThreadComponent, KitFileUploaderComponent],
      templateUrl: './expense-payment-detail-dialog.component.html',
      styleUrls: ['./expense-payment-detail-dialog.component.scss'],
      animations: [
            trigger('statusChangeFade', [
                  transition('* => *', [
                        style({ opacity: 0, transform: 'scale(0.98)' }),
                        animate('280ms ease-out', style({ opacity: 1, transform: 'scale(1)' }))
                  ])
            ]),
      ]
})
export class ExpensePaymentDetailDialogComponent implements OnInit {
      private readonly dialogRef = inject(MatDialogRef<ExpensePaymentDetailDialogComponent>);
      private readonly expensePaymentApi = inject(ExpensePaymentApiService);
      private readonly expenseWorkflowInstanceApi = inject(ExpenseWorkflowInstanceApiService);
      private readonly toast = inject(ToastService);
      currentStepStatus$!: Observable<{ seconds: number, expired: boolean }>;
      private readonly countdown = inject(CountdownService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly router = inject(Router);
      currentUser$ = inject(UserFacade).currentUser$;
      private readonly filePreview = inject(FilePreviewService);
      private readonly fileApi = inject(FileService);

      approving = false;
      rejecting = false;
      submitting = false;
      canApproveOrReject = false;
      isInProgress = false;
      
      paymentId: string;
      paymentDetail: ExpensePaymentDetailDto | null = null;

      commentUploads: UploadItem[] = [];
      uploadMetaForComment = {
            module: 'expense',
            entity: 'comment-attachment',
            entityId: undefined as string | undefined,
            isPublic: false
      };

      constructor(@Inject(MAT_DIALOG_DATA) public data: string) {
            this.paymentId = data;
            this.getPaymentDetail(this.paymentId);
      }

      ngOnInit(): void {
            this.getComments();
      }

      async getPaymentDetail(id: string) {
            this.paymentDetail = await firstValueFrom(this.expensePaymentApi.getDetailById(id));

            const step = this.currentStep;
            const userId = (await firstValueFrom(this.currentUser$)).id;

            this.canApproveOrReject = step?.approverIds?.includes(userId) ?? false;
            this.isInProgress = this.paymentDetail.workflowInstance.status === ExpenseWorkflowStatus.inProgress;

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

      async reject() {
            this.rejecting = true;
            this.submitting = true;
            if (!this.paymentDetail?.workflowInstance?.id) {
                  this.toast.errorRich("Không thể truy vấn luồng duyệt của thanh toán này");
                  console.error("workflowInstance.id is missing");
                  return;
            }

            try {
                  await firstValueFrom(this.expenseWorkflowInstanceApi.reject(this.paymentDetail?.workflowInstance.id));
                  this.toast.successRich("Đã từ chối thanh toán");
                  
                  this.close(true);
                  this.router.navigateByUrl('expense/expense-payment-shell/following-payments');
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Từ chối thất bại");
            } finally {
                  this.rejecting = false;
                  this.submitting = false;
            }
      }

      previewInvoice(item: ExpensePaymentItemLookupDto) {
            if (!item.invoiceFile) {
                  this.toast.errorRich("Không tìm thấy hóa đơn");
                  return;
            }

            this.filePreview.previewStoredFile({
                  fileId: item.invoiceFile.fileId ?? '',
                  objectKey: item.invoiceFile.objectKey ?? '',
                  fileName: item.invoiceFile.fileName ?? 'invoice',
                  isPublic: item.invoiceFile.isPublic ?? false
            });
      }

      previewAttachment(item: StoredFileMetadataDto) {
            if(!item) {
                  this.toast.errorRich("Không tìm thấy tệp đính kèm");
                  return;
            }

            this.filePreview.previewStoredFile({
                  fileId: item.fileId ?? '',
                  objectKey: item.objectKey ?? '',
                  fileName: item.fileName ?? '',
                  isPublic: item.isPublic ?? false
            })
      }

      redirectToOutgoingPaymentRequestPage(paymentId: string) {
            this.dialogRef.close({ redirect: true, paymentId });
            KitShellTabsComponent.allowOnce('outgoing-payment-request');
            this.router.navigateByUrl(`/expense/outgoing-payment-shell/outgoing-payment-request/${paymentId}`);  
      }

      close(isSuccess: boolean = false) {
            this.dialogRef.close(isSuccess);
      }

      // ===== Comment =====
      private readonly commentApi = inject(CommentApiService);
      module: string = "expense";
      entity: string ="expense-payment";
      isCommenting = false;
      isSubmittingComment = false;
      commentControl = new FormControl<string>('', { nonNullable: true });
      comments: CommentDetailDto[] = [];

      async getComments() {
            this.comments = await firstValueFrom(this.commentApi.getComments(DOCUMENT_TYPE.EXPENSE_PAYMENT, this.paymentId));
      }

      get canSubmitComment(): boolean {
            return !this.isSubmittingComment && this.commentControl.value.trim().length > 0;
      }

      async submitComment() {
            const content = this.commentControl.value.trim();

            if (!content) {
                  this.toast.errorRich("Bạn chưa nhập bình luận");
                  return;
            }

            try {
                  this.isSubmittingComment = true;
                  this.isCommenting = false;

                  // ====== 1. Upload ATTACHMENTS trước ======
                  const uploadedIds: string[] = [];

                  for (const u of this.commentUploads) {
                        try {
                              const result = await firstValueFrom(
                                    this.fileApi.uploadFile(
                                          u.file,
                                          'expense',
                                          'expense-payment-comment-attachment',
                                          this.paymentId,
                                          false
                                    )
                              );

                              if (result.data?.id) {
                                    uploadedIds.push(result.data.id);
                                    u.fileId = result.data.id;
                                    u.status = 'done';
                              }
                        } catch (err) {
                              u.status = 'error';
                        }
                  }

                  const payload: CommentPayload = ({
                        documentType: DOCUMENT_TYPE.EXPENSE_PAYMENT,
                        documentId: this.paymentId,
                        content: content,
                        attachmentIds: uploadedIds.length ? uploadedIds : undefined
                  });

                  console.log('payload: ', payload);

                  const newCommentDto = await firstValueFrom(this.commentApi.create(payload));
                  this.comments.unshift(newCommentDto);
                  
                  this.commentControl.setValue('');
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Bình luận không thành công");
            } finally {
                  this.isCommenting = false;
                  this.isSubmittingComment = false;
            }
      }

      cancelComment() {
            this.isCommenting = false;
      }

      // ==== REPLY ====
      replyingToCommentId: string | null = null;
      isSubmittingReply = false;
      replyControl = new FormControl<string>('', { nonNullable: true });

      startReply(commentId: string | null) {
            this.replyingToCommentId = commentId;
            this.comments = [...this.comments];
      }
      
      async submitReply(event: { parentId: string; content: string }) {
            const { parentId, content } = event;

            const parent = this.findCommentRecursive(this.comments, parentId);
            if (!parent) {
                  this.toast.errorRich("Lỗi chương trình, ko tìm thấy comment cha");
                  return;
            }

            if (!content) {
                  this.toast.warningRich("Bạn chưa nhập nội dung phản hồi");
                  return;
            }
            
            try {
                  this.isSubmittingReply = true;

                  const payload: CommentPayload = {
                        documentType: DOCUMENT_TYPE.EXPENSE_PAYMENT,
                        documentId: this.paymentId,
                        content: content.trim(),
                  };

                  const newReply = await firstValueFrom(this.commentApi.reply(parentId, payload));

                  parent.replies = parent.replies ?? [];
                  parent.replies.push(newReply);

                  this.comments = [...this.comments]; // force update để OnPush chạy

                  this.replyingToCommentId = null;
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Gửi phản hồi thất bại");
            } finally {
                  this.isSubmittingReply = false;
            }
      }

      cancelReply() {
            this.replyingToCommentId = null;
      }

      previewCommentLocalFile(file: File) {
            this.filePreview.previewLocalFile(file);
      }

      private findCommentRecursive(list: CommentDetailDto[], id: string): CommentDetailDto | null {
            for (const c of list) {
                  if (c.id === id) return c;
                  if (c.replies?.length) {
                        const found = this.findCommentRecursive(c.replies, id);
                        if (found) return found;
                  }
            }
            return null;
      }
}