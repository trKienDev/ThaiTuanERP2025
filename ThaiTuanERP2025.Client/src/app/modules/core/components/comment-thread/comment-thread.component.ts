import { CommonModule } from "@angular/common";
import { Component, Input, Output, EventEmitter, ViewChild, ElementRef } from "@angular/core";
import { FormControl, ReactiveFormsModule } from "@angular/forms";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";
import { CommentDetailDto } from "../../models/comment.model";

@Component({
      selector: 'comment-thread',
      standalone: true,
      imports: [CommonModule, AvatarUrlPipe, ReactiveFormsModule, KitSpinnerButtonComponent],
      templateUrl: './comment-thread.component.html',
})
export class CommentThreadComponent {
      @Input() comment!: CommentDetailDto;
      @Input() depth = 0;
      @Input() replyingToCommentId: string | null = null;

      @Output() reply = new EventEmitter<string | null>();
      @Output() submitReply = new EventEmitter<{ parentId: string; content: string }>();
      @Output() cancelReply = new EventEmitter<void>();

      replyControl = new FormControl('', { nonNullable: true });

      @ViewChild('replyInput') replyInput!: ElementRef<HTMLInputElement>;

      ngAfterViewInit(): void {
            if (this.replyingToCommentId === this.comment.id) {
                  setTimeout(() => this.replyInput?.nativeElement?.focus(), 50);
            }
      }

      onReply() {
            this.reply.emit(this.comment.id);
      }
      onSubmitReply() {
            const content = this.replyControl.value.trim();
            if (!content) return;

            this.submitReply.emit({ parentId: this.comment.id, content });
            this.replyControl.setValue('');
      }

      onCancel() {
            this.cancelReply.emit();
      }

      showReplies = false;
      toggleReplies() {
            this.showReplies = !this.showReplies;
      }
}