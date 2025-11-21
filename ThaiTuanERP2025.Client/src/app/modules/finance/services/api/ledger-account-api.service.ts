import { Injectable } from "@angular/core";
import { LedgerAccountDto, LedgerAccountPayload, LedgerAccountTreeDto } from "../../models/ledger-account.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { BaseApiService } from "../../../../shared/services/base-api.service";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class LedgerAccountApiService extends BaseApiService<LedgerAccountDto, LedgerAccountPayload> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/ledger-account`);
      }

      getTreeAsync(): Observable<LedgerAccountTreeDto[]> {
            return this.http.get<ApiResponse<LedgerAccountTreeDto[]>>(`${this.endpoint}/tree`)
                  .pipe(handleApiResponse$<LedgerAccountTreeDto[]>());
      }
}