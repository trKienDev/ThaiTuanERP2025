import { Injectable } from "@angular/core";
import { ExpensePaymentDetailDto, ExpensePaymentDto, ExpensePaymentLookupDto, ExpensePaymentPayload } from "../../models/expense-payment.model";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class ExpensePaymentApiService extends BaseApiService<ExpensePaymentDto, ExpensePaymentPayload> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/expense-payment`);
      }

      getDetailById(id: string): Observable<ExpensePaymentDetailDto> {
            return this.http.
                  get<ApiResponse<ExpensePaymentDetailDto>>(`${this.endpoint}/detail/${id}`)
                  .pipe(
                        handleApiResponse$<ExpensePaymentDetailDto>(),
                        catchError(err => throwError(() => err))
                  );
      }

      getFollowing(): Observable<ExpensePaymentLookupDto[]> {
            return this.http.get<ApiResponse<ExpensePaymentLookupDto[]>>(`${this.endpoint}/following`)
                  .pipe(
                        handleApiResponse$<ExpensePaymentLookupDto[]>(),
                        catchError(err => throwError(() => err))
                  )
      }

      // getFollowingPaymentsPaged(page: number, pageSize: number, updatedAfter?: string): Observable<ExpensePaymentSummaryDto[]> {
      //       let params = new HttpParams()
      //             .set('page', page.toString())
      //             .set('pageSize', pageSize.toString());

      //       if (updatedAfter) {
      //             params = params.set('updatedAfter', updatedAfter);
      //       }

      //       return this.http.
      //             get<ApiResponse<ExpensePaymentSummaryDto[]>>(`${this.endpoint}/following`, { params })
      //             .pipe(
      //                   handleApiResponse$<ExpensePaymentSummaryDto[]>(),
      //                   catchError(err => throwError(() => err))
      //             );
      // }
} 