import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { BudgetCodeModel, CreateBudgetCodeModel } from "../models/budget-code.model";

@Injectable({ providedIn: 'root' })
export class BudgetCodeService {
      private readonly API_URL = `${environment.apiUrl}/budget-code`;
      constructor(private http: HttpClient) {}

      getAll(): Observable<ApiResponse<BudgetCodeModel[]>> {
            return this.http.get<ApiResponse<BudgetCodeModel[]>>(`${this.API_URL}/all`);
      }

      create(budgetCode: CreateBudgetCodeModel): Observable<ApiResponse<BudgetCodeModel>> {
            return this.http.post<ApiResponse<BudgetCodeModel>>(this.API_URL, budgetCode);
      } 

      updateStatus(id: string, isActive: boolean): Observable<ApiResponse<void>> {
            return this.http.put<ApiResponse<void>>(`${this.API_URL}/${id}/status`, isActive);
      }
}