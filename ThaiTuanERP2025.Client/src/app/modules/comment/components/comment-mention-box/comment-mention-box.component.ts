import { CommonModule } from "@angular/common";
import { Component, inject, Input, OnDestroy, OnInit } from "@angular/core";
import { AbstractControl, FormControl } from "@angular/forms";
import { Subject, takeUntil } from "rxjs";
import { UserOptionStore } from "../../../account/options/user-dropdown.option";

export interface DropdownOption {
      id: string;
      label: string;
      imgUrl?: string;
}

@Component({
      selector: 'comment-mention-box',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './comment-mention-box.component.html',
      styleUrls: ['./comment-mention-box.component.scss']
})
export class CommentMentionBoxComponent implements OnInit, OnDestroy {
      @Input({ required: true }) control!: FormControl<string | null> | AbstractControl<string | null>;

      private readonly destroy$ = new Subject<void>();

      // lấy user options từ UserOptionStore
      mentionUsers$ = inject(UserOptionStore).option$;

      showMentionBox = false;

      ngOnInit(): void {
            if (!this.control) return;

            this.control.valueChanges.pipe(takeUntil(this.destroy$))
                  .subscribe(value => {
                        const text = value ?? '';

                        // nếu ký tự cuối cùng là '@' → mở box
                        if (text.endsWith('@')) {
                              this.showMentionBox = true;
                        }

                        // nếu không còn '@' nào → tắt box
                        if (!text.includes('@')) {
                              this.showMentionBox = false;
                        }
                  }
            );
      }

      selectMention(user: DropdownOption) {
            const current = this.control.value ?? '';
            const mentionText = `@${user.label} `;

            // thay ký tự '@' cuối cùng bằng @Full Name
            this.control.setValue(current.replace(/@$/, mentionText));

            this.showMentionBox = false;
      }

      ngOnDestroy(): void {
            this.destroy$.next();
            this.destroy$.complete();
      }
}