import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../core/services/api/base-crud.service";
import { CreateLedgerAccountRequest, LedgerAccountDto, UpdateLedgerAccountRequest } from "../../finance/models/ledger-account.dto";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";

@Injectable({ providedIn: 'root' })
export class LedgerAccountService extends BaseCrudService<LedgerAccountDto, CreateLedgerAccountRequest, UpdateLedgerAccountRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/ledger-accounts`);
      }
}