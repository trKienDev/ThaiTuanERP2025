import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { SupplierDto, SupplierRequest } from "../models/supplier.model";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";
import { BaseApiService } from "../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class SupplierApiService extends BaseApiService<SupplierDto, SupplierRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/suppliers`)
      }

      checkNameAvailable(name: string, excludeId?: string): Observable<boolean> {
            let params = new HttpParams().set('name', name);
            if(excludeId) params = params.set('excludeId', excludeId);

            return this.http.get<ApiResponse<boolean>>(`${this.endpoint}/check-available`, { params })
                  .pipe(
                        handleApiResponse$<boolean>(),
                        catchError(err => throwError(() => err))
                  )
      }
}