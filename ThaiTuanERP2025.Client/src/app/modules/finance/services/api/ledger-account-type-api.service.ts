import { Injectable } from "@angular/core";
import { LedgerAccountTypeDto, LedgerAccountTypePayload } from "../../models/ledger-account-type.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class LedgerAccountTypeApiService extends BaseApiService<LedgerAccountTypeDto, LedgerAccountTypePayload> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/ledger-account-type`);
      }
}