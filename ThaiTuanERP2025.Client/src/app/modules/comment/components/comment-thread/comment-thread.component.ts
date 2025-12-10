import { CommonModule } from "@angular/common";
import { Component, Input, Output, EventEmitter, ViewChild, ElementRef, ChangeDetectionStrategy, ChangeDetectorRef, inject } from "@angular/core";
import { FormControl, ReactiveFormsModule } from "@angular/forms";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";
import { CommentDetailDto } from "../../models/comment.model";
import { CommentStateService } from "../../services/comment-state.service";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { FilePreviewService, StoredFileMetadataDto } from "../../../files/file-preview.service";
import { MentionHighlightPipe } from "../../pipes/comment-mention-highlight.pipe";
import { CommentMentionBoxComponent } from "../comment-mention-box/comment-mention-box.component";
import { KitFileUploaderComponent } from "../../../../shared/components/kit-file-uploader/kit-file-uploader.component";
import { CommentEditorComponent } from "../comment-editor/comment-editor.component";
import { DocumentTypeLiteral } from "../../../../core/constants/document-types.constants";
import { UploadItem } from "../../../../shared/components/kit-file-uploader/upload-item.model";

@Component({
      selector: 'comment-thread',
      standalone: true,
      imports: [CommonModule, AvatarUrlPipe, ReactiveFormsModule, KitSpinnerButtonComponent, MentionHighlightPipe, CommentMentionBoxComponent, KitFileUploaderComponent, CommentEditorComponent],
      templateUrl: './comment-thread.component.html',
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class CommentThreadComponent {

      @Input() comment!: CommentDetailDto;
      @Input() depth = 0;
      @Input() replyingToCommentId: string | null = null;
      @Input() replyLoading = false;
      @Input({ required: true }) documentType!: DocumentTypeLiteral;
      
      @Output() replyRequest = new EventEmitter<string | null>();
      @Output() submitReply = new EventEmitter<{
            parentId: string;
            content: string;
            mentionLabels: string[];
            uploads: UploadItem[];
      }>();
      @Output() cancelReply = new EventEmitter<void>();

      private readonly state = inject(CommentStateService);
      private readonly changeDetector = inject(ChangeDetectorRef);
      private readonly toast = inject(ToastService);
      private readonly filePreview = inject(FilePreviewService);

      replyControl = new FormControl('', { nonNullable: true });

      get canSubmitReply(): boolean {
            return this.replyControl.value.trim().length > 0;
      }

      trackById(index: number, item: CommentDetailDto) {
            return item.id;
      }

      /** Comment này có đang mở reply không */
      get expanded(): boolean { return this.state.isExpanded(this.comment.id); }

      /** Toggle mở/đóng toàn bộ cây con */
      toggleReplies() {
            if (this.expanded)
                  this.state.collapseRecursive(this.comment);
            else
                  this.state.expandRecursive(this.comment);

            this.changeDetector.markForCheck();
      }

      @ViewChild('replyInput') replyInput!: ElementRef<HTMLInputElement>;


      ngAfterViewInit() {
            if (this.replyingToCommentId === this.comment.id) {
                  setTimeout(() => this.replyInput?.nativeElement?.focus(), 50);
            }
      }

      onReply() {
            this.replyRequest.emit(this.comment.id);
      }

      onSubmitReply(event: { content: string; mentionLabels: string[]; uploads: UploadItem[] }) {
            this.submitReply.emit({
                  parentId: this.comment.id,
                  content: event.content,
                  mentionLabels: event.mentionLabels,
                  uploads: event.uploads
            });

            this.replyControl.setValue('');
      }

      onCancelReply() {
            this.replyControl.setValue('');
            this.cancelReply.emit();
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
            })
      }

}