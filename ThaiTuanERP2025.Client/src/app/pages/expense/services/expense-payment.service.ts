import { Injectable } from "@angular/core";
import { ExpensePaymentDetailDto, ExpensePaymentDto, ExpensePaymentRequest, ExpensePaymentSummaryDto } from "../models/expense-payment.model";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { HttpClient } from "@angular/common/http";
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

      getFollowingPayments(): Observable<ExpensePaymentSummaryDto[]> {
            return this.http.
                  get<ApiResponse<ExpensePaymentSummaryDto[]>>(`${this.endpoint}/following`)
                  .pipe(
                        handleApiResponse$<ExpensePaymentSummaryDto[]>(),
                        catchError(err => throwError(() => err))
                  );
      }
} 