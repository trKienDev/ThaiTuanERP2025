import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { PagedResult } from "../../../shared/models/paged-result.model";
import { BankAccountDto, CreateBankAccountCommand, UpdateBankAccountCommand } from "../models/bank-account.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";
import { ApiResponse } from "../../../core/models/api-response.model";

@Injectable({ providedIn: 'root' })
export class BankAccountService {
      private readonly API_URL = `${environment.apiUrl}/bank-accounts`;

      constructor(private http: HttpClient) {}

      getPaged(options: {
            onlyActive?: boolean;
            departmentId?: string;
            page?: number;
            pageSize?: number;
      }): Observable<PagedResult<BankAccountDto>> {
            let params = new HttpParams();
            if(options.onlyActive !== undefined) params = params.set('onlyActive', String(options.onlyActive));
            if(options.departmentId) params = params.set('departmentId', options.departmentId);
            if(options.page) params = params.set('page', options.page);
            if(options.pageSize) params = params.set('pageSize', options.pageSize);

            return this.http.get<ApiResponse<PagedResult<BankAccountDto>>>(`${this.API_URL}/all`, { params })
                  .pipe(handleApiResponse$<PagedResult<BankAccountDto>>());
      }

      getById(id: string): Observable<BankAccountDto> {
            return this.http.get<ApiResponse<BankAccountDto>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<BankAccountDto>());
      }

      create(command: CreateBankAccountCommand): Observable<BankAccountDto> {
            return this.http.post<ApiResponse<BankAccountDto>>(this.API_URL, command)
                  .pipe(handleApiResponse$<BankAccountDto>());
      }

      update(id: string, command: UpdateBankAccountCommand): Observable<BankAccountDto> {
            return this.http.put<ApiResponse<BankAccountDto>>(`${this.API_URL}/${id}`, command)
                  .pipe(handleApiResponse$<BankAccountDto>());
      }

      delete(id: string): Observable<string> {
            return this.http.delete<ApiResponse<string>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<string>());
      }

      toggleStatus(id: string, isActive: boolean): Observable<string> {
            const params = new HttpParams().set('isActive', String(isActive));
            return this.http.patch<ApiResponse<string>>(`${this.API_URL}/${id}/status`, null, { params })
                  .pipe(handleApiResponse$<string>());
      }
}