import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { BudgetApproverDto, BudgetApproversRequest } from "../models/budget-approvers.model";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class BudgetApproverService extends BaseCrudService<BudgetApproverDto, BudgetApproversRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-approver`);
      }

      getByUserDepartment(): Observable<BudgetApproverDto[]> {
            return this.http.get<ApiResponse<BudgetApproverDto[]>>(`${this.endpoint}/by-user-department`)
                  .pipe(handleApiResponse$<BudgetApproverDto[]>());
      }
}

