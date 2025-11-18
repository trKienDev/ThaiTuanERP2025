import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BudgetPlanDto, BudgetPlanRequest } from "../models/budget-plan.model";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";
import { BaseApiService } from "../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class BudgetPlanApiService extends BaseApiService<BudgetPlanDto, BudgetPlanRequest>{
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-plan`);
      }

      getFollowing(budgetPeriodId: string): Observable<BudgetPlansByDepartmentDto[]> {
            return this.http.get<ApiResponse<BudgetPlansByDepartmentDto[]>>(`${this.endpoint}/following/${budgetPeriodId}`)
                  .pipe(handleApiResponse$<BudgetPlansByDepartmentDto[]>());
      }

      updateAmount(budgetPeriodId: string, amount: number): Observable<void> {
            return this.http.put<ApiResponse<void>>(`${this.endpoint}/${budgetPeriodId}/amount`, amount)
                  .pipe(handleApiResponse$<void>());
      }

      markReview(id: string): Observable<void> {
            return this.http.post<ApiResponse<void>>(`${this.endpoint}/${id}/review`, null)
                  .pipe(handleApiResponse$<void>());
      }
}