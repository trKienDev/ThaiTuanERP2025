import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
      name: 'invoiceStatus',
      standalone: true
})
export class InvoiceStatusPipe implements PipeTransform {
      transform(value: boolean): string {
            return value ? 'Nháp' : 'Đã tải';
      }
}