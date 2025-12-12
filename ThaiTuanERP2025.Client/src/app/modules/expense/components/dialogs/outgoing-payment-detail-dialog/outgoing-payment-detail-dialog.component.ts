import { CommonModule } from "@angular/common";
import { Component, inject, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material/dialog";
import { firstValueFrom } from "rxjs";
import { Kit404PageComponent } from "../../../../../shared/components/kit-404-page/kit-404-page.component";
import { KitLoadingSpinnerComponent } from "../../../../../shared/components/kit-loading-spinner/kit-loading-spinner.component";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { AvatarUrlPipe } from "../../../../../shared/pipes/avatar-url.pipe";
import { useOutgoingPaymentDetail } from "../../../composables/use-outgoing-payment-detail";
import { OutgoingPaymentDetailDto } from "../../../models/outgoing-payment.model";
import { OutgoingPaymentStatusPipe } from "../../../pipes/outgoing-payment-status.pipe";
import { OutgoingPaymentApiService } from "../../../services/api/outgoing-payment.service";
import { ExpensePaymentItemsTableComponent } from "../../tables/expense-payment-items-table/expense-payment-items-table.component";
import { HttpErrorHandlerService } from "../../../../../core/services/http-errror-handler.service";
import { OutgoingPaymentsTableComponent } from "../../tables/outgoing-payments-table/outgoing-payments-table.component";
import { ExpensePaymentDetailDialogComponent } from "../expense-payment-detail-dialog/expense-payment-detail-dialog.component";
import { UserFacade } from "../../../../account/facades/user.facade";
import { CommentEditorComponent } from "../../../../comment/components/comment-editor/comment-editor.component";
import { UploadItem } from "../../../../../shared/components/kit-file-uploader/upload-item.model";
import { DOCUMENT_TYPE } from "../../../../../core/constants/document-types.constants";
import { FormControl } from "@angular/forms";
import { CommentDetailDto, CommentPayload } from "../../../../comment/models/comment.model";
import { FilePreviewService } from "../../../../files/file-preview.service";
import { UserOptionStore } from "../../../../account/options/user-dropdown.option";
import { CommentApiService } from "../../../../comment/services/comment-api.service";
import { CommentThreadComponent } from "../../../../comment/components/comment-thread/comment-thread.component";
import { FileApiService } from "../../../../files/file-api.service";

@Component({
      selector: 'outgoing-payment-detail-dialog',
      templateUrl: './outgoing-payment-detail-dialog.component.html',
      styleUrls: ['./outgoing-payment-detail-dialog.component.scss'],
      standalone: true,
      imports: [CommonModule, OutgoingPaymentStatusPipe, AvatarUrlPipe, KitLoadingSpinnerComponent, Kit404PageComponent, ExpensePaymentItemsTableComponent, OutgoingPaymentsTableComponent, CommentEditorComponent, CommentThreadComponent],
})
export class OutgoingPaymentDetailDialogComponent implements OnInit {
      private readonly matDialog = inject(MatDialog);
      private readonly dialogRef = inject(MatDialogRef<OutgoingPaymentDetailDialogComponent>);
      private readonly toastService = inject(ToastService);
      private readonly outgoingPaymentService = inject(OutgoingPaymentApiService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly outgoingPaymentLogic = useOutgoingPaymentDetail();
      private readonly filePreview = inject(FilePreviewService);
      private readonly fileApi = inject(FileApiService);
      private readonly userOptionsStore = inject(UserOptionStore);
      private readonly commentApi = inject(CommentApiService);

      loading = this.outgoingPaymentLogic.isLoading;
      error = this.outgoingPaymentLogic.error;
      submitting = false;
      currentUser$ = inject(UserFacade).currentUser$;
      outgoingPaymentId: string | null = null;

      constructor(@Inject(MAT_DIALOG_DATA) private data: string) {
            if(data) {
                  this.outgoingPaymentId = data;
                  this.outgoingPaymentLogic.load(data);
            }
      }

      ngOnInit(): void {
            this.getComments()
      }

      get outgoingPaymentDetail(): OutgoingPaymentDetailDto | null { 
            return this.outgoingPaymentLogic.outgoingPaymentDetail();
      }

      // === TAB NAVIGATION ===     
      activeTab: 'items' | 'outgoings' = 'items';
      setActiveTab(tab: 'items' | 'outgoings') {
            this.activeTab = tab;
      }

      async onApprove(): Promise<void> {
            this.submitting = true;
            try {
                  await firstValueFrom(this.outgoingPaymentService.approve(this.outgoingPaymentDetail!.id));
                  this.toastService.successRich("Duyệt khoản chi thành công");
                  this.outgoingPaymentLogic.refresh();

                  this.dialogRef.close(true);
            } catch (error) {
                  this.httpErrorHandler.handle(error, "Duyệt khoản chi thất bại");
            } finally {
                  this.submitting = false;
            }
      }

      async markCreated(): Promise<void> {
            this.submitting = true;
            try {
                  await firstValueFrom(this.outgoingPaymentService.markCreated(this.outgoingPaymentDetail!.id));
                  this.toastService.successRich("Đánh dấu khoản chi đã tạo thành công");
                  this.outgoingPaymentLogic.refresh();
                  this.dialogRef.close(true);
            } catch (error) {
                  console.error('Error marking outgoing payment as created', error);
                  this.httpErrorHandler.handle(error, "Tạo lệnh khoản chi thất bại");
            } finally {
                  this.submitting = false;
            }
      }

      close(isSuccess: boolean = false): void {
            this.dialogRef.close(isSuccess);
      }

      // ===== SUPPORT FUNCTION =====
      openOutgoingPaymentDetailDialog(exensePaymentId: string) {
            const dialogRef = this.matDialog.open(ExpensePaymentDetailDialogComponent, {
                  data: exensePaymentId,
            });
            
      }

      // ===== Comment =====
      readonly documentType = DOCUMENT_TYPE;
      module: string = "expense";
      entity: string ="outgoing-payment";
      isCommenting = false;
      isSubmittingComment = false;
      commentControl = new FormControl<string>('', { nonNullable: true });
      comments: CommentDetailDto[] = [];

      commentUploads: UploadItem[] = [];
      uploadMetaForComment = {
            module: 'expense',
            entity: 'comment-attachment',
            entityId: this.outgoingPaymentId,
      };

      async getComments() {
            if(this.outgoingPaymentId === null) {
                  this.toastService.warningRich("Không thể xác định định danh khoản chi");
                  return;
            } 
            this.comments = await firstValueFrom(this.commentApi.getComments(DOCUMENT_TYPE.OUTGOING_PAYMENT, this.outgoingPaymentId));
            console.log('comments: ', this.comments);
      }

      get canSubmitComment(): boolean {
            return !this.isSubmittingComment && this.commentControl.value.trim().length > 0;
      }

      async handleSubmitComment(event: { content: string; mentionLabels: string[]; uploads: UploadItem[] }) {
            try {
                  if(this.outgoingPaymentId === null) {
                        this.toastService.warningRich("Không thể xác định định danh khoản chi");
                        return;
                  } 
                  this.isSubmittingComment = true;

                  const { content, mentionLabels, uploads } = event;

                  // 1 ) Upload attachments
                  const uploadedIds = [];
                  for (const u of uploads) {
                        try {
                              const result = await firstValueFrom(
                                    this.fileApi.uploadFile(
                                          u.file,
                                          'core',
                                          'comment-attachment',
                                          this.outgoingPaymentDetail?.id
                                    )
                              );
                              if (result.data?.id) {
                                    uploadedIds.push(result.data.id);
                                    u.status = 'done';
                              }
                        } catch {
                              u.status = 'error';
                        }
                  }

                  // 2 ) Resolve mention IDs
                  const mentionIds = mentionLabels
                        .map(label => this.userOptionsStore.snapshot.find(u => u.label === label)?.id)
                        .filter(x => !!x) as string[];

                  // 3. Gửi comment
                  const payload: CommentPayload = {
                        documentType: DOCUMENT_TYPE.OUTGOING_PAYMENT,
                        documentId: this.outgoingPaymentId,
                        content,
                        attachmentIds: uploadedIds.length ? uploadedIds : undefined,
                        mentionIds: mentionIds.length ? mentionIds : undefined
                  };

                  const newComment = await firstValueFrom(this.commentApi.create(payload));
                  console.log('new comment: ', newComment);
                  this.comments.unshift(newComment);
                  this.commentControl.setValue('');
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Bình luận không thành công");
            } finally {
                  this.isSubmittingComment = false;
            }
      }

      cancelComment() {
            this.isCommenting = false;
            this.commentControl.setValue('', { emitEvent: false }); // clear value nhưng không trigger valueChanges → CommentMentionBox không xử lý lại
            this.commentUploads = [];
      }

      // ===== MENTION =====
      mentionLabels: string[] = [];
      onMentionsChanged(labels: string[]) {
            this.mentionLabels = labels;
      }

      // ===== REPLY COMMENT =====
      replyingToCommentId: string | null = null;
      isSubmittingReply = false;
      replyControl = new FormControl<string>('', { nonNullable: true });


      startReply(commentId: string | null) {
            this.replyingToCommentId = commentId;
            this.comments = [...this.comments];
      }
      
      async submitReply(event: { parentId: string; content: string; mentionLabels: string[]; uploads: UploadItem[]; }) {
            if(this.outgoingPaymentId === null) {
                  this.toastService.warningRich("Không thể xác định định danh khoản chi");
                        return;
            }

            const { parentId, content, mentionLabels, uploads } = event;

            try {
                  this.isSubmittingReply = true;

                  // 1) Upload files
                  const uploadedIds: string[] = [];
                  for (const u of uploads) {
                        try {
                              const result = await firstValueFrom(
                                    this.fileApi.uploadFile(
                                          u.file,
                                          'core',
                                          'comment-attachment',
                                          this.outgoingPaymentId,
                                    )
                              );

                              if (result.data?.id) {
                                    uploadedIds.push(result.data.id);
                                    u.status = 'done';
                              }
                        } catch {
                              u.status = 'error';
                        }
                  }

                  // 2) Resolve mention IDs
                  const mentionIds = mentionLabels
                        .map(label => this.userOptionsStore.snapshot.find(x => x.label === label)?.id)
                        .filter(id => !!id) as string[];

                  // 3) Send reply payload
                  const payload: CommentPayload = {
                        documentType: DOCUMENT_TYPE.OUTGOING_PAYMENT,
                        documentId: this.outgoingPaymentId,
                        content: content.trim(),
                        attachmentIds: uploadedIds.length ? uploadedIds : undefined,
                        mentionIds: mentionIds.length ? mentionIds : undefined
                  };

                  const newReply = await firstValueFrom(this.commentApi.reply(parentId, payload));

                  // add to tree
                  const parent = this.findCommentRecursive(this.comments, parentId);
                  parent!.replies = parent!.replies ?? [];
                  parent!.replies.push(newReply);

                  this.comments = [...this.comments];
                  this.replyingToCommentId = null;

            } catch (error) {
                  this.httpErrorHandler.handle(error, 'Gửi phản hồi thất bại');
            } finally {
                  this.isSubmittingReply = false;
            }
      }

      cancelReply() {
            this.replyingToCommentId = null;
            this.commentControl.setValue('');
            this.commentUploads = [];  
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