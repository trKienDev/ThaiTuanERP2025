import { Injectable } from "@angular/core";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BudgetPlanDetailDto, BudgetPlanDto, BudgetPlanRequest } from "../../models/budget-plan.model";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class BudgetPlanApiService extends BaseApiService<BudgetPlanDto, BudgetPlanRequest>{
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-plan`);
      }

      getFollowing(budgetPeriodId: string): Observable<BudgetPlanDto[]> {
            return this.http.get<ApiResponse<BudgetPlanDto[]>>(`${this.endpoint}/following/${budgetPeriodId}`)
                  .pipe(handleApiResponse$<BudgetPlanDto[]>());
      }

      getAvailableDetails(): Observable<BudgetPlanDetailDto[]> {
            return this.http.get<ApiResponse<BudgetPlanDetailDto[]>>(`${this.endpoint}/available/details`)
                  .pipe(handleApiResponse$<BudgetPlanDetailDto[]>());
      }

      updateDetailAmount(planDetailId: string, amount: number): Observable<void> {
            return this.http.put<ApiResponse<void>>(`${this.endpoint}/detail/${planDetailId}/amount`, amount)
                  .pipe(handleApiResponse$<void>());
      }

      markReview(id: string): Observable<void> {
            return this.http.post<ApiResponse<void>>(`${this.endpoint}/${id}/review`, null)
                  .pipe(handleApiResponse$<void>());
      }

     approve(id: string): Observable<void> {
            return this.http.post<ApiResponse<void>>(`${this.endpoint}/${id}/approve`, null)
                  .pipe(handleApiResponse$<void>());
      }
}