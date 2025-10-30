import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
      selector: 'kit-404-page',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './kit-404-page.component.html',
      styleUrls: ['./kit-404-page.component.scss'],
})
export class Kit404PageComponent {
      @Input() code?: string; // ví dụ: '404', '500', 'ERR'
      @Input() title?: string;
      @Input() message?: string;
      @Input() primaryLabel?: string;
      @Input() secondaryLabel?: string;
      @Input() variant: 'error' | '404' = '404';

      @Output() primary = new EventEmitter<void>();
      @Output() secondary = new EventEmitter<void>();

      onPrimary() {
            this.primary.emit();
      }

      onSecondary() {
            this.secondary.emit();
      }
}
