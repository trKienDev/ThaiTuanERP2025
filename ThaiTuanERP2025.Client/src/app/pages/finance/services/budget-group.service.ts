import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { catchError, map, Observable, throwError } from "rxjs";
import { BudgetGroupDto, CreateBudgetGroupDto } from "../dtos/budget-group.dto";
import { ApiResponse } from "../../../core/models/api-response.model";

@Injectable({ providedIn: 'root' })
export class BudgetGroupService {
      private readonly API_URL = `${environment.apiUrl}/budget-group`;
      constructor(private http: HttpClient) {}

      getAll(): Observable<ApiResponse<BudgetGroupDto[]>> {
            return this.http.get<ApiResponse<BudgetGroupDto[]>>(`${this.API_URL}/all`);
      }

      create(dto: CreateBudgetGroupDto): Observable<ApiResponse<BudgetGroupDto>> {
            return this.http.post<ApiResponse<BudgetGroupDto>>(this.API_URL, dto);
      }

      update(dto: BudgetGroupDto): Observable<ApiResponse<BudgetGroupDto>> {
            return this.http.put<ApiResponse<BudgetGroupDto>>(`${this.API_URL}/${dto.id}`, dto);
      }

      delete(id: string): Observable<ApiResponse<string>> {
            return this.http.delete<ApiResponse<string>>(`${this.API_URL}/${id}`);
      }
}