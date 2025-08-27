import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
      selector: 'app-alert-message',
      standalone: true,
      imports: [CommonModule],
      template: `
            <p class="alert-message" [ngClass]="type" role="alert" *ngIf="visible">
                  <ng-content></ng-content>
                  <button type="button" class="close-btn" (click)="close()" aria-label="Đóng">×</button>
            </p>
      `
})
export class AlertMesssageComponent {
      @Input() visible = false;
      @Input() type: 'danger-alert' | 'infor-alert' | 'success-alert' = 'danger-alert';
      @Output() closed = new EventEmitter<void>();

      close() {
            this.visible = false;
            this.closed.emit();
      }
}