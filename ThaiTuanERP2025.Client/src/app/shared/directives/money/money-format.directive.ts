import { Directive, ElementRef, HostListener, Input, OnDestroy } from "@angular/core";
import { NgControl } from "@angular/forms";
import { Subscription } from "rxjs";

@Directive({
      selector: '[appMoney]',
      standalone: true
})
export class MoneyFormatDirective implements OnDestroy {
      // Số chữ số thập phân muốn hiển thị (VND thường = 0)
      @Input('appMoneyDecimals') decimals = 0;

      private subscription?: Subscription;
      private composing = false;
      private updatingView = false;

      constructor(private element: ElementRef<HTMLInputElement>, private ngControl: NgControl) {
            // Nếu value thay đổi từ code, format lại display
            this.subscription = this.ngControl.control?.valueChanges.subscribe(v => {
                  if (this.element.nativeElement !== document.activeElement) {
                        // chạy sau khi Angular đã writeValue để không bị ghi đè
                        Promise.resolve().then(() => {
                              this.updatingView = true;                              // <-- bật cờ
                              this.element.nativeElement.value = this.formatFromNumber(v);
                              this.updatingView = false;                             // <-- tắt cờ
                        });
                  }
            });
      }

      ngOnDestroy(): void {
            this.subscription?.unsubscribe();
      }

      
      @HostListener('input', ['$event'])
      onInput(event: Event) {
            if (this.composing || this.updatingView) return;            // <-- CHẶN tái nhập

            const input = this.element.nativeElement;
            const prev = input.value;
            const selectionStart = input.selectionStart ?? prev.length;
            const unitsBeforeCaret = this.countUnits(prev.slice(0, selectionStart)); // Đếm "đơn vị đếm caret" (digit + dấu thập phân nếu cho phép) trước caret cũ

            const normalized = this.normalize(prev); // Chuẩn hóa chuỗi (chỉ còn số và (.) nếu decimals>0) + giới hạn số lẻ

            // Cập nhật FormControl với number
            const numeric = this.toNumber(normalized);
            this.ngControl.control?.setValue(numeric, { emitEvent: true });

            // 🔒 Chỉ set vào FormControl nếu giá trị THỰC SỰ khác,
            // tránh set đè trùng với DefaultValueAccessor
            const ctrl = this.ngControl.control;
            Promise.resolve().then(() => {
                  if (ctrl && ctrl.value !== numeric) {
                        ctrl.setValue(numeric, { emitEvent: true });
                  }
            });

            // Format hiển thị với dấu phẩy
            const formatted = this.formatFromString(normalized);

            this.updatingView = true;                                   // <-- bật cờ
            input.value = formatted;
            // Đặt lại caret dựa trên số "đơn vị" trước caret
            const newCaret = this.indexFromUnits(formatted, unitsBeforeCaret);
            input.setSelectionRange(newCaret, newCaret);
            this.updatingView = false;                                  // <-- tắt cờ
      }

      @HostListener('focus')
      onFocus() {
            const value = this.ngControl.control?.value;
            this.element.nativeElement.value = value ?? '';
            this.element.nativeElement.select();
      }

      @HostListener('blur')
      onBlur() {
            const current = Number(this.ngControl.control?.value ?? this.element.nativeElement.value);
            const rounded = (this.decimals === 0) ? Math.round(current) : Number(current.toFixed(this.decimals));

            this.ngControl.control?.setValue(rounded, { emitEvent: true });
            this.updatingView = true;                                   // <-- bật cờ

            this.element.nativeElement.value = this.formatFromNumber(rounded);
            this.updatingView = false;                                  // <-- tắt cờ
      }

      @HostListener('compositionstart') onCompStart() { this.composing = true; }
      @HostListener('compositionend') onCompEnd() { this.composing = false; }

      // ---------- Helpers ----------
      /** Loại bỏ mọi ký tự ngoài digit (và '.' nếu cho phép), cắt số lẻ theo this.decimals */
      private normalize(text: string): string {
            if (!text) return '';
            const allowDot = this.decimals > 0;

            // Giữ số và (tối đa một) dấu '.'
            let cleaned = text.replace(/[^\d.]/g, '');
            if (!allowDot) {
                  cleaned = cleaned.replace(/\./g, '');
                  return cleaned.replace(/^0+(\d)/, '$1'); // bỏ 0 đầu nếu dài
            }

            // Cho phép 1 dấu '.' và giới hạn số lẻ
            const firstDot = cleaned.indexOf('.');
            if (firstDot >= 0) {
                  const intPart = cleaned.slice(0, firstDot).replace(/\./g, '');
                  let fracPart = cleaned.slice(firstDot + 1).replace(/\./g, '');
                  if (this.decimals >= 0) 
                        fracPart = fracPart.slice(0, this.decimals);
                  cleaned = intPart + '.' + fracPart;
            } else {
                  cleaned = cleaned.replace(/\./g, '');
            }
            return cleaned.replace(/^0+(\d)/, '$1');
      }

      /** Chuyển chuỗi normalized -> number */
      private toNumber(normalized: string): number {
            if (!normalized) 
                  return 0;
            const n = Number(normalized);
            return isNaN(n) ? 0 : n;
      }

      /** Format từ number (sử dụng Intl). Với decimals=0 không có phần lẻ. */
      private formatFromNumber(val: unknown): string {
            const n = Number(val);
            if (isNaN(n)) return '';
            return new Intl.NumberFormat('en-US', {
                  minimumFractionDigits: this.decimals,
                  maximumFractionDigits: this.decimals,
            }).format(n);
      }

      /** Format từ chuỗi normalized (tự format phần nguyên, giữ nguyên phần lẻ) */
      private formatFromString(normalized: string): string {
            if (!normalized) return '';
            if (this.decimals === 0) {
                  const intOnly = normalized.replace(/\./g, '');
                  return this.groupThousands(intOnly);
            }
            const [i, f = ''] = normalized.split('.');
            const intGrouped = this.groupThousands(i);
            return f.length ? `${intGrouped}.${f}` : intGrouped;
      }

      /** Thêm dấu phẩy phần nguyên */
      private groupThousands(intStr: string): string {
            if (!intStr) 
                  return '0';
            return intStr.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
      }

      /** Đếm "đơn vị caret": digit + ('.' nếu decimals>0) trong chuỗi */
      private countUnits(s: string): number {
            const allowDot = this.decimals > 0;
            let cnt = 0;
            for (const ch of s) {
                  if (/\d/.test(ch) || (allowDot && ch === '.')) cnt++;
            }
            return cnt;
      }

      /** Tìm index trong formatted sao cho số "đơn vị" trước index khớp units */
      private indexFromUnits(formatted: string, units: number): number {
            if (units <= 0) 
                  return 0;

            const allowDot = this.decimals > 0;
            let cnt = 0;
            for (let i = 0; i < formatted.length; i++) {
                  const ch = formatted[i];
                  if (/\d/.test(ch) || (allowDot && ch === '.')) {
                        cnt++;
                        if (cnt === units) return i + 1;
                  }
            }
            return formatted.length;
      }
}