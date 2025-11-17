import { Injectable } from "@angular/core";
import { CashoutCodeDto, CashoutCodeRequest } from "../../models/cashout-code.model";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class CashoutCodeApiService extends BaseApiService<CashoutCodeDto, CashoutCodeRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/cashout-code`);
      }

      getByGroup(groupId: string): Observable<CashoutCodeDto[]> {
            const params = new HttpParams().set('groupId', groupId);
            return this.http
                  .get<ApiResponse<CashoutCodeDto[]>>(this.endpoint, { params })
                  .pipe(handleApiResponse$<CashoutCodeDto[]>());
      }
}