import { inject, Injectable } from "@angular/core";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { OutgoingBankAccountDto, OutgoingBankAccountRequest } from "../models/outgoing-bank-account.model";
import { OutgoingBankAccountService } from "../services/outgoing-bank-account.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class OutgoingBankAccountFacade extends BaseCrudFacade<OutgoingBankAccountDto, OutgoingBankAccountRequest> {
      constructor() {
            super(inject(OutgoingBankAccountService));
      }
      readonly outgoingBankAccounts$: Observable<OutgoingBankAccountDto[]> = this.list$;
}