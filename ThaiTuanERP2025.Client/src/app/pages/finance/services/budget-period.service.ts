import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { BudgetPeriodModel } from "../models/budget-period.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class BudgetPeriodService {
      private readonly API_URL = `${environment.apiUrl}/budget-period`;

      constructor(private http: HttpClient) {}

      getAll(): Observable<BudgetPeriodModel[]> {
            return this.http.get<ApiResponse<BudgetPeriodModel[]>>(`${this.API_URL}/all`)
                  .pipe(handleApiResponse$<BudgetPeriodModel[]>());
      }

      getAllActive(): Observable<BudgetPeriodModel[]> {
            return this.http.get<ApiResponse<BudgetPeriodModel[]>>(`${this.API_URL}/active`)
                  .pipe(handleApiResponse$<BudgetPeriodModel[]>());
      }

      create(payload: { year: number, month: number }): Observable<BudgetPeriodModel> {
            return this.http.post<ApiResponse<BudgetPeriodModel>>(this.API_URL, payload)
                  .pipe(handleApiResponse$<BudgetPeriodModel>());
      }

      updateStatus(id: string, isActive: boolean): Observable<void> {
            return this.http.put<ApiResponse<void>>(`${this.API_URL}/${id}`, {id, isActive })
                  .pipe(handleApiResponse$<void>());
      }
}