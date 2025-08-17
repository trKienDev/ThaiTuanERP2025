import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { CreateSupplierRequest, SupplierDto, UpdateSupplierRequest } from "../models/supplier.model";
import { catchError, Observable } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";
import { handleHttpError } from "../../../core/utils/handle-http-errors.util";
import { PagedResult } from "../../../shared/models/paged-result.model";

@Injectable({ providedIn: 'root'}) 
export class SupplierService {
      private readonly API_URL = `${environment.apiUrl}/supplier`;
      constructor(private http: HttpClient) {}

      create(payload: CreateSupplierRequest): Observable<SupplierDto> {
            return this.http.post<ApiResponse<SupplierDto>>(this.API_URL, payload).pipe(
                  handleApiResponse$<SupplierDto>(),
                  catchError(err => { throw handleHttpError(err); })
            )
      }

      getById(id: string): Observable<SupplierDto> {
            return this.http.get<ApiResponse<SupplierDto>>(`${this.API_URL}/${id}`).pipe(
                  handleApiResponse$<SupplierDto>(),
                  catchError(err => { throw handleHttpError(err); })
            )
      }

      getByCode(code: string): Observable<SupplierDto> {
            return this.http.get<ApiResponse<SupplierDto>>(`${this.API_URL}/by-code/${encodeURIComponent(code)}`).pipe(
                  handleApiResponse$<SupplierDto>(),
                  catchError(err => { throw handleHttpError(err); })
            )
      }

      getAll(opts?: { keyword?: string, isActive?: boolean, currency?: string; page?: number, pageSize?: number }): Observable<PagedResult<SupplierDto>> {
            let params = new HttpParams();
            if(opts?.keyword) params = params.set('keyword', opts.keyword);
            if(opts?.isActive !== undefined) params = params.set('isActive', String(opts.isActive));
            if(opts?.currency) params = params.set('currency', opts.currency);
            if(opts?.page) params = params.set('page', String(opts.page));
            if(opts?.pageSize) params = params.set('pageSize', String(opts.pageSize));

            return this.http.get<ApiResponse<PagedResult<SupplierDto>>>(this.API_URL, { params}).pipe(
                  handleApiResponse$<PagedResult<SupplierDto>>(),
                  catchError(err => { throw handleHttpError(err); })
            )
      }

      update(id: string, payload: UpdateSupplierRequest): Observable<SupplierDto> {
            return this.http.put<ApiResponse<SupplierDto>>(`${this.API_URL}/${id}`, payload).pipe(
                  handleApiResponse$<SupplierDto>(),
                  catchError(err => { throw handleHttpError(err); })
            );
      }

      toggleStatus(id: string, isActive: boolean): Observable<SupplierDto> {
            return this.http.patch<ApiResponse<SupplierDto>>(`${this.API_URL}/${id}/status`, null, {
                  params: new HttpParams().set('isActive', String(isActive))
            }).pipe(
                  handleApiResponse$<SupplierDto>(),
                  catchError(err => { throw handleHttpError(err); })
            )
      }

      delete(id: string): Observable<boolean> {
            return this.http.delete<ApiResponse<boolean>>(`${this.API_URL}/${id}`).pipe(
                  handleApiResponse$<boolean>(),
                  catchError(err => { throw handleHttpError(err); })
            );
      }
}