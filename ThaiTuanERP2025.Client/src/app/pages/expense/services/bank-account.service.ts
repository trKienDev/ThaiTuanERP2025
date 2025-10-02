import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BankAccountDto, CreateSupplierBankAccountRequest, CreateUserBankAccountRequest, UpdateBankAccountRequest } from "../models/bank-account.model";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class BankAccountService {
      private readonly API_URL = `${environment.apiUrl}/bank-accounts`;

      constructor(private http: HttpClient) {}

      getById(id: string): Observable<BankAccountDto> {
            return this.http.get<ApiResponse<BankAccountDto>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<BankAccountDto>());
      }

      getByUser(userId: string): Observable<BankAccountDto> {
            return this.http.get<ApiResponse<BankAccountDto>>(`${this.API_URL}/user/${userId}`)
                  .pipe(handleApiResponse$<BankAccountDto>());
      }

      createForUser(request: CreateUserBankAccountRequest): Observable<BankAccountDto> {
            return this.http.post<ApiResponse<BankAccountDto>>(`${this.API_URL}/user`, request)
                  .pipe(handleApiResponse$<BankAccountDto>());
      }

      listBySupplier(supplierId: string): Observable<BankAccountDto[]> {
            return this.http.get<ApiResponse<BankAccountDto[]>>(`${this.API_URL}/supplier/${supplierId}`)
                  .pipe(handleApiResponse$<BankAccountDto[]>());
      }

      createForSupplier(request: CreateSupplierBankAccountRequest): Observable<BankAccountDto> {
            return this.http.post<ApiResponse<BankAccountDto>>(`${this.API_URL}/supplier`, request)
                  .pipe(handleApiResponse$<BankAccountDto>());
      }

      update(id: string, request: UpdateBankAccountRequest): Observable<BankAccountDto> {
            return this.http.put<ApiResponse<BankAccountDto>>(`${this.API_URL}/${id}`, request)
                  .pipe(handleApiResponse$<BankAccountDto>());
      }

      delete(id: string): Observable<boolean> {
            return this.http.delete<ApiResponse<boolean>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<boolean>());
      }
}
