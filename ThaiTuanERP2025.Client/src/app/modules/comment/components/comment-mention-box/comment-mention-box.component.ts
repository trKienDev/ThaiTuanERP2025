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

      activeIndex = 0;               // index đang được highlight
      currentUsers: DropdownOption[] = []; // danh sách user hiện tại để điều hướng

      private mentionedSet = new Set<string>(); // Danh sách user đã được mention
      
      
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
                  return result.filter(u => u.label.toLowerCase().includes(search));
            })
      )

      ngOnInit(): void {
            if (!this.control) return;

            // detect khi user gõ '@'
            this.control.valueChanges.pipe(takeUntil(this.destroy$))
                  .subscribe(text => {
                        const value = text ?? "";

                        this.syncMentionedSet(value);

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

      ngAfterViewInit() {
      document.addEventListener("keydown", this.handleKeydown);
      }

      selectMention(user: DropdownOption) {
            const current = this.control.value ?? '';
            const mentionText = `@${user.label} `;

            // replace toàn bộ @keyword ở cuối chuỗi
            const newText = current.replace(/@[A-Za-zÀ-ỹ0-9 ._-]*$/, mentionText);

            this.control.setValue(newText);
            this.showMentionBox = false;
      }

      ngOnDestroy(): void {
            document.removeEventListener("keydown", this.handleKeydown);
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
      
      private updateFilteredUsers() {
            this.filteredUsers$ = this.mentionUsers$.pipe(
                  map(users => users
                        .filter(u => !this.mentionedSet.has(u.label)) // loại bỏ user đã mention
                        .filter(u => u.label.toLowerCase().includes(this.currentKeyword))
                  ),
                  tap(list => {
                        this.currentUsers = list;
                        this.activeIndex = 0; // reset về item đầu tiên
                  })
            );
      }

      private getCaretPosition(): number {
            const input = document.activeElement as HTMLInputElement;
            return input?.selectionStart ?? 0;
      }

      private handleKeydown = (event: KeyboardEvent) => {
            if (!this.showMentionBox || this.currentUsers.length === 0) return;

            switch (event.key) {
                  case "ArrowDown":
                        this.activeIndex = (this.activeIndex + 1) % this.currentUsers.length;
                        event.preventDefault();
                        break;

                  case "ArrowUp":
                        this.activeIndex =
                        (this.activeIndex - 1 + this.currentUsers.length) % this.currentUsers.length;
                        event.preventDefault();
                        break;

                  case "Enter":
                        const user = this.currentUsers[this.activeIndex];
                        if (user) {
                              this.selectMention(user);
                              event.preventDefault();
                        }
                        break;

                  default:
                        break;
            }
      };

      private syncMentionedSet(text: string) {
            this.mentionedSet.clear();

            // Regex bắt các mention đã "commit": @Full Name␣
            const regex = /@([A-Za-zÀ-ỹ0-9 ._-]+)\s/g;
            let match;

            while ((match = regex.exec(text)) !== null) {
                  const label = match[1].trim();
                  if (label) {
                        this.mentionedSet.add(label);
                  }
            }
      }
}