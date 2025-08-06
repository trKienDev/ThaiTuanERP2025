import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { catchError, map, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { BudgetGroupModel, CreateBudgetGroupModel } from "../models/budget-group.model";

@Injectable({ providedIn: 'root' })
export class BudgetGroupService {
      private readonly API_URL = `${environment.apiUrl}/budget-group`;
      constructor(private http: HttpClient) {}

      getAll(): Observable<ApiResponse<BudgetGroupModel[]>> {
            return this.http.get<ApiResponse<BudgetGroupModel[]>>(`${this.API_URL}/all`);
      }

      create(dto: CreateBudgetGroupModel): Observable<ApiResponse<BudgetGroupModel>> {
            return this.http.post<ApiResponse<BudgetGroupModel>>(this.API_URL, dto);
      }

      update(dto: BudgetGroupModel): Observable<ApiResponse<BudgetGroupModel>> {
            return this.http.put<ApiResponse<BudgetGroupModel>>(`${this.API_URL}/${dto.id}`, dto);
      }

      delete(id: string): Observable<ApiResponse<string>> {
            return this.http.delete<ApiResponse<string>>(`${this.API_URL}/${id}`);
      }
}