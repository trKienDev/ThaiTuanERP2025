import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable, pipe } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { BudgetCodeModel, CreateBudgetCodeModel } from "../models/budget-code.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class BudgetCodeService {
      private readonly API_URL = `${environment.apiUrl}/budget-code`;
      constructor(private http: HttpClient) {}

      getAll(): Observable<BudgetCodeModel[]> {
            return this.http.get<ApiResponse<BudgetCodeModel[]>>(`${this.API_URL}/all`)
                  .pipe(handleApiResponse$<BudgetCodeModel[]>());
      }

      getAllActive(): Observable<BudgetCodeModel[]> {
            return this.http.get<ApiResponse<BudgetCodeModel[]>>(`${this.API_URL}/active`)
                  .pipe(handleApiResponse$<BudgetCodeModel[]>());
      }

      create(budgetCode: CreateBudgetCodeModel): Observable<BudgetCodeModel> {
            return this.http.post<ApiResponse<BudgetCodeModel>>(this.API_URL, budgetCode)
                  .pipe(handleApiResponse$<BudgetCodeModel>());
      } 

      updateStatus(id: string, isActive: boolean): Observable<void> {
            return this.http.put<ApiResponse<void>>(`${this.API_URL}/${id}/status`, isActive)
                  .pipe(handleApiResponse$<void>());
      }
}