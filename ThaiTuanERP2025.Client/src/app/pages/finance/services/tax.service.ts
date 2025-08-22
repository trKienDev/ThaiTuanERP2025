import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../core/services/api/base-crud.service";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { TaxDto, TaxRequestDto } from "../models/tax.dto";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";

@Injectable({ providedIn: 'root'})
export class TaxService extends BaseCrudService<TaxDto, TaxRequestDto, TaxRequestDto> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/taxes`);
      }

      checkAvailable(policyName: string, excludeId?: string): Observable<boolean> {
            let params = new HttpParams().set('policyName', policyName);
            if(excludeId) params = params.set('excludeId', excludeId);
            
            return this.http.get<ApiResponse<boolean>>(`${environment.apiUrl}/check-available`, {params})
                  .pipe(
                        handleApiResponse$<boolean>(),
                        catchError(err => throwError(() => err))
                  )
      }
}