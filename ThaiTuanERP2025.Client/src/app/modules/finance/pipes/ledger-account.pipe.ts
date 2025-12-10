import { Pipe, PipeTransform } from "@angular/core";

@Pipe({ name: 'ledgerAccountBalanceType', standalone: true })
export class LedgerAccountBalanceKind implements PipeTransform {
      transform(kind: number | null | undefined): string {
            switch (kind) {
                  case 1: return 'Dư nợ';
                  case 2: return 'Dư có';
                  case 3: return 'Lưỡng tính';
                  case 5: return 'Không có số dư';
                  default: return 'Không có'
            }
      }
}
