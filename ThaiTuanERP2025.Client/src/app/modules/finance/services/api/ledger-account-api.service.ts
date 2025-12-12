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
            super(http, `${environment.server.apiUrl}/ledger-account`);
      }

      getTreeAsync(): Observable<LedgerAccountTreeDto[]> {
            return this.http.get<ApiResponse<LedgerAccountTreeDto[]>>(`${this.endpoint}/tree`)
                  .pipe(handleApiResponse$<LedgerAccountTreeDto[]>());
      }

      importExcel(file: File): Observable<void> {
            const formData = new FormData();
            formData.append("file", file);

            console.log('form: ', formData);

            return this.http.post<ApiResponse<void>>(`${this.endpoint}/excel`, formData)
                  .pipe(handleApiResponse$<void>());
      }
}