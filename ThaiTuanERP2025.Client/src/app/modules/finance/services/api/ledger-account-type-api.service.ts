import { Injectable } from "@angular/core";
import { LedgerAccountTypeDto, LedgerAccountTypeRequest } from "../../models/ledger-account-type.dto";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class LedgerAccountTypeApiService extends BaseApiService<LedgerAccountTypeDto, LedgerAccountTypeRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/ledger-account-types`);
      }
}