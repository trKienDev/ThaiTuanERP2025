import { inject, Injectable } from "@angular/core";
import { LedgerAccountTypeFacade } from "../facades/ledger-account-type.facade";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { shareReplay } from "rxjs";

@Injectable({ providedIn: 'root' })
export class LedgerAccountTypeOptionStore {
      private readonly facade = inject(LedgerAccountTypeFacade);

      readonly options$ = this.facade.ledgerAccountTypes$.pipe(
            toDropdownOptions({
                  id: 'id',
                  label: (t) => `${t.name} - ${t.code}`,
            }),
            shareReplay({ bufferSize: 1, refCount: false })
      );
}