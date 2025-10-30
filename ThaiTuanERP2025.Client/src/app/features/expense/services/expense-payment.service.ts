import { Injectable } from "@angular/core";
import { ExpensePaymentDetailDto, ExpensePaymentDto, ExpensePaymentRequest, ExpensePaymentSummaryDto } from "../models/expense-payment.model";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' }) 
export class ExpensePaymentService extends BaseCrudService<ExpensePaymentDto, ExpensePaymentRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/expense-payments`);
      }

      getDetailById(id: string): Observable<ExpensePaymentDetailDto> {
            return this.http.
                  get<ApiResponse<ExpensePaymentDetailDto>>(`${this.endpoint}/${id}/detail`)
                  .pipe(
                        handleApiResponse$<ExpensePaymentDetailDto>(),
                        catchError(err => throwError(() => err))
                  );
      }

      getFollowingPaymentsPaged(page: number, pageSize: number, updatedAfter?: string): Observable<ExpensePaymentSummaryDto[]> {
            let params = new HttpParams()
                  .set('page', page.toString())
                  .set('pageSize', pageSize.toString());

            if(updatedAfter) {
                  params = params.set('updatedAfter', updatedAfter);
            }

            return this.http.
                  get<ApiResponse<ExpensePaymentSummaryDto[]>>(`${this.endpoint}/following`, { params })
                  .pipe(
                        handleApiResponse$<ExpensePaymentSummaryDto[]>(),
                        catchError(err => throwError(() => err))
                  );
      }
} 