import { inject, Injectable } from "@angular/core";
import { CashoutGroupFacade } from "../facades/cashout-group.facade";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { shareReplay } from "rxjs";

@Injectable({ providedIn: 'root' })
export class CashoutGroupOptionStore {
      private readonly facade = inject(CashoutGroupFacade);

      readonly options$ = this.facade.cashoutGroups$.pipe(
            toDropdownOptions({
                  id: (b) => b.id,
                  label: (b) => b.name,
            }),
            shareReplay({ bufferSize: 1, refCount: false })
      );
}