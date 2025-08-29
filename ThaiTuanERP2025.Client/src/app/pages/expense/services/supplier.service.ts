import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { CreateSupplierRequest, SupplierDto, UpdateSupplierRequest } from "../models/supplier.model";
import { BaseCrudService } from "../../../core/services/api/base-crud.service";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class SupplierService extends BaseCrudService<SupplierDto, CreateSupplierRequest, UpdateSupplierRequest> {
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