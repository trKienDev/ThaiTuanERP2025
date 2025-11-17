import { Injectable } from "@angular/core";
import { LedgerAccountDto, LedgerAccountLookupDto, LedgerAccountRequest } from "../../models/ledger-account.model";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class LedgerAccountApiService extends BaseApiService<LedgerAccountDto, LedgerAccountRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/ledger-accounts`);
      }

      lookup(keyword: string, take = 20): Observable<LedgerAccountLookupDto[]> {
            let params = new HttpParams().set('keyword', keyword).set('take', String(take));
            return this.http.get<ApiResponse<LedgerAccountLookupDto[]>>(`${this.endpoint}/lookup`, { params })
                  .pipe(handleApiResponse$<LedgerAccountLookupDto[]>(),
                  catchError(err => throwError(() => err))
            );
      }
}