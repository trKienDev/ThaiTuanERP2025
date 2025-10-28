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
                  transition('closed <=> open', [animate('300ms ease')])
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
      @Input() multiple = false; // Bật chế độ chọn nhiều
      @Input() enableFilter = true;       /** Bật/tắt ô filter và placeholder của nó */
      @Input() filterPlaceholder = '🔎 Tìm...';
      @Input() caseSensitive = false;       /** Có phân biệt hoa/thường không */
      @Input() autoFocusFilter = true;       /** Khi mở menu, tự động focus vào ô filter */     

      @Output() selectionChange = new EventEmitter<KitDropdownOption>();
      @Output() selectionChangeMany = new EventEmitter<KitDropdownOption[]>();    // Output cho multi-select 

      isOpen = false;
      disabled = false;
      // ** Filter & Focus **
      filterText = '';  // Filter state
      focusedIndex = -1;

      // *** single ***
      private _value: string | null = null;
      selectedLabel: string | null = null;
      selectedImgUrl: string | null = null;

      // *** multiple ***
      private _values = new Set<string>();

      // ===== CVA =====
      private onChange: (val: any) => void = () => {};
      private onTouched: () => void = () => {};

      writeValue(val: string | string[] | null): void {
            if (this.multiple) {
                  this._values.clear();
                  if (Array.isArray(val)) val.forEach(v => this._values.add(v));
            } else {
                  this._value = (typeof val === 'string' ? val : null);
                  this.syncLabelFromValue();
                  this.eRef.nativeElement.classList.toggle('has-value', !!this._value);
            }
      }
      registerOnChange(fn: any): void { this.onChange = fn; }
      registerOnTouched(fn: () => void): void { this.onTouched = fn; }
      setDisabledState(isDisabled: boolean): void { this.disabled = isDisabled; }

      ngOnChanges(changes: SimpleChanges): void {
            if (changes['options']) {
                  if (!this.multiple) this.syncLabelFromValue();
            }
      }

      get computedWidth(): string | null {
            if(this.width === null || this.width === undefined) 
                  return null;
            return typeof this.width === 'number' ? `${this.width}px` : this.width;
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

      // ** Hiển thị label ô chọn **
      get selectedText(): string {
            if(!this.multiple) return this.selectedLabel ?? this.placeholder;
            const labels = this.options.filter(o => this._values.has(o.id)).map(o => o.label);
            if(labels.length === 0) return this.placeholder;
            if(labels.length <= 2) return labels.join(', ');
            return `${labels[0]}, ${labels[1]} +${labels.length - 2}`;
      }

      // ===== Toggle mở/đóng =====
      onToggle() {
            if (this.disabled) return;
            this.isOpen = !this.isOpen;
            if (this.isOpen) { 
                  this.focusedIndex = this.filteredOptions.length > 0 ? 0 : -1; // Reset vị trí focus theo danh sách đã filter
                  (this as any).eRef.nativeElement.focus();

                  // Chờ render xong rồi ensure visible + (tùy chọn) focus filter
                  setTimeout(() => {
                        this.ensureItemVisible();
                        if (this.enableFilter && this.autoFocusFilter) {
                              this.filterInput?.nativeElement.focus();
                              this.filterInput?.nativeElement.select();
                        }
                  });
            } else {
                  this.clearFilter();
            }
      }

      selectOption(opt: KitDropdownOption) {
            if(this.disabled) return;

            if(this.multiple) {
                  // toggle
                  if(this._values.has(opt.id)) this._values.delete(opt.id);
                  else this._values.add(opt.id);

                  // emit mảng id (CVA) + emit danh sách option (event)
                  this.onChange(Array.from(this._values));
                  this.onTouched();
                  this.selectionChange.emit(opt); // tùy bạn có dùng hay không
                  this.selectionChangeMany.emit(this.options.filter(o => this._values.has(o.id)));
                  // không đóng menu trong multi
                  return;
            }

            // single
            this._value = opt.id;
            this.selectedLabel = opt.label;
            this.selectedImgUrl = opt.imgUrl ?? null;
            this.isOpen = false;

            this.onChange(this._value);
            this.onTouched();
            this.selectionChange.emit(opt);
            this.clearFilter(); // Đóng menu rồi thì dọn filter
            this.eRef.nativeElement.classList.toggle('has-value', !!this._value);
      }

      /** Danh sách option đã chọn (giữ thứ tự theo options gốc) */
      get selectedOptions(): KitDropdownOption[] {
            return this.options.filter(o => this._values.has(o.id));
      }

      /** Xoá một lựa chọn khi đang ở multiple */
      remove(id: string) {
            if (!this.multiple || this.disabled) return;
            if (this._values.delete(id)) {
                  this.onChange(Array.from(this._values));       
                  this.onTouched();                // CVA: push mảng id
                  this.selectionChangeMany.emit(this.selectedOptions);           // emit option đã chọn (tuỳ bạn dùng)  
            }
      }

      /** (Tuỳ chọn) Xoá hết lựa chọn */
      clearAll() {
            if (!this.multiple || this.disabled) return;
            if (this._values.size === 0) return;
            this._values.clear();
            this.onChange([]);                                               // CVA: mảng rỗng
            this.selectionChangeMany.emit([]);                               // emit rỗng
      }

      // ===== Trạng thái selected cho item =====
      isSelected(id: string): boolean {
            return this.multiple ? this._values.has(id) : this._value === id;
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
                        this.selectOption(list[idx]); 
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

      @ViewChildren('optRef') optionItems!: QueryList<ElementRef<HTMLLIElement>>;
      @ViewChild('filterInput') filterInput?: ElementRef<HTMLInputElement>;

      constructor(private eRef: ElementRef<HTMLElement>) {}

      private ensureItemVisible() {
            const items = this.optionItems?.toArray();
            if (!items || this.focusedIndex < 0 || this.focusedIndex >= items.length) return;
            items[this.focusedIndex].nativeElement.scrollIntoView({ block: 'nearest' });
      }
      // Ngăn phím mũi tên trong ô filter “lọt” ra host (tránh cuộn focus)
      onFilterKeydown(ev: KeyboardEvent) { ev.stopPropagation(); }

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
