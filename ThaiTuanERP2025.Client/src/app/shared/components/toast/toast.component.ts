import { CommonModule } from "@angular/common";
import { Component, Inject } from "@angular/core";
import { MatIconModule } from "@angular/material/icon";
import { MAT_SNACK_BAR_DATA } from "@angular/material/snack-bar";

type ToastType = 'success' | 'error' | 'warning' | 'info';
export interface ToastData { type: ToastType; title?: string; message: string; durationMs?: number; }

@Component({
      standalone: true,
      selector: 'kit-toast',
      imports: [ CommonModule, MatIconModule ],
      templateUrl: './toast.component.html',
})
export class ToastComponent {
      constructor(
            @Inject(MAT_SNACK_BAR_DATA) public data: ToastData,
      ) {}
      
      get icon(): string {
            switch (this.data.type) {
                  case 'success': return 'check_circle';
                  case 'error':   return 'error';
                  case 'warning': return 'warning';
                  default: return 'info';
            }
      }

      get hasProgress(): boolean {
            console.log('progress: ', typeof this.data.durationMs === 'number' && this.data.durationMs > 0);
            return typeof this.data.durationMs === 'number' && this.data.durationMs > 0;
      }

      get progressDuration(): string | null {
            return this.hasProgress ? `${this.data.durationMs}ms` : null;
      }

}