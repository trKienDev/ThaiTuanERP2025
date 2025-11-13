import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { BudgetPlanDto, BudgetPlanRequest } from "../models/budget-plan.model";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class BudgetPlanService extends BaseCrudService<BudgetPlanDto, BudgetPlanRequest>{
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-plan`);
      }

      getByMyDepartment(): Observable<BudgetPlanDto[]> {
            return this.http.get<ApiResponse<BudgetPlanDto[]>>(`${this.endpoint}/following`)
                  .pipe(handleApiResponse$<BudgetPlanDto[]>());
      }
}