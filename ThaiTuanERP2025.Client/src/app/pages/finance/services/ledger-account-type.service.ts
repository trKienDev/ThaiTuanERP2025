import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../core/services/api/base-crud.service";
import { CreateLedgerAccountTypeRequest, LedgerAccountTypeDto, UpdateLedgerAccountTypeRequest } from "../../finance/models/ledger-account-type.dto";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";

@Injectable({ providedIn: 'root' })
export class LedgerAccountTypeService extends BaseCrudService<LedgerAccountTypeDto, CreateLedgerAccountTypeRequest, UpdateLedgerAccountTypeRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/ledger-account-types`);
      }
}