import { Pipe, PipeTransform } from "@angular/core";

export type PaymentStatusView = { text: string; class: string };

@Pipe({ name: 'outgoingPaymentStatus', standalone: true })
export class OutgoingPaymentStatusPipe implements PipeTransform {
      transform(status: number | null | undefined): PaymentStatusView {
            switch (status) {
                  case 0: return { text: 'Chờ duyệt', class: 'pending' };
                  case 1: return { text: 'Đã duyệt', class: 'approved' };
                  case 2: return { text: 'Đã tạo lệnh', class: 'created' };
                  case 5: return { text: 'Đã hủy', class: 'cancelled' };
                  default: return { text: 'Không xác định', class: 'unknown' }
            }
      }
}
