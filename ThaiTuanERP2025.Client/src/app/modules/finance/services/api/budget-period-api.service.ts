import { Injectable } from "@angular/core";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BudgetPeriodDto, BudgetPeriodLookupDto, BudgetPeriodRequest, UpdateBudgetPeriodPayload } from "../../models/budget-period.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class BudgetPeriodApiService extends BaseApiService<BudgetPeriodDto, BudgetPeriodRequest, UpdateBudgetPeriodPayload, void> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-period`);
      }

      getAvailable() {
            return this.http.get<ApiResponse<BudgetPeriodLookupDto[]>>(`${this.endpoint}/available`)
                  .pipe(
                        handleApiResponse$<BudgetPeriodLookupDto[]>(),
                        catchError(err => throwError(() => err))
                  );
      }

      getYears() {
            return this.http.get<ApiResponse<number[]>>(`${this.endpoint}/years`)
                  .pipe(
                        handleApiResponse$<number[]>(),
                        catchError(err => throwError(() => err))
                  );
      }

      getForYear(year: number) {
            return this.http.get<ApiResponse<BudgetPeriodDto[]>>(`${this.endpoint}/year/${year}`)
                  .pipe(
                        handleApiResponse$<BudgetPeriodDto[]>(),
                        catchError(err => throwError(() => err))
                  )
      }

      createForYear(year: number) {
            return this.http.post<ApiResponse<string>>(`${this.endpoint}/year/${year}`, null)
                  .pipe(
                        handleApiResponse$<string>(),
                        catchError(err => throwError(() => err))
                  )
      }

      override update(id: string, payload: UpdateBudgetPeriodPayload): Observable<void> {
            return this.http.put<ApiResponse<void>>(`${this.endpoint}/${id}`, payload)
                  .pipe(
                        handleApiResponse$<void>(),
                        catchError(err => throwError(() => err))
                  )
      }
}