import { Component } from '@angular/core';

@Component({
  selector: 'app-unauthorized',
      template: `
            <div class="unauthorized">
            <h2>Không có quyền truy cập</h2>
            <p>Bạn không có quyền xem trang này.</p>
            </div>
      `,
      styles: [`
            .unauthorized {
                  text-align: center;
                  padding: 60px 20px;
                  color: #666;
            }
      `]
})
export class UnauthorizedComponent {}
