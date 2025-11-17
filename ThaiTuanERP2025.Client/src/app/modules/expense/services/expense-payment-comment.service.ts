import { Injectable } from "@angular/core";
import { ExpensePaymentCommentDto, ExpensePaymentCommentRequest } from "../models/expense-payment-comment.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class ExpensePaymentCommentApiService {
      private readonly API_URL = `${environment.apiUrl}/expense-payment-comments`;
      constructor(private readonly http: HttpClient) {}

      submitComment(paymentId: string, request: ExpensePaymentCommentRequest): Observable<ExpensePaymentCommentDto> {
            return this.http
                  .post<ApiResponse<ExpensePaymentCommentDto>>(`${this.API_URL}/${paymentId}`, request)
                  .pipe(
                        handleApiResponse$<ExpensePaymentCommentDto>(),
                        catchError(err => throwError(() => err))
                  );
      }
      
      getByPayment(paymentId: string): Observable<ExpensePaymentCommentDto[]> {
            return this.http
                  .get<ApiResponse<ExpensePaymentCommentDto[]>>(`${this.API_URL}/by-payment/${paymentId}`)
                  .pipe(
                        handleApiResponse$<ExpensePaymentCommentDto[]>(),
                        catchError(err => throwError(() => err))
                  );
      }
}