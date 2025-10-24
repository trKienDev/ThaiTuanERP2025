import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
      selector: 'kit-overlay-spinner',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './kit-overlay-spinner.component.html',
      styleUrls: ['./kit-overlay-spinner.component.scss']
})
export class KitOverlaySpinnerComponent {
      @Input() show = false;
      @Input() message = 'Đang xử lý...';
}
