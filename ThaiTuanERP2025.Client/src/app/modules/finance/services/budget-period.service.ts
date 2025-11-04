import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { BudgetPeriodDto, BudgetPeriodRequest } from "../models/budget-period.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";
import { catchError, throwError } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";

@Injectable({ providedIn: 'root' })
export class BudgetPeriodService extends BaseCrudService<BudgetPeriodDto, BudgetPeriodRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-period`);
      }

      getForYear(year: number) {
            return this.http.get<ApiResponse<BudgetPeriodDto[]>>(`${this.endpoint}/for-year/${year}`)
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
}