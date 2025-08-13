import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { AbstractControl, NonNullableFormBuilder } from "@angular/forms";

@Component({
      selector: 'app-field-error',
      standalone: true,
      imports: [CommonModule],
      template: `
            <div class="alert-message danger-alert" role="alert" *ngIf="visible">
                  <ng-content></ng-content>
                  <button type="button" class="close-btn" (click)="closed=true" aria-label="Đóng">x</button>
            </div>
      `
})
export class FieldErrorComponent {
      @Input() control: AbstractControl | null = null;
      @Input() submitted = false;
      closed = false;

      get visible(): boolean {
            const c = this.control;
            if(!c || this.closed) return false;

            // Hiện lỗi nếu: (đã submit || user đã tương tác) & control invalid
            const interacted = c.touched || c.dirty;
            return (this.submitted || interacted) && c.invalid;
      }
}