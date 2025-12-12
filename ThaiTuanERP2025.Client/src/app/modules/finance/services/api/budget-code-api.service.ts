import { Injectable } from "@angular/core";
import { environment } from "../../../../../environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { BudgetCodeDto, BudgetCodeRequest, BudgetCodeWithAmountDto } from "../../models/budget-code.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class BudgetCodeApiService extends BaseApiService<BudgetCodeDto, BudgetCodeRequest > {
      constructor(http: HttpClient) {
            super(http, `${environment.server.apiUrl}/budget-code`);
      }

      getAllActive(): Observable<BudgetCodeDto[]> {
            return this.http.get<ApiResponse<BudgetCodeDto[]>>(`${this.endpoint}/active`)
                  .pipe(handleApiResponse$<BudgetCodeDto[]>());
      }

      getAvailable(): Observable<BudgetCodeDto[]> {
            return this.http.get<ApiResponse<BudgetCodeDto[]>>(`${this.endpoint}/available`)
                  .pipe(handleApiResponse$<BudgetCodeDto[]>());
      }

      getWithAmount(options?: { year?: number; month?: number; }): Observable<BudgetCodeWithAmountDto[]> {
            let params = new HttpParams();
            if (options?.year != null)  params = params.set('year', String(options.year));
            if (options?.month != null) params = params.set('month', String(options.month));

            return this.http.get<ApiResponse<BudgetCodeWithAmountDto[]>>(`${this.endpoint}/with-current-amount`, { params })
                  .pipe(handleApiResponse$<BudgetCodeWithAmountDto[]>());
      }
}