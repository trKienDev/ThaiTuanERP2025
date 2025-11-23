import { Injectable } from "@angular/core";
import { LedgerAccountTypeDto, LedgerAccountTypePayload } from "../../models/ledger-account-type.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { BaseApiService } from "../../../../shared/services/base-api.service";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class LedgerAccountTypeApiService extends BaseApiService<LedgerAccountTypeDto, LedgerAccountTypePayload> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/ledger-account-type`);
      }

      importExcel(file: File): Observable<void> {
            const formData = new FormData();
            formData.append("file", file);

            console.log('form: ', formData);

            return this.http.post<ApiResponse<void>>(`${this.endpoint}/excel`, formData)
                  .pipe(handleApiResponse$<void>());
      }
}