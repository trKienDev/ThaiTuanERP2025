import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { BudgetGroupModel, CreateBudgetGroupModel } from "../models/budget-group.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class BudgetGroupService {
      private readonly API_URL = `${environment.apiUrl}/budget-group`;
      constructor(private http: HttpClient) {}

      getAll(): Observable<BudgetGroupModel[]> {
            return this.http.get<ApiResponse<BudgetGroupModel[]>>(`${this.API_URL}/all`)
                  .pipe(handleApiResponse$<BudgetGroupModel[]>());
      }

      create(dto: CreateBudgetGroupModel): Observable<BudgetGroupModel> {
            return this.http.post<ApiResponse<BudgetGroupModel>>(this.API_URL, dto)
                  .pipe(handleApiResponse$<BudgetGroupModel>());
      }

      update(dto: BudgetGroupModel): Observable<BudgetGroupModel> {
            return this.http.put<ApiResponse<BudgetGroupModel>>(`${this.API_URL}/${dto.id}`, dto)
                  .pipe(handleApiResponse$<BudgetGroupModel>());
      }

      delete(id: string): Observable<string> {
            return this.http.delete<ApiResponse<string>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<string>());
      }
}