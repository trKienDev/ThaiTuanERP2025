import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { CashoutCodeDto, CreateCashoutCodeRequest, UpdateCashoutCodeRequest } from "../models/cashout-code.model";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class CashoutCodeService extends BaseCrudService<CashoutCodeDto, CreateCashoutCodeRequest, UpdateCashoutCodeRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/cashout-codes`);
      }

      getByGroup(groupId: string): Observable<CashoutCodeDto[]> {
            const params = new HttpParams().set('groupId', groupId);
            return this.http
                  .get<ApiResponse<CashoutCodeDto[]>>(this.endpoint, { params })
                  .pipe(handleApiResponse$<CashoutCodeDto[]>());
      }
}