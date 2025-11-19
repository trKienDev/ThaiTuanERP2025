import { Pipe, PipeTransform } from "@angular/core";

@Pipe({ name: 'ledgerAccountTypeKind', standalone: true })
export class LedgerAccountTypeKind implements PipeTransform {
      transform(kind: number | null | undefined): string {
            switch (kind) {
                  case 0: return 'Tài sản';
                  case 1: return 'Nợ';
                  case 2: return 'Vốn chủ sở hữu';
                  case 4: return 'Doanh thu';
                  case 5: return 'Chi phí';
                  default: return 'Không có'
            }
      }
}
