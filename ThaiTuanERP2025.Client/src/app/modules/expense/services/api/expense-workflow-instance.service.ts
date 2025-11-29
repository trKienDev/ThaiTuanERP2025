import { Injectable } from "@angular/core";
import { ApprovalWorkflowInstanceDto, ApprovalWorkflowInstanceRequest } from "../../models/expense-workflow-instance.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { Observable } from "rxjs";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root'})
export class ExpenseWorkflowInstanceApiService extends BaseApiService<ApprovalWorkflowInstanceDto, ApprovalWorkflowInstanceRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/expense-workflow-instance`);
      }

      approve(stepId: string): Observable<void> {
            return this.http.post<ApiResponse<void>>(`${this.endpoint}/approve/${stepId}`, null)
                  .pipe(handleApiResponse$<void>());
      }
}