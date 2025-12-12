import { Injectable } from "@angular/core";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { SupplierBeneficiaryInforDto, SupplierDto, SupplierPayload } from "../../models/supplier.model";
import { BaseApiService } from "../../../../shared/services/base-api.service";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class SupplierApiService extends BaseApiService<SupplierDto, SupplierPayload, string> {
      constructor(http: HttpClient) {
            super(http, `${environment.server.apiUrl}/supplier`)
      }

      override create(payload: SupplierPayload): Observable<string> {
            return this.http.post<ApiResponse<string>>(this.endpoint, payload)
                  .pipe(handleApiResponse$<string>(),
                  catchError(err => throwError(() => err))
            );
      }

      getBeneficiaryById(id: string): Observable<SupplierBeneficiaryInforDto> {
            return this.http.get<ApiResponse<SupplierBeneficiaryInforDto>>(`${this.endpoint}/${id}/beneficiary`)
                  .pipe(
                        handleApiResponse$<SupplierBeneficiaryInforDto>(),
                        catchError(err => throwError(() => err))
                  );
      }
}