// expense-payment-status.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';

export type PaymentStatusView = { text: string; class: string };

@Pipe({ name: 'expensePaymentStatus', standalone: true })
export class ExpensePaymentStatusPipe implements PipeTransform {
      transform(status: number | null | undefined): PaymentStatusView {
            switch (status) {
                  case 0: return { text: 'Nháp', class: 'draft' };
                  case 1: return { text: 'Đã gửi xử lý', class: 'submitted' };
                  case 2: return { text: 'Chờ duyệt', class: 'pending' };
                  case 3: return { text: 'Đã duyệt', class: 'approved' };
                  case 4: return { text: 'Chờ khoản tiền ra', class: 'outgoing-payment' };
                  case 5: return { text: 'Bị từ chối', class: 'rejected' };
                  case 6: return { text: 'Đã hủy', class: 'cancelled' };
                  default: return { text: 'Không xác định', class: 'unknown' }
            }
      }
}
