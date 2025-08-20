import { HttpClient, HttpParams } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../models/api-response.model";
import { handleApiResponse$ } from "../../utils/handle-api-response.operator";
import { PagedRequest } from "../../models/paged-request.model";
import { PagedResult } from "../../models/paged-result.model";

export abstract class BaseCrudService<TDto, TCreate, TUpdate> {
      protected constructor(
            protected http: HttpClient,
            protected readonly endpoint: string// e.g. `${environment.apiUrl}/api/taxes`
      ) {}

      getAll(): Observable<TDto[]> {
            return this.http
                  .get<ApiResponse<TDto[]>>(this.endpoint)
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

      create(payload: TCreate): Observable<TDto> {
            return this.http
                  .post<ApiResponse<TDto>>(this.endpoint, payload)
                  .pipe(
                        handleApiResponse$<TDto>(), 
                        catchError(err => throwError(() => err))
                  );
      }

      update(id: string, payload: TUpdate): Observable<TDto> {
            return this.http
                  .put<ApiResponse<TDto>>(`${this.endpoint}/${id}`, payload)
                  .pipe(
                        handleApiResponse$<TDto>(),
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

      toggleActive(id: string, isActive: boolean): Observable<boolean> {
            return this.http
                  .patch<ApiResponse<boolean>>(`${this.endpoint}/${id}/status`, { isActive })
                  .pipe(
                        handleApiResponse$<boolean>(),
                        catchError(err => throwError(() => err))
                  );
      }
}