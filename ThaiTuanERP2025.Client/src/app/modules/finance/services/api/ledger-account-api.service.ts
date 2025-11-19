import { Injectable } from "@angular/core";
import { LedgerAccountDto, LedgerAccountPayload } from "../../models/ledger-account.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class LedgerAccountApiService extends BaseApiService<LedgerAccountDto, LedgerAccountPayload> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/ledger-account`);
      }

}