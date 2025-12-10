import { inject, Injectable } from "@angular/core";
import { OutgoingBankAccountDto, OutgoingBankAccountPayload } from "../models/outgoing-bank-account.model";
import { Observable } from "rxjs";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { OutgoingBankAccountApiService } from "../services/api/outgoing-bank-account.service";

@Injectable({ providedIn: 'root' })
export class OutgoingBankAccountFacade extends BaseApiFacade<OutgoingBankAccountDto, OutgoingBankAccountPayload> {
      constructor() {
            super(inject(OutgoingBankAccountApiService));
      }
      readonly outgoingBankAccounts$: Observable<OutgoingBankAccountDto[]> = this.list$;
}