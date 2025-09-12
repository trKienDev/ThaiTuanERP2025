import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, forwardRef, HostListener, Input, OnChanges, Output, QueryList, SimpleChanges, ViewChild, ViewChildren } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export type KitDropdownOption = { id: string; label: string, imgUrl?: string };

@Component({
      selector: 'kit-dropdown',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './kit-dropdown.component.html',
      styleUrl: './kit-dropdown.component.scss',
      animations: [
            trigger('slide', [
                  state('closed', style({ height: '0px', opacity: 0, overflow: 'hidden' })),
                  state('open',   style({ height: '*',   opacity: 1, overflow: 'hidden' })),
                  transition('closed => open', [ animate('300ms ease') ]),
                  transition('open => closed', [ animate('300ms ease') ]),
            ]),
      ],
      providers: [{
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => KitDropdownComponent),
            multi: true
      }]
})
export class KitDropdownComponent implements ControlValueAccessor, OnChanges {
      @Input() options: KitDropdownOption[] = [];
      @Input() placeholder = 'chọn ....';
      @Input() width: string | number | null = null;

      /** Bật/tắt ô filter và placeholder của nó */
      @Input() enableFilter = true;
      @Input() filterPlaceholder = 'Tìm...';
      /** Có phân biệt hoa/thường không */
      @Input() caseSensitive = false;
      /** Khi mở menu, tự động focus vào ô filter */
      @Input() autoFocusFilter = true;

      get computedWidth(): string | null {
            if(this.width === null || this.width === undefined) 
                  return null;
            return typeof this.width === 'number' ? `${this.width}px` : this.width;
      }
      @Output() selectionChange = new EventEmitter<KitDropdownOption>();

      isOpen = false;
      selectedLabel: string | null = null;
      selectedImgUrl: string | null = null;
      disabled = false;

      private _value: string | null = null;
      
      filterText = '';  // Filter state
      
      focusedIndex = -1;
      @ViewChildren('optRef') optionItems!: QueryList<ElementRef<HTMLLIElement>>;
      @ViewChild('filterInput') filterInput?: ElementRef<HTMLInputElement>;

      constructor(private eRef: ElementRef<HTMLElement>) {}

      // ===== CVA =====
      private onChange: (val: string | null) => void = () => {};
      private onTouched: () => void = () => {};

      writeValue(value: string | null): void {
            this._value = value;
            this.syncLabelFromValue();
      }

      registerOnChange(fn: (val: string | null) => void): void { this.onChange = fn; }
      registerOnTouched(fn: () => void): void { this.onTouched = fn; }
      setDisabledState(isDisabled: boolean): void { this.disabled = isDisabled; }

      // Đồng bộ label mỗi khi options thay đổi (ví dụ: load async)
      ngOnChanges(changes: SimpleChanges): void {
            if (changes['options']) this.syncLabelFromValue();
      }

      // ===== Filtering =====
      get filteredOptions(): KitDropdownOption[] {
            const text = this.caseSensitive ? this.filterText : this.filterText.toLowerCase();
            if (!this.enableFilter || !text) return this.options;
            return this.options.filter(o => {
                  const label = this.caseSensitive ? o.label : o.label.toLowerCase();
                  return label.includes(text);
            });
      }
      onFilterInput(event: Event) {
            const input = event.target as HTMLInputElement | null;
            this.filterText = input?.value ?? '';
      }
      clearFilter() {
            if (!this.enableFilter) return;
            this.filterText = '';
            // Sau khi xóa filter, đảm bảo focusedIndex hợp lệ
            this.focusedIndex = this.filteredOptions.length > 0 ? 0 : -1;
            this.ensureItemVisible();
      }
      onToggle() {
            if (this.disabled) return;
            this.isOpen = !this.isOpen;
            if (this.isOpen) {
                  // Reset vị trí focus theo danh sách đã filter
                  this.focusedIndex = this.filteredOptions.length > 0 ? 0 : -1;

                  // focus host để nghe phím mũi tên/enter/space
                  this.eRef.nativeElement.focus();

                  // Chờ render xong rồi ensure visible + (tùy chọn) focus filter
                  setTimeout(() => {
                        this.ensureItemVisible();
                        if (this.enableFilter && this.autoFocusFilter) {
                              this.filterInput?.nativeElement.focus();
                              this.filterInput?.nativeElement.select();
                        }
                  });
            } else {
                  // Đóng menu thì giữ lại filterText hay xóa? — ở đây xóa cho tiện
                  this.clearFilter();
            }
      }

      selectOption(opt: KitDropdownOption) {
            if(this.disabled) return;
            this._value = opt.id;
            this.selectedLabel = opt.label;
            this.selectedImgUrl = opt.imgUrl ?? null;
            this.isOpen = false;

            this.onChange(this._value);
            this.onTouched();
            this.selectionChange.emit(opt);

            this.clearFilter(); // Đóng menu rồi thì dọn filter
      }

      // Click ra ngoài thì đóng
      @HostListener('document:click', ['$event'])
            onClickOutside(event: Event) {
            if (this.isOpen && !this.eRef.nativeElement.contains(event.target as Node)) {
                  this.isOpen = false;
                  this.clearFilter();
            }
      }

      // Nhấn ESC thì đóng (dùng document để vẫn bắt được nếu focus trôi)
      @HostListener('document:keydown.escape', ['$event'])
      onEsc(event: KeyboardEvent) {
            if (this.isOpen) {
                  this.isOpen = false;
                  this.clearFilter();
                  event.stopPropagation();
            }
      }

      // 🔑 Điều hướng bàn phím — NGHE TRÊN HOST (khi host đã được focus)
      @HostListener('keydown', ['$event'])
      onKeydown(event: KeyboardEvent) {
            if (!this.isOpen) return;

            const list = this.filteredOptions;         // ⟵ dùng filteredOptions
            const len = list.length;
            if (len === 0) return;

            if (event.key === 'ArrowDown') {
                  event.preventDefault();
                  this.focusedIndex = (this.focusedIndex + 1 + len) % len;
                  this.ensureItemVisible();
                  return;
            }

            if (event.key === 'ArrowUp') {
                  event.preventDefault();
                  this.focusedIndex = (this.focusedIndex - 1 + len) % len;
                  this.ensureItemVisible();
                  return;
            }

            if (event.key === 'Enter' || event.key === ' ') {
                  event.preventDefault();
                  const idx = this.focusedIndex;

                  if (idx >= 0 && idx < len) 
                        this.selectOption(this.options[idx]);
                  return;
            }

            if (event.key === 'Home') {
                  event.preventDefault();
                  this.focusedIndex = 0;
                  this.ensureItemVisible();
                  return;
            }

            if (event.key === 'End') {
                  event.preventDefault();
                  this.focusedIndex = len - 1;
                  this.ensureItemVisible();
                  return;
            }
      }

      // Ngăn phím mũi tên trong ô filter “lọt” ra host (tránh cuộn focus)
      onFilterKeydown(ev: KeyboardEvent) {
            ev.stopPropagation();
      }

      private ensureItemVisible() {
            const items = this.optionItems?.toArray();
            if (!items || this.focusedIndex < 0 || this.focusedIndex >= items.length) return;
            items[this.focusedIndex].nativeElement.scrollIntoView({ block: 'nearest' });
      }

      private syncLabelFromValue() {
            if(!this._value) {
                  this.selectedLabel = null;
                  this.selectedImgUrl = null;
                  return;
            }

            const opt = this.options.find(o => o.id === this._value);
            this.selectedLabel = opt ? opt.label : null;
            this.selectedImgUrl = opt?.imgUrl ?? null;
      }
}
