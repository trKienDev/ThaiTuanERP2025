import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../core/services/api/base-crud.service";
import { CreateLedgerAccountRequest, LedgerAccountDto, UpdateLedgerAccountRequest } from "../../finance/models/ledger-account.dto";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { catchError, map, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";
import { LedgerAccountTreeDto } from "../models/ledger-account-tree.dto";

@Injectable({ providedIn: 'root' })
export class LedgerAccountService extends BaseCrudService<LedgerAccountDto, CreateLedgerAccountRequest, UpdateLedgerAccountRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/ledger-accounts`);
      }

      getAllByType(typeId: string): Observable<LedgerAccountDto[]> {
            return this.http.get<ApiResponse<LedgerAccountDto[]>>(`${this.endpoint}`, {
                  params: { typeId }
            }).pipe(
                  handleApiResponse$<LedgerAccountDto[]>(),
                  catchError(err => throwError(() => err))
            );
      }

      getTreeByType(typeId: string): Observable<LedgerAccountTreeDto[]> {
            return this.http.get<ApiResponse<LedgerAccountTreeDto[]>>(`${this.endpoint}?typeId=${typeId}`).pipe(
                  map(res => res.data ?? [])
            );
      }

}