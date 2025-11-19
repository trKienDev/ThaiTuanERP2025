import { inject, Injectable } from "@angular/core";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { LedgerAccountDto, LedgerAccountPayload } from "../models/ledger-account.model";
import { LedgerAccountApiService } from "../services/api/ledger-account-api.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class LedgerAccountFacade extends BaseApiFacade<LedgerAccountDto, LedgerAccountPayload> {
      constructor() {
            super(inject(LedgerAccountApiService));
      }
      readonly ledgerAccounts$: Observable<LedgerAccountDto[]> = this.list$;
}