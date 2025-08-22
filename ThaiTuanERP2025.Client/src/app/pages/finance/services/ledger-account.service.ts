import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../core/services/api/base-crud.service";
import { CreateLedgerAccountRequest, LedgerAccountDto, LedgerAccountLookupDto, LedgerAccountTreeDto, UpdateLedgerAccountRequest } from "../models/ledger-account.model";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class LedgerAccountService extends BaseCrudService<LedgerAccountDto, CreateLedgerAccountRequest, UpdateLedgerAccountRequest> {
      private readonly base = `${environment.apiUrl}/ledger-accounts`;
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/ledger-accounts`);
      }

      lookup(keyword: string, take = 20): Observable<LedgerAccountLookupDto[]> {
            let params = new HttpParams().set('keyword', keyword).set('take', take);
            return this.http.get<ApiResponse<LedgerAccountLookupDto[]>>(`${this.endpoint}/lookup`, { params })
                  .pipe(handleApiResponse$<LedgerAccountLookupDto[]>(),
                  catchError(err => throwError(() => err))
            );
      }
}