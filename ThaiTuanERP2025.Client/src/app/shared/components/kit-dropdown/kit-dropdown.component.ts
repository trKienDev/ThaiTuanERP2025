import { CommonModule } from '@angular/common';
import { booleanAttribute, Component, ElementRef, EventEmitter, forwardRef,HostBinding, HostListener, inject, Input, OnChanges, Output, QueryList, SimpleChanges, ViewChild, ViewChildren } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export interface KitDropdownOption<T = any> {
      id: T;
      label: string;
      imgUrl?: string;
}

@Component({
      selector: 'kit-dropdown',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './kit-dropdown.component.html',
      styleUrl: './kit-dropdown.component.scss',
      animations: [
            trigger('slide', [
                  state('closed', style({
                        height: '0px',
                        opacity: 0,
                        overflow: 'hidden'
                  })),
                  state('open', style({
                        height: '*',
                        opacity: 1
                  })),
                  transition('closed <=> open', [ animate('300ms ease') ])
            ])
      ],
      providers: [
            {
                  provide: NG_VALUE_ACCESSOR,
                  useExisting: forwardRef(() => KitDropdownComponent),
                  multi: true,
            },
      ],
})
export class KitDropdownComponent<T = any> implements ControlValueAccessor, OnChanges {
      // ===== Inputs / Outputs =====
      @Input() options: KitDropdownOption<T>[] = [];
      @Input() placeholder = 'chọn ....';
      @Input() width: string | number | null = null;
      @Input() multiple = false;
      @Input() enableFilter = true;
      @Input() filterPlaceholder = '🔎 Tìm...';
      @Input() caseSensitive = false;
      @Input() autoFocusFilter = true;
      @Input() required = false;
      @Input() visibleItemCount: number = 6;

      @Input({ transform: booleanAttribute }) invalid = false;
      @HostBinding('class.invalid') get hostInvalidClass() {
            return this.invalid;
      }

      @Output() selectionChange = new EventEmitter<KitDropdownOption<T>>();
      @Output() selectionChangeMany = new EventEmitter<KitDropdownOption<T>[]>();

      // ===== State =====
      isOpen = false;
      disabled = false;

      filterText = '';
      focusedIndex = -1;

      // Danh sách options sau filter (state thật)
      filteredOptions: KitDropdownOption<T>[] = [];

      // Single-value
      private _value: T | null = null;
      selectedLabel: string | null = null;
      selectedImgUrl: string | null = null;

      // Multi-value
      private readonly _values = new Set<T>();

      // CVA
      private onChange: (val: T | T[]) => void = () => {};
      private onTouched: () => void = () => {};

      // View refs
      @ViewChild('filterInput') filterInput!: ElementRef<HTMLInputElement>;
      @ViewChildren('optionRow') optionRows!: QueryList<ElementRef<HTMLLIElement>>;

      private readonly hostEl = inject(ElementRef<HTMLElement>);

      // ===== ControlValueAccessor =====
      writeValue(val: T | T[] | null | undefined): void {
            console.log("⛳ writeValue value =", val);
    console.log("⛳ writeValue multiple =", this.multiple);


            if (this.multiple) {
                  this._values.clear();
                  if (Array.isArray(val)) {
                        val.forEach((v) => this._values.add(v as T));
                  }
                  console.log("⛳ _values after patch =", Array.from(this._values));
            } else {
                  if (val === null || val === undefined) {
                        this._value = null;
                  } else {
                        this._value = val as T;
                  }
                  this.syncLabelFromValue();
            }
      }

      registerOnChange(fn: any): void {
            this.onChange = fn as (val: T | T[] | null) => void;
      }

      registerOnTouched(fn: () => void): void {
            this.onTouched = fn;
      }

      setDisabledState(isDisabled: boolean): void {
            this.disabled = isDisabled;
      }

      // ===== Lifecycle =====
      ngOnChanges(changes: SimpleChanges): void {
            if (changes['options']) {
                  // Khi options thay đổi: đồng bộ lại label + filteredOptions
                  this.syncLabelFromValue();
                  this.resetFilteredOptions();

                  this.syncSelectedOptionsForMultiple();
            }
      }

      // ===== Layout helpers =====
      get maxMenuHeight(): string {
            const itemHeight = 36;
            return `${this.visibleItemCount * itemHeight}px`;
      }

      get hasValue(): boolean {
            return this.multiple ? this._values.size > 0 : this._value !== null && this._value !== undefined;
      }

      get computedWidth(): string | null {
            if (this.width === null || this.width === undefined) return null;
            return typeof this.width === 'number' ? `${this.width}px` : this.width;
      }

      // ===== Filtering =====
      private resetFilteredOptions(): void {
            this.filteredOptions = [...this.options];
            this.focusedIndex = this.filteredOptions.length > 0 ? 0 : -1;
      }

      applyFilter(): void {
            if (!this.enableFilter) {
                  this.resetFilteredOptions();
                  return;
            }

            const raw = this.filterText.trim();
            const text = this.caseSensitive ? raw : raw.toLowerCase();

            if (text === '') {
                  this.resetFilteredOptions();
                  return;
            }

            this.filteredOptions = this.options.filter((opt) => {
                  const label = this.caseSensitive ? opt.label : opt.label.toLowerCase();
                  return label.includes(text);
            });

            if (this.filteredOptions.length === 0) {
                  this.focusedIndex = -1;
            } else {
                  this.focusedIndex = 0;
                  this.scrollToFocused();
            }
      }

      onFilterInput(event: Event): void {
            const input = event.target as HTMLInputElement | null;
            this.filterText = input?.value ?? '';
            this.applyFilter();
      }

      clearFilter(): void {
            if (!this.enableFilter) return;
            this.filterText = '';
            this.resetFilteredOptions();
      }

      // Ngăn phím trong ô filter “lọt” ra host
      onFilterKeydown(event: KeyboardEvent): void {
            const key = event.key;

            if (key === 'ArrowDown') {
                  event.preventDefault();
                  this.moveFocus(1);
            } else if (key === 'ArrowUp') {
                  event.preventDefault();
                  this.moveFocus(-1);
            } else if (key === 'Enter') {
                  event.preventDefault();
                  const opt = this.filteredOptions[this.focusedIndex];
                  if (opt) this.selectOption(opt);
            } else if (key === 'Escape') {
                  event.preventDefault();
                  this.close();
            }
      }

      // ===== Toggle mở/đóng =====
      toggle(): void {
            if (this.disabled) return;
            this.isOpen ? this.close() : this.open();
      }

      open(): void {
            if (this.disabled || this.isOpen) return;

            this.isOpen = true;
            this.applyFilter();

            this.hostEl.nativeElement.focus();

            setTimeout(() => {
                  if (this.enableFilter && this.autoFocusFilter && this.filterInput) {
                  this.filterInput.nativeElement.focus();
                  this.filterInput.nativeElement.select();
                  }
                  this.scrollToFocused();
            });
      }

      close(): void {
            if (!this.isOpen) return;

            this.isOpen = false;
            this.focusedIndex = -1;

            if (this.enableFilter) {
                  this.filterText = '';
                  this.resetFilteredOptions();
            }

            // Focus lại host để A11y ok
            setTimeout(() => {
                  this.hostEl.nativeElement.focus();
            });

            this.onTouched();
      }

      // ===== Selection =====
      selectOption(opt: KitDropdownOption<T>): void {
            if (this.disabled) return;

            if (this.multiple) {
                  if (this._values.has(opt.id)) this._values.delete(opt.id);
                  else this._values.add(opt.id);

                  const selectedIds = Array.from(this._values);
                  this.onChange(selectedIds);
                  this.onTouched();
                  this.selectionChange.emit(opt);
                  this.selectionChangeMany.emit(this.selectedOptions);

                  return;
            }

            // single
            this._value = opt.id;
            this.selectedLabel = opt.label;
            this.selectedImgUrl = opt.imgUrl ?? null;

            this.onChange(this._value);
            this.onTouched();
            this.selectionChange.emit(opt);

            this.close();
      }

      get selectedOptions(): KitDropdownOption<T>[] {
            const arr = this.options.filter((o) => this._values.has(o.id));
            console.log("🔥 selectedOptions =", arr);
            return arr;
      }

      remove(id: T): void {
            if (!this.multiple || this.disabled) return;
            if (this._values.delete(id)) {
                  const selectedIds = Array.from(this._values);
                  this.onChange(selectedIds);
                  this.onTouched();
                  this.selectionChangeMany.emit(this.selectedOptions);
            }
      }

      clearAll(): void {
            if (!this.multiple || this.disabled) return;
            if (this._values.size === 0) return;

            this._values.clear();
            this.onChange([]);
            this.onTouched();
            this.selectionChangeMany.emit([]);
      }

      isSelected(id: T): boolean {
            return this.multiple ? this._values.has(id) : this._value === id;
      }

      // ===== Keyboard & Outside click =====
      @HostListener('document:click', ['$event'])
            onDocumentClick(event: Event): void {
            if (!this.isOpen) return;
            const target = event.target as Node;
            if (!this.hostEl.nativeElement.contains(target)) {
                  this.close();
            }
      }

      @HostListener('keydown', ['$event'])
            onHostKeydown(event: KeyboardEvent): void {
            // nếu dropdown đang đóng → cho phép dùng phím mở
            if (!this.isOpen) {
                  if (event.key === 'Enter' || event.key === ' ' || event.key === 'ArrowDown') {
                        event.preventDefault();
                        this.open();
                  }
                  return;
            }

            const len = this.filteredOptions.length;
            if (len === 0) return;

            if (event.key === 'ArrowDown') {
                  event.preventDefault();
                  this.moveFocus(1);
                  return;
            }

            if (event.key === 'ArrowUp') {
                  event.preventDefault();
                  this.moveFocus(-1);
                  return;
            }

            if (event.key === 'Home') {
                  event.preventDefault();
                  this.focusedIndex = 0;
                  this.scrollToFocused();
                  return;
            }

            if (event.key === 'End') {
                  event.preventDefault();
                  this.focusedIndex = len - 1;
                  this.scrollToFocused();
                  return;
            }

            if (event.key === 'Enter') {
                  event.preventDefault();
                  const idx = this.focusedIndex;
                  if (idx >= 0 && idx < len) {
                        this.selectOption(this.filteredOptions[idx]);
                  }
                  return;
            }

            if (event.key === 'Escape') {
                  event.preventDefault();
                  this.close();
                  return;
            }
      }

      // ===== Focus helpers =====
      private scrollToFocused(): void {
            if (!this.optionRows || this.focusedIndex < 0) return;
            const arr = this.optionRows.toArray();
            if (this.focusedIndex >= arr.length) return;

            const el = arr[this.focusedIndex]?.nativeElement;
            if (el) {
                  el.scrollIntoView({ block: 'nearest' });
            }
      }

      private moveFocus(step: number): void {
            const len = this.filteredOptions.length;
            if (len === 0) return;

            this.focusedIndex = (this.focusedIndex + step + len) % len;
            this.scrollToFocused();
      }

      private syncLabelFromValue(): void {
            if (!this._value) {
                  this.selectedLabel = null;
                  this.selectedImgUrl = null;
                  return;
            }

            const opt = this.options.find((o) => o.id === this._value);
            this.selectedLabel = opt ? opt.label : null;
            this.selectedImgUrl = opt?.imgUrl ?? null;
      }

      private syncSelectedOptionsForMultiple(): void {
            if (!this.multiple) return;

            console.log("🔥 options changed! options =", this.options);
console.log("🔥 current _values =", Array.from(this._values));
            // dựa trên danh sách options mới, chọn lại những cái đã tồn tại trong _values
            const selected = this.options.filter(o => this._values.has(o.id));

            console.log("🔥 matched selected =", selected);

             // clear set cũ
            this._values.clear();

            // add lại từng id
            selected.forEach(s => this._values.add(s.id));
      }
}
