import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../core/services/api/base-crud.service";
import { CashOutCodeDto, CreateCashoutCodeRequest, UpdateCashoutCodeRequest } from "../../finance/models/cash-out-code.dto";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class CashOutCodeService extends BaseCrudService<CashOutCodeDto, CreateCashoutCodeRequest, UpdateCashoutCodeRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/cashout-codes`);
      }

      getByGroup(groupId: string): Observable<CashOutCodeDto[]> {
            const params = new HttpParams().set('groupId', groupId);
            return this.http
                  .get<ApiResponse<CashOutCodeDto[]>>(this.endpoint, { params })
                  .pipe(handleApiResponse$<CashOutCodeDto[]>());
      }
}