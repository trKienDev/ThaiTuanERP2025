import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { OutgoingBankAccountDto, OutgoingBankAccountRequest } from "../models/outgoing-bank-account.model";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class OutgoingBankAccountService extends BaseCrudService<OutgoingBankAccountDto, OutgoingBankAccountRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/outgoing-bank-accounts`);
      }
}