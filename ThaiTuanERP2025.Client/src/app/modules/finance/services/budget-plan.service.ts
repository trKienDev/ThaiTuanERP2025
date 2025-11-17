import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { BudgetPlanDto, BudgetPlanRequest } from "../models/budget-plan.model";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class BudgetPlanService extends BaseCrudService<BudgetPlanDto, BudgetPlanRequest>{
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-plan`);
      }

      getFollowing(budgetPeriodId: string): Observable<BudgetPlanDto[]> {
            return this.http.get<ApiResponse<BudgetPlanDto[]>>(`${this.endpoint}/following/${budgetPeriodId}`)
                  .pipe(handleApiResponse$<BudgetPlanDto[]>());
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