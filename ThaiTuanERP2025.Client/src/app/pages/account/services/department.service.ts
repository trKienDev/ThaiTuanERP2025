import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { catchError, map, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { DepartmentDto } from "../dtos/department.dto";

@Injectable({ providedIn: 'root' }) 
export class DepartmentService {
      private readonly API_URL = `${environment.apiUrl}/department`;
      constructor(private http: HttpClient) {}

      getAll(): Observable<ApiResponse<DepartmentDto[]>> {
            return this.http.get<ApiResponse<DepartmentDto[]>>(`${this.API_URL}/all`);
      }

      getByIds(ids: string[]) {
            return this.http.post<ApiResponse<DepartmentDto[]>>(`${this.API_URL}/by-ids`, ids);
      } 

      create(dept: DepartmentDto): Observable<ApiResponse<string>> {
            return this.http.post<ApiResponse<string>>(this.API_URL, dept);
      }

      importExcel(departments: DepartmentDto[]): Observable<ApiResponse<number>> {
            return this.http.post<ApiResponse<number>>(`${this.API_URL}/bulk`, { departments });
      }
}