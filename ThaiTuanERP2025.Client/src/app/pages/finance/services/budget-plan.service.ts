import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BudgetPlanModel, CreateBudgetPlanModel } from "../models/budget-plan.model";
import { ApiResponse } from "../../../core/models/api-response.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class BudgetPlanService {
      private readonly API_URL = `${environment.apiUrl}/budget-plan`;
      constructor(private http: HttpClient) {}

      getAll(): Observable<BudgetPlanModel[]> {
            return this.http.get<ApiResponse<BudgetPlanModel[]>>(`${this.API_URL}/all`)
                  .pipe(handleApiResponse$<BudgetPlanModel[]>());
      } 

      create(plan: CreateBudgetPlanModel): Observable<BudgetPlanModel> {
            return this.http.post<ApiResponse<BudgetPlanModel>>(this.API_URL, plan)
                  .pipe(handleApiResponse$<BudgetPlanModel>());
      }

      update(id: string, plan: Partial<CreateBudgetPlanModel>): Observable<BudgetPlanModel> {
            return this.http.put<ApiResponse<BudgetPlanModel>>(`${this.API_URL}/${id}`, plan)
                  .pipe(handleApiResponse$<BudgetPlanModel>());
      }

      delete(id: string): Observable<ApiResponse<void>> {
            return this.http.delete<ApiResponse<void>>(`${this.API_URL}/${id}`);
      }
}