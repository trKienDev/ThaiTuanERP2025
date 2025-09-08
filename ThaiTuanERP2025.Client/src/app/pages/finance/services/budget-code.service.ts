import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable, pipe } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";
import { BudgetCodeDto, CreateBudgetCodeRequest, UpdateBudgetCodeRequest } from "../models/budget-code.model";
import { BaseCrudService } from "../../../core/services/api/base-crud.service";

@Injectable({ providedIn: 'root' })
export class BudgetCodeService extends BaseCrudService<BudgetCodeDto, CreateBudgetCodeRequest, UpdateBudgetCodeRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-code`);
      }

      getAllActive(): Observable<BudgetCodeDto[]> {
            return this.http.get<ApiResponse<BudgetCodeDto[]>>(`${this.endpoint}/active`)
                  .pipe(handleApiResponse$<BudgetCodeDto[]>());
      }
}