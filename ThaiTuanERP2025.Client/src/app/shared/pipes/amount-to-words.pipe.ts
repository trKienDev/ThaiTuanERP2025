import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
      name: 'amountToWords',
      standalone: true
})
export class AmountToWordsPipe implements PipeTransform {
      transform(value: number | null | undefined): string {
            if (value == null || isNaN(value)) return '';
            if (value === 0) return '0';

            return this.formatCompactVietnamese(value);
      }

      private formatCompactVietnamese(num: number): string {
            const parts: string[] = [];

            // ====== Các mốc lớn ======
            const trillion = Math.floor(num / 1_000_000_000_000);                   // nghìn tỷ
            const billion  = Math.floor((num % 1_000_000_000_000) / 1_000_000_000); // tỷ
            const million  = Math.floor((num % 1_000_000_000) / 1_000_000);         // triệu
            const thousand = Math.floor((num % 1_000_000) / 1_000);                 // nghìn
            const hundred  = Math.floor((num % 1_000) / 100);                       // trăm

            if (trillion > 0) parts.push(`${trillion} nghìn tỷ`);
            if (billion > 0)  parts.push(`${billion} tỷ`);
            if (million > 0)  parts.push(`${million} triệu`);
            if (thousand > 0) parts.push(`${thousand} nghìn`);
            if (hundred > 0 && parts.length === 0) parts.push(`${hundred} trăm`);

            return parts.join(' ').trim();
      }
}