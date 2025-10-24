import { Injectable } from "@angular/core";
import { OutgoingPaymentDetailDto, OutgoingPaymentDto, OutgoingPaymentRequest, OutgoingPaymentSummaryDto } from "../models/outgoing-payment.model";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";
import { ApiResponse } from "../../../shared/models/api-response.model";

@Injectable({ providedIn: 'root' })
export class OutgoingPaymentService extends BaseCrudService<OutgoingPaymentSummaryDto, OutgoingPaymentRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/outgoing-payments`);
      }

      getDetailById(id: string): Observable<OutgoingPaymentDetailDto> {
            return this.http.get<ApiResponse<OutgoingPaymentDetailDto>>(`${this.endpoint}/${id}/detail`)
                  .pipe(
                        handleApiResponse$<OutgoingPaymentDetailDto>(),
                        catchError(err => throwError(() => err))
                  );
      }

      getFollowing(): Observable<OutgoingPaymentSummaryDto[]> {
            return this.http.get<ApiResponse<OutgoingPaymentSummaryDto[]>>(`${this.endpoint}/following`)
                  .pipe(
                        handleApiResponse$<OutgoingPaymentSummaryDto[]>(),
                        catchError(err => throwError(() => err))
                  );
      }

      onApprove(id: string): Observable<void> {
            return this.http.post<ApiResponse<void>>(`${this.endpoint}/${id}/approve`, {})
                  .pipe(
                        handleApiResponse$<void>(),
                        catchError(err => throwError(() => err))
                  );
      }

      markCreated(id: string): Observable<void> {
            return this.http.post<ApiResponse<void>>(`${this.endpoint}/${id}/created`, {})
                  .pipe(
                        handleApiResponse$<void>(),
                        catchError(err => throwError(() => err))
                  );
      }
}