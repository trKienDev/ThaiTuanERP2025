import { inject, Injectable } from "@angular/core";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { LedgerAccountTypeDto, LedgerAccountTypePayload } from "../models/ledger-account-type.model";
import { LedgerAccountTypeApiService } from "../services/api/ledger-account-type-api.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class LedgerAccountTypeFacade extends BaseApiFacade<LedgerAccountTypeDto, LedgerAccountTypePayload> {
      constructor() {
            super(inject(LedgerAccountTypeApiService));
      }
      readonly ledgerAccountTypes$: Observable<LedgerAccountTypeDto[]> = this.list$;
}