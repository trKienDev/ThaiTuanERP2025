import { Directive, ElementRef, HostListener, Input, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

@Directive({
      selector: '[appMoney]',
      standalone: true,
      providers: [
            { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => MoneyFormatDirective), multi: true }
      ]
})
export class MoneyFormatDirective implements ControlValueAccessor {
      @Input('appMoneyDecimals') decimals = 0;

      private composing = false;
      private updatingView = false;
      private onChange: (value: number | null) => void = () => {};
      private onTouched: () => void = () => {};
      private disabled = false;

      constructor(private readonly element: ElementRef<HTMLInputElement>) {}

      // ===== ControlValueAccessor =====
      writeValue(value: any): void {
            if (this.element.nativeElement === document.activeElement) {
                  this.updatingView = true;
                  this.element.nativeElement.value = value ?? '';
                  this.updatingView = false;
            } else {
                  this.updatingView = true;
                  this.element.nativeElement.value = this.formatFromNumber(value);
                  this.updatingView = false;
            }
      }
      registerOnChange(fn: any): void { this.onChange = fn; }
      registerOnTouched(fn: any): void { this.onTouched = fn; }
      setDisabledState(isDisabled: boolean): void {
            this.disabled = isDisabled;
            this.element.nativeElement.disabled = isDisabled;
      }

      // ===== UI events =====
      @HostListener('input', ['$event'])
      onInput(_: Event) {
            if (this.composing || this.updatingView || this.disabled) return;

            const input = this.element.nativeElement;
            const prev = input.value;
            const selStart = input.selectionStart ?? prev.length;
            const unitsBefore = this.countUnits(prev.slice(0, selStart));

            const normalized = this.normalize(prev);
            const numeric = this.toNumber(normalized);

            this.onChange(numeric);

            const formatted = this.formatFromString(normalized);
            this.updatingView = true;
            input.value = formatted;
            const caretIndex = this.indexFromUnits(formatted, unitsBefore);
            input.setSelectionRange(caretIndex, caretIndex);
            this.updatingView = false;
      }

      @HostListener('focus')
      onFocus() {
            const current = this.element.nativeElement.value;
            const numeric = this.toNumber(this.normalize(current));
            this.updatingView = true;
            this.element.nativeElement.value = Number.isFinite(numeric) ? String(numeric) : '';
            this.element.nativeElement.select();
            this.updatingView = false;
      }

      @HostListener('blur')
      onBlur() {
            const raw = this.toNumber(this.normalize(this.element.nativeElement.value));
            const rounded = (this.decimals === 0) ? Math.round(raw) : Number(raw.toFixed(this.decimals));

            this.onChange(rounded);
            this.onTouched();

            this.updatingView = true;
            this.element.nativeElement.value = this.formatFromNumber(rounded);
            this.updatingView = false;
      }

      @HostListener('compositionstart') onCompStart() { this.composing = true; }
      @HostListener('compositionend') onCompEnd() { this.composing = false; }

      // ===== Helpers =====
      private normalize(text: string): string {
      if (!text) return '';
      const allowDot = this.decimals > 0;

      let cleaned = text.replace(/[^\d.]/g, '');
      if (!allowDot) {
            cleaned = cleaned.replace(/\./g, '');
            return cleaned.replace(/^0+(\d)/, '$1');
      }

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

      private toNumber(normalized: string): number {
            if (!normalized) return 0;
                  const n = Number(normalized);
            return isNaN(n) ? 0 : n;
      }

      private formatFromNumber(val: unknown): string {
            const n = Number(val);
            if (isNaN(n)) return '';
            return new Intl.NumberFormat('en-US', {
                  minimumFractionDigits: this.decimals,
                  maximumFractionDigits: this.decimals,
            }).format(n);
      }

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

      private groupThousands(intStr: string): string {
            if (!intStr) return '0';
            return intStr.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
      }

      private countUnits(s: string): number {
            const allowDot = this.decimals > 0;
            let cnt = 0;
            for (const ch of s) {
                  if (/\d/.test(ch) || (allowDot && ch === '.')) cnt++;
            }
            return cnt;
      }

      private indexFromUnits(formatted: string, units: number): number {
            if (units <= 0) return 0;

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