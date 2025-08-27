import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { catchError, Observable, of, throwError } from "rxjs";
import { PartnerBankAccountDto, UpsertPartnerBankAccountRequest } from "../models/partner-bank-account.model";
import { ApiResponse } from "../../../core/models/api-response.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";
import { handleHttpError } from "../../../core/utils/handle-http-errors.util";

@Injectable({ providedIn: 'root' })
export class PartnerBankAccountService {
      private readonly API_URL = `${environment.apiUrl}/partners/suppliers`;
      constructor(private http: HttpClient) {}

      get(supplierId: string){
            return this.http.get<ApiResponse<PartnerBankAccountDto>>(`${this.API_URL}/${supplierId}/bank-account`).pipe(
                  handleApiResponse$<PartnerBankAccountDto>(),
                  catchError((err: HttpErrorResponse) => {
                        if(err.status === 404) return of(null);
                        return throwError(() => err); // gửi lỗi  cho component xử lý
                  })
            );
      }

      upsert(supplierId: string, payload: UpsertPartnerBankAccountRequest): Observable<PartnerBankAccountDto> {
            return this.http.put<ApiResponse<PartnerBankAccountDto>>(`${this.API_URL}/${supplierId}/bank-account`, payload).pipe(
                  handleApiResponse$<PartnerBankAccountDto>(),
                  catchError(err => { throw handleHttpError(err); })
            )
      }

      delete(supplierId: string): Observable<boolean> {
            return this.http.delete<ApiResponse<boolean>>(`${this.API_URL}/${supplierId}/bank-account`).pipe(
                  handleApiResponse$<boolean>(),
                  catchError(err => { throw handleHttpError(err); })
            )
      }

}