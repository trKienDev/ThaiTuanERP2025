import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
      selector: 'kit-spinner-button',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './kit-spinner-button.component.html',
      styleUrls: ['./kit-spinner-button.component.scss']
})
export class KitSpinnerButtonComponent {
      /** Kiểu nút: submit | button | reset */
      @Input() type: 'button' | 'submit' | 'reset' = 'button';

      /** Văn bản mặc định */
      @Input() text = 'Lưu';

      /** Văn bản khi đang loading */
      @Input() loadingText = 'Đang xử lý…';

      @Input() displayLoadingText: boolean = true;
      
      /** Đang loading hay không */
      @Input() loading = false;

      /** Vô hiệu hóa nút */
      @Input() disabled = false;
      
      /** Style tùy chọn (primary, secondary, danger...) */
      @Input() cssClass: string = 'save-button';
}
