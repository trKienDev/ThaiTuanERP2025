import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { BudgetPeriodModel } from "../models/budget-period.model";

@Injectable({ providedIn: 'root' })
export class BudgetPeriodService {
      private readonly API_URL = `${environment.apiUrl}/budget-period`;

      constructor(private http: HttpClient) {}

      getAll(): Observable<ApiResponse<BudgetPeriodModel[]>> {
            return this.http.get<ApiResponse<BudgetPeriodModel[]>>(`${this.API_URL}/all`);
      }

      create(payload: { year: number, month: number }): Observable<ApiResponse<BudgetPeriodModel>> {
            return this.http.post<ApiResponse<BudgetPeriodModel>>(this.API_URL, payload );
      }

      updateStatus(id: string, isActive: boolean): Observable<ApiResponse<void>> {
            return this.http.put<ApiResponse<void>>(`${this.API_URL}/${id}`, {id, isActive });
      }
}