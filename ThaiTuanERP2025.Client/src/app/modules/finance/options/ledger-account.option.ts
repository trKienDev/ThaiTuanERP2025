import { inject, Injectable } from "@angular/core";
import { LedgerAccountFacade } from "../facades/ledger-account.facade";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { shareReplay } from "rxjs";

@Injectable({ providedIn: 'root' })
export class LedgerAccountOptionStore {
      private readonly facade = inject(LedgerAccountFacade);

      readonly options$ = this.facade.ledgerAccounts$.pipe(
            toDropdownOptions({
                  id: 'id',
                  label: (b) => `(${b.number}) - ${b.name}`,
            }),
            shareReplay({ bufferSize: 1, refCount: false })
      );
}