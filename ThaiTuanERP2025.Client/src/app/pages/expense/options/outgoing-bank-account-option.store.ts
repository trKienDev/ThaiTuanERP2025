import { inject, Injectable } from "@angular/core";
import { catchError, of, shareReplay } from "rxjs";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { OutgoingBankAccountFacade } from "../facades/outgoing-bank-account.facade";

@Injectable({ providedIn: 'root' })
export class OutgoingBankAccountOptionStore {
      private readonly facade = inject(OutgoingBankAccountFacade);

      readonly options$ = this.facade.outgoingBankAccounts$.pipe(
            toDropdownOptions({
                  id: 'id', 
                  label: 'name',
            }),
            catchError(() => of([])),
            shareReplay({ bufferSize: 1, refCount: false })
      )
}