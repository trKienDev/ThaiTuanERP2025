import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { OutgoingBankAccountDto, OutgoingBankAccountRequest } from "../models/outgoing-bank-account.model";
import { Injectable } from "@angular/core";
import { BaseApiService } from "../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class OutgoingBankAccountApiService extends BaseApiService<OutgoingBankAccountDto, OutgoingBankAccountRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/outgoing-bank-accounts`);
      }
}