import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { DepartmentModel } from "../models/department.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";

@Injectable({ providedIn: 'root' }) 
export class DepartmentService {
      private readonly API_URL = `${environment.apiUrl}/department`;
      constructor(private http: HttpClient) {}

      getAll(): Observable<DepartmentModel[]> {
            return this.http.get<ApiResponse<DepartmentModel[]>>(`${this.API_URL}/all`)
                  .pipe(handleApiResponse$<DepartmentModel[]>());
      }

      getByIds(ids: string[]): Observable<DepartmentModel[]> {
            return this.http.post<ApiResponse<DepartmentModel[]>>(`${this.API_URL}/by-ids`, ids)
                  .pipe(handleApiResponse$<DepartmentModel[]>());
      } 

      create(dept: DepartmentModel): Observable<string> {
            return this.http.post<ApiResponse<string>>(this.API_URL, dept)
                  .pipe(handleApiResponse$<string>());
      }

      importExcel(departments: DepartmentModel[]): Observable<number> {
            return this.http.post<ApiResponse<number>>(`${this.API_URL}/bulk`, { departments })
                  .pipe(handleApiResponse$<number>());
      }

      updateDepartment(id: string, body: Partial<DepartmentModel>): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.API_URL}/${id}`, body)
                  .pipe(handleApiResponse$<string>());
      }

      deleteDepartment(id: string): Observable<string> {
            return this.http.delete<ApiResponse<string>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<string>());
      }
}