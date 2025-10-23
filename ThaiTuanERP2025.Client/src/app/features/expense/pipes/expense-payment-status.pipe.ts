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
                  case 4: return { text: 'Bị từ chối', class: 'rejected' };
                  case 5: return { text: 'Đã hủy', class: 'cancelled' };
                  case 6: return { text: 'Chờ khoản tiền ra', class: 'ready-for-payment' };
                  case 7: return { text: 'Tạo lệnh 1 phần', class: 'partially-paid' };
                  case 8: return { text: 'Tạo lệnh đầy đủ', class: 'fully-paid' };
                  default: return { text: 'Không xác định', class: 'unknown' }
            }
      }
}
