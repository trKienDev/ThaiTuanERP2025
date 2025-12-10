import { DOCUMENT_TYPE, DocumentTypeLiteral } from './../../../../core/constants/document-types.constants';
import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule, ReactiveFormsModule, FormControl } from "@angular/forms";
import { KitFileUploaderComponent } from "../../../../shared/components/kit-file-uploader/kit-file-uploader.component";
import { UploadItem } from "../../../../shared/components/kit-file-uploader/upload-item.model";
import { FilePreviewService } from "../../../files/file-preview.service";
import { CommentMentionBoxComponent } from "../comment-mention-box/comment-mention-box.component";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";

@Component({
      selector: 'comment-editor',
      standalone: true,
      imports: [CommonModule, FormsModule, ReactiveFormsModule, CommentMentionBoxComponent, KitFileUploaderComponent, KitSpinnerButtonComponent],
      templateUrl: './comment-editor.component.html',
      styleUrls: ['./comment-editor.component.scss'],
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class CommentEditorComponent {
      @Input({ required: true }) placeholder = 'Nhập bình luận...';
      @Input({ required: true }) documentType!: DocumentTypeLiteral;
      @Input({ required: true }) documentId!: string;
      @Input() loading = false;

      // Mode: "comment" hoặc "reply" → để cha biết đang xử lý gì
      @Input() mode: 'comment' | 'reply' = 'comment';

      isCommenting = false;

      // ---- OUTPUT ----
      @Output() submitted = new EventEmitter<{
            content: string;
            mentionLabels: string[];
            uploads: UploadItem[];
      }>();

      @Output() canceled = new EventEmitter<void>();

      // ---- Form control chính ----
      @Input() control = new FormControl<string>('', { nonNullable: true });

      // ---- Mention ----
      mentionLabels: string[] = [];
      onMentionsChanged(labels: string[]) {
            this.mentionLabels = labels;
      }

      // ---- Uploads ----
      uploads: UploadItem[] = [];

      uploadMeta = {
            module: 'core',
            entity: 'comment-attachment',
            entityId: undefined as string | undefined,
            isPublic: false
      };

      constructor(private readonly filePreview: FilePreviewService) {}

      get canSubmit(): boolean {
            return this.control.value.trim().length > 0;
      }

      previewLocal(file: File) {
            this.filePreview.previewLocalFile(file);
      }

      onSubmit() {
            this.submitted.emit({
                  content: this.control.value.trim(),
                  mentionLabels: this.mentionLabels,
                  uploads: this.uploads
            });
      }

      onCancel() {
            this.isCommenting = false;
            this.control.setValue('', { emitEvent: false });
            this.uploads = [];
            this.canceled.emit();
      }
}
