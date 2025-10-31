import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { TaxRequest, TaxDto } from "../models/tax.model";
import { catchError, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root'})
export class TaxService extends BaseCrudService<TaxDto, TaxRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/taxes`);
      }

      // excludeId: dùng khi update để server bỏ qua id ExcludeId khi check trùng
      checkAvailable(policyName: string, excludeId?: string): Observable<boolean> { 
            let params = new HttpParams().set('policyName', policyName);
            if(excludeId) params = params.set('excludeId', excludeId);
            
            return this.http.get<ApiResponse<boolean>>(`${this.endpoint}/check-available`, {params})
                  .pipe(
                        handleApiResponse$<boolean>(),
                        catchError(err => throwError(() => err))
                  )
      }
}