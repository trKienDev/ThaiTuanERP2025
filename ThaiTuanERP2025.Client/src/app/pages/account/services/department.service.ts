import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { DepartmentModel } from "../models/department.model";

@Injectable({ providedIn: 'root' }) 
export class DepartmentService {
      private readonly API_URL = `${environment.apiUrl}/department`;
      constructor(private http: HttpClient) {}

      getAll(): Observable<ApiResponse<DepartmentModel[]>> {
            return this.http.get<ApiResponse<DepartmentModel[]>>(`${this.API_URL}/all`);
      }

      getByIds(ids: string[]) {
            return this.http.post<ApiResponse<DepartmentModel[]>>(`${this.API_URL}/by-ids`, ids);
      } 

      create(dept: DepartmentModel): Observable<ApiResponse<string>> {
            return this.http.post<ApiResponse<string>>(this.API_URL, dept);
      }

      importExcel(departments: DepartmentModel[]): Observable<ApiResponse<number>> {
            return this.http.post<ApiResponse<number>>(`${this.API_URL}/bulk`, { departments });
      }
      deleteDepartment(id: string): Observable<any> {
            return this.http.delete(`${this.API_URL}/${id}`);
      }
         updateDepartment(id: string, body: Partial<DepartmentModel>): Observable<any> {
            return this.http.put(`${this.API_URL}/${id}`, body);
      }
}