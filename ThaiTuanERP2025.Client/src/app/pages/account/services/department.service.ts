import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";
import { DepartmentDto } from "../models/department.model";

@Injectable({ providedIn: 'root' }) 
export class DepartmentService {
      private readonly API_URL = `${environment.apiUrl}/department`;
      constructor(private http: HttpClient) {}

      getAll(): Observable<DepartmentDto[]> {
            return this.http.get<ApiResponse<DepartmentDto[]>>(`${this.API_URL}/all`)
                  .pipe(handleApiResponse$<DepartmentDto[]>());
      }

      getByIds(ids: string[]): Observable<DepartmentDto[]> {
            return this.http.post<ApiResponse<DepartmentDto[]>>(`${this.API_URL}/by-ids`, ids)
                  .pipe(handleApiResponse$<DepartmentDto[]>());
      } 

      create(dept: DepartmentDto): Observable<string> {
            return this.http.post<ApiResponse<string>>(this.API_URL, dept)
                  .pipe(handleApiResponse$<string>());
      }

      importExcel(departments: DepartmentDto[]): Observable<number> {
            return this.http.post<ApiResponse<number>>(`${this.API_URL}/bulk`, { departments })
                  .pipe(handleApiResponse$<number>());
      }

      updateDepartment(id: string, body: Partial<DepartmentDto>): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.API_URL}/${id}`, body)
                  .pipe(handleApiResponse$<string>());
      }

      deleteDepartment(id: string): Observable<string> {
            return this.http.delete<ApiResponse<string>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<string>());
      }
}