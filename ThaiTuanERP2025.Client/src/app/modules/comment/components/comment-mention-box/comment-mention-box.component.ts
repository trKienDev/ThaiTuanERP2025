import { CommonModule } from "@angular/common";
import { Component, EventEmitter, inject, Input, OnDestroy, OnInit, Output } from "@angular/core";
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

      // [ Inputs/Outputs & Injections ]
      @Input({ required: true }) control!: FormControl<string | null> | AbstractControl<string | null>;
      mentionUsers$ = inject(UserOptionStore).option$;

      @Output() mentionsChange = new EventEmitter<string[]>();
      // ===================


      // [ Internal State ]
      private readonly destroy$ = new Subject<void>();
      private readonly mentionedSet = new Set<string>(); // Danh sách user đã được mention
      private allUsers: DropdownOption[] = [];

      filterControl = new FormControl('');
      showMentionBox = false;
      currentKeyword = "";
      activeIndex = 0;               // index đang được highlight
      currentUsers: DropdownOption[] = []; // danh sách user hiện tại để điều hướng
      // ==============================


      // [Lifecycle Hooks ]
      ngOnInit(): void {
            if (!this.control) return;

            // detect khi user gõ '@'
            this.control.valueChanges
                  .pipe(takeUntil(this.destroy$))
                  .subscribe(value => this.handleInputChanged(value ?? ""));

            this.mentionUsers$
                  .pipe(takeUntil(this.destroy$))
                  .subscribe(users => { this.allUsers = users; });
      }

      ngAfterViewInit() {
            document.addEventListener("keydown", this.handleKeydown);
      }

      ngOnDestroy(): void {
            document.removeEventListener("keydown", this.handleKeydown);
            this.destroy$.next();
            this.destroy$.complete();
      }
      // ================================


      // [ Main input handler ]
      private handleInputChanged(text: string): void {
            // cập nhật mentionedSet từ nội dung input
            this.syncMentionedSet(text);

            // xác định keyword người dùng đang nhập sau @
            const caret = this.getCaretPosition();
            const keyword = this.extractMentionKeyword(text, caret);

            if (keyword !== null) {
                  this.currentKeyword = keyword.toLowerCase();
                  this.showMentionBox = true;
            } else {
                  this.currentKeyword = "";
                  this.showMentionBox = false;
            }

            // cập nhật danh sách gợi ý
            this.updateFilteredUsers();
      }


      // [ Filtered input stream ]
      filteredUsers$ = combineLatest([
            this.mentionUsers$, 
            this.filterControl.valueChanges.pipe(startWith(''))
      ]).pipe(
            map(([users, keyword]) => { 
                  let result = this.filterOutMentioned(users); // loại bỏ các user đã được mention
                  const search = (keyword ?? '').trim().toLowerCase(); // nếu không gõ filter thì trả về toàn bộ

                  return search 
                        ? result.filter(u => u.label.toLowerCase().includes(search)) 
                        : result;
            })
      )


      // [ Select mention ]
      selectMention(user: DropdownOption) {
            const current = this.control.value ?? '';
            const mentionText = `@${user.label} `;

            // replace toàn bộ @keyword ở cuối chuỗi
            const newText = current.replace(/@[A-Za-zÀ-ỹ0-9 ._-]*$/, mentionText);

            this.control.setValue(newText);
            this.showMentionBox = false;
      }


      // [ Keyboard navigation ]
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


      // [ Sync mentioned set ]
      private syncMentionedSet(text: string) {
            this.mentionedSet.clear();

            // Regex bắt các mention đã "commit": @Full Name␣
            const regex = /@([A-Za-zÀ-ỹ0-9 ._-]+)\s/g;
            let match;

            while ((match = regex.exec(text)) !== null) {
                  const raw = match[1].trim(); 

                  const label = this.resolveMentionLabel(raw); 
                  if (label) {
                        this.mentionedSet.add(label);
                        this.mentionsChange.emit([...this.mentionedSet]);
                  }
            }
      }


      // [ Filtering logic for popup display ]
      private updateFilteredUsers() {
            this.filteredUsers$ = this.mentionUsers$.pipe(
                  map(users => this.filterOutMentioned(users).filter(u => u.label.toLowerCase().includes(this.currentKeyword))),
                  tap(list => {
                        this.currentUsers = list;
                        this.activeIndex = 0; // reset về item đầu tiên
                  })
            );
      }
      private filterOutMentioned(users: DropdownOption[]): DropdownOption[] {
            return users.filter(u => !this.mentionedSet.has(u.label));
      }


      // [ Mention label resolution ]
      private resolveMentionLabel(raw: string): string | null {
            const text = raw.trim();

            // ưu tiên label dài nhất (tránh nhầm với prefix ngắn)
            const candidates = this.allUsers
                  .filter(u => text.startsWith(u.label))
                  .sort((a, b) => b.label.length - a.label.length);

            return candidates.length > 0 ? candidates[0].label : null;
      }


      // [ Helper ]
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
}