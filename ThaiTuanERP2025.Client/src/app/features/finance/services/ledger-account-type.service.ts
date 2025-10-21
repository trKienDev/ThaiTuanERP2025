import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { LedgerAccountTypeDto, LedgerAccountTypeRequest } from "../../finance/models/ledger-account-type.dto";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";

@Injectable({ providedIn: 'root' })
export class LedgerAccountTypeService extends BaseCrudService<LedgerAccountTypeDto, LedgerAccountTypeRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/ledger-account-types`);
      }
}