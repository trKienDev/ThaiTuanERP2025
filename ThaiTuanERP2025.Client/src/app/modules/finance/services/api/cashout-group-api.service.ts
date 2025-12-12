import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { CashoutGroupDto, CashoutGroupPayload, CashoutGroupTreeDto } from "../../models/cashout-group.model";
import { BaseApiService } from "../../../../shared/services/base-api.service";
import { Observable } from "rxjs";
import { ApiResponse } from '../../../../shared/models/api-response.model';
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class CashoutGroupApiService extends BaseApiService<CashoutGroupDto, CashoutGroupPayload> {
      constructor(http: HttpClient) {
            super(http, `${environment.server.apiUrl}/cashout-group`);
      }

      getTree(): Observable<CashoutGroupTreeDto[]> {
            return this.http.get<ApiResponse<CashoutGroupTreeDto[]>>(`${this.endpoint}/tree`)
                  .pipe(handleApiResponse$<CashoutGroupTreeDto[]>());
      }
}