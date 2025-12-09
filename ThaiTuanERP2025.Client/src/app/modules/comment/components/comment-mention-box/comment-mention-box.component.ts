import { CommonModule } from "@angular/common";
import { Component, inject, Input, OnDestroy, OnInit } from "@angular/core";
import { AbstractControl, FormControl, ReactiveFormsModule } from "@angular/forms";
import { combineLatest, map, Observable, startWith, Subject, takeUntil, tap } from "rxjs";
import { UserOptionStore } from "../../../account/options/user-dropdown.option";

export interface DropdownOption {
      id: string;
      label: string;
      imgUrl?: string;
}

@Component({
      selector: 'comment-mention-box',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule ],
      templateUrl: './comment-mention-box.component.html',
      styleUrls: ['./comment-mention-box.component.scss']
})
export class CommentMentionBoxComponent implements OnInit, OnDestroy {
      @Input({ required: true }) control!: FormControl<string | null> | AbstractControl<string | null>;

      private readonly destroy$ = new Subject<void>();

      filterControl = new FormControl('');
      mentionUsers$ = inject(UserOptionStore).option$;
      
      showMentionBox = false;
      currentKeyword = "";

      private mentionedSet = new Set<string>();  // Danh sách user đã được mention 

      filteredUsers$ = combineLatest([
            this.mentionUsers$,
            this.filterControl.valueChanges.pipe(startWith(''))
      ]).pipe(
            map(([users, keyword]) => {

                  // 1. loại bỏ các user đã được mention
                  let result = users.filter(u => !this.mentionedSet.has(u.label));

                  // 2. nếu không gõ filter thì trả về toàn bộ
                  const search = (keyword ?? '').trim().toLowerCase();
                  if (!search) return result;

                  // 3. filter theo text
                  return result.filter(u =>
                        u.label.toLowerCase().includes(search)
                  );
            })
      );

      ngOnInit(): void {
            if (!this.control) return;

            // detect khi user gõ '@'
            this.control.valueChanges
                  .pipe(takeUntil(this.destroy$))
                  .subscribe(text => {
                        const value = text ?? "";
                        const caret = this.getCaretPosition();

                        const keyword = this.extractMentionKeyword(value, caret);

                        if (keyword !== null) {
                              this.currentKeyword = keyword.toLowerCase();
                              this.showMentionBox = true;
                        } else {
                              this.currentKeyword = "";
                              this.showMentionBox = false;
                        }

                        this.updateFilteredUsers();
                  });
      }


      selectMention(user: DropdownOption) {
            const current = this.control.value ?? '';
            const mentionText = `@${user.label} `;

            this.mentionedSet.add(user.label);

            // thay ký tự '@' cuối cùng bằng @Full Name
            this.control.setValue(current.replace(/@$/, mentionText));

            this.showMentionBox = false;

            this.filterControl.setValue("");
      }

      ngOnDestroy(): void {
            this.destroy$.next();
            this.destroy$.complete();
      }

      private extractMentionKeyword(text: string, caretPos: number): string | null {
            // Lấy phần text trước caret
            const left = text.slice(0, caretPos);

            // Regex tìm từ gần nhất có dạng @abc
            const match = left.match(/@([A-Za-zÀ-ỹ0-9 ._-]*)$/);

            return match ? match[1] : null;
      }
      private getCaretPosition(): number {
            const input = document.activeElement as HTMLInputElement;
            return input?.selectionStart ?? 0;
      }
      private updateFilteredUsers() {
            this.filteredUsers$ = this.mentionUsers$.pipe(
                  map(users =>
                        users
                        .filter(u => !this.mentionedSet.has(u.label)) // loại bỏ user đã mention
                        .filter(u => u.label.toLowerCase().includes(this.currentKeyword))
                  )
            );
      }
}