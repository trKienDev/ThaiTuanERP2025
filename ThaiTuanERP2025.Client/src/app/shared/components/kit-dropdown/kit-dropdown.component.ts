import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, forwardRef, HostListener, Input, OnChanges, Output, QueryList, SimpleChanges, ViewChildren } from '@angular/core';
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
      
      focusedIndex = -1;
      @ViewChildren('optRef') optionItems!: QueryList<ElementRef<HTMLLIElement>>;

      constructor(private eRef: ElementRef<HTMLElement>) {}

      // ===== CVA =====
      private onChange: (val: string | null) => void = () => {};
      private onTouched: () => void = () => {};

      writeValue(value: string | null): void {
            this._value = value;
            this.syncLabelFromValue();
      }

      registerOnChange(fn: (val: string | null) => void): void {
            this.onChange = fn;
      }

      registerOnTouched(fn: () => void): void {
            this.onTouched = fn;
      }

      setDisabledState(isDisabled: boolean): void {
            this.disabled = isDisabled;
      }

      // Đồng bộ label mỗi khi options thay đổi (ví dụ: load async)
      ngOnChanges(changes: SimpleChanges): void {
            if (changes['options']) this.syncLabelFromValue();
      }


      onToggle() {
            this.isOpen = !this.isOpen;
            if (this.isOpen) {
                  this.focusedIndex = this.options.length > 0 ? 0 : -1;
                  // focus vào host để nhận phím Arrow/Enter/Space
                  this.eRef.nativeElement.focus();
                  // chờ render xong rồi ensure visible
                  setTimeout(() => this.ensureItemVisible());
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
      }

      // Click ra ngoài thì đóng
      @HostListener('document:click', ['$event'])
            onClickOutside(event: Event) {
            if (this.isOpen && !this.eRef.nativeElement.contains(event.target as Node)) {
                  this.isOpen = false;
            }
      }

      // Nhấn ESC thì đóng (dùng document để vẫn bắt được nếu focus trôi)
      @HostListener('document:keydown.escape', ['$event'])
      onEsc(event: KeyboardEvent) {
            if (this.isOpen) {
                  this.isOpen = false;
                  event.stopPropagation();
            }
      }

      // 🔑 Điều hướng bàn phím — NGHE TRÊN HOST (khi host đã được focus)
      @HostListener('keydown', ['$event'])
      onKeydown(event: KeyboardEvent) {
            if (!this.isOpen) return;

            const len = this.options.length;
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
                  if (idx >= 0 && idx < len) this.selectOption(this.options[idx]);
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

      private ensureItemVisible() {
            const items = this.optionItems?.toArray();
            if (!items || this.focusedIndex < 0 || this.focusedIndex >= items.length) return;
            items[this.focusedIndex].nativeElement.scrollIntoView({ block: 'nearest' });
      }

      private syncLabelFromValue() {
            if(!this._value) {
                  this.selectedLabel = null;
                  return;
            }

            const opt = this.options.find(o => o.id === this._value);
            this.selectedLabel = opt ? opt.label : null;
      }
}
