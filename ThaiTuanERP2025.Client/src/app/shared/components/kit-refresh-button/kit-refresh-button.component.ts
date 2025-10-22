import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
      selector: 'kit-refresh-button',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './kit-refresh-button.component.html',
      styleUrls: ['./kit-refresh-button.component.scss'],
})
export class KitRefreshButtonComponent {
      @Input() disabled = false;
      @Input() label = 'Làm mới';
      @Input() color = '#ff342b';
      @Output() clicked = new EventEmitter<void>();
}
