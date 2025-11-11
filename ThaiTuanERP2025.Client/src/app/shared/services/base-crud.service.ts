import { HttpClient, HttpParams } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../models/api-response.model";
import { PagedRequest } from "../models/paged-request.model";
import { PagedResult } from "../models/paged-result.model";
import { handleApiResponse$ } from "../operators/handle-api-response.operator";

export abstract class BaseCrudService<TDto, TRequest, TUpdate = TRequest, TUpdateResponse = TDto> {
      protected constructor(
            protected http: HttpClient,
            protected readonly endpoint: string// e.g. `${environment.apiUrl}/api/taxes`
      ) {}

      getAll(): Observable<TDto[]> {
            return this.http.get<ApiResponse<TDto[]>>(this.endpoint)
                  .pipe(
                        handleApiResponse$<TDto[]>(),
                        catchError(err => throwError(() => err))
                  );
      }

      getPaged(request: PagedRequest): Observable<PagedResult<TDto>> {
            let params = new HttpParams()
                  .set('pageIndex', request.pageIndex)
                  .set('pageSize', request.pageSize);
            
            if(request.keyword) params = params.set('keyword', request.keyword);
            if(request.sort) params = params.set('sort', request.sort);
            if(request.filters) {
                  Object.entries(request.filters).forEach(([k, v]) => {
                        if (v !== undefined && v !== null) params = params.set(k, String(v));
                  });
            }

            return this.http
                  .get<ApiResponse<PagedResult<TDto>>>(`${this.endpoint}/paged`, { params })
                  .pipe(
                        handleApiResponse$<PagedResult<TDto>>(), 
                        catchError(err => throwError(() => err))
                  )
      }

      getById(id: string): Observable<TDto> {
            return this.http
                  .get<ApiResponse<TDto>>(`${this.endpoint}/${id}`)
                  .pipe(
                        handleApiResponse$<TDto>(),
                        catchError(err => throwError(() => err))
                  );
      }

      create(payload: TRequest): Observable<void> {
            return this.http
                  .post<ApiResponse<void>>(this.endpoint, payload)
                  .pipe(
                        handleApiResponse$<void>(), 
                        catchError(err => throwError(() => err))
                  );
      }

      update(id: string, payload: TUpdate): Observable<TUpdateResponse> {
            return this.http
                  .put<ApiResponse<TUpdateResponse>>(`${this.endpoint}/${id}`, payload)
                  .pipe(
                        handleApiResponse$<TUpdateResponse>(),
                        catchError(err => throwError(() => err))
                  );
      }

      delete(id: string): Observable<boolean> {
            return this.http
                  .delete<ApiResponse<boolean>>(`${this.endpoint}/${id}`)
                  .pipe(
                        handleApiResponse$<boolean>(),
                        catchError(err => throwError(() => err))
                  );
      }

      toggleActive(id: string): Observable<boolean> {
            console.log('Toggle active called for id:', id);
            return this.http
                  .patch<ApiResponse<boolean>>(`${this.endpoint}/${id}/toggle-activate`, null)
                  .pipe(
                        handleApiResponse$<boolean>(),
                        catchError(err => throwError(() => err))
                  );
      }
}