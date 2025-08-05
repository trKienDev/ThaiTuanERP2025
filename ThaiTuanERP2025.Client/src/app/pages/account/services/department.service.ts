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

      getAll(): Observable<DepartmentDto[]> {
            return this.http.get<ApiResponse<DepartmentDto[]>>(`${this.API_URL}/all`).pipe(
                  map(res => {
                        if (res.isSuccess && res.data) return res.data;
                        throw new Error(res.message || 'Không thể tải danh sách phòng ban');
                  }),
                  catchError(err => throwError(() => new Error(err?.error?.message || 'Không thể tải danh sách phòng ban')))
            );
      }

      getByIds(ids: string[]) {
            return this.http.post<ApiResponse<DepartmentDto[]>>(`${this.API_URL}/by-ids`, ids);
      } 

      add(dept: DepartmentDto): Observable<void> {
            return this.http.post<ApiResponse<{ departmentId: string }>>(this.API_URL, dept).pipe(
                  map(res => {
                        if(!res.isSuccess) throw new Error(res.message || 'Thêm phòng ban mới thất bại');
                  }),
                  catchError(err => throwError(() => new Error(err?.error?.message || 'Không thể thêm phòng ban mới')))
            )
      }

      importExcel(departments: DepartmentDto[]): Observable<number> {
            return this.http.post<ApiResponse<{ added: number}>>(`${this.API_URL}/bulk`, { departments }).pipe(
                  map(res => {
                        if(!res.isSuccess || !res.data) throw new Error(res.message || 'Import excel thất bại');
                        return res.data.added;
                  }),
                  catchError(err => throwError(() => new Error(err?.error?.message || 'Lỗi khi import excel')))
            )
      }
}