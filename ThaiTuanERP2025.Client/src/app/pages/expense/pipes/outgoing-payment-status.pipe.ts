import { Pipe, PipeTransform } from "@angular/core";
import { OutgoingPaymentStatus } from "../models/outgoing-payment.model";

@Pipe({ name: 'outgoingPaymentStatus', standalone: true })
export class OutgoingPaymentStatusPipe implements PipeTransform {
  transform(value: string | number, mode: 'label' | 'class' = 'label'): string {
    const key = typeof value === 'number'
      ? OutgoingPaymentStatus[value]
      : value;

    const normalized = (key ?? '').toString();
    const pascal = normalized.charAt(0).toUpperCase() + normalized.slice(1);

    const STATUS_LABEL: Record<string, string> = {
      Pending: 'Chờ duyệt',
      Approved: 'Đã duyệt',
      Created: 'Đã tạo lệnh',
      Recorded: 'Đã ghi sổ',
      Cancelled: 'Đã hủy',
    };

    const STATUS_CLASS: Record<string, string> = {
      Pending: 'badge-pending',
      Approved: 'badge-approved',
      Created: 'badge-created',
      Recorded: 'badge-recorded',
      Cancelled: 'badge-cancelled',
    };

    return mode === 'class'
      ? STATUS_CLASS[pascal] ?? ''
      : STATUS_LABEL[pascal] ?? normalized;
  }
}
