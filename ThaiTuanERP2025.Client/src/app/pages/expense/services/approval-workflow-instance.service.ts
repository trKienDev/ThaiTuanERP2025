import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { ApprovalWorkflowInstanceDto, ApprovalWorkflowInstanceRequest } from "../models/approval-workflow-instance.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";
import { catchError, throwError } from "rxjs";
import { ApproveStepRequest } from "../models/approval-step-instance.model";

@Injectable({ providedIn: 'root'})
export class ApprovalWorkflowInstanceService extends BaseCrudService<ApprovalWorkflowInstanceDto, ApprovalWorkflowInstanceRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/approval-workflow-instances`);
      }

      approveStep(instanceId: string, stepId: string, request: ApproveStepRequest) {
            return this.http.post<ApiResponse<string>>(`${this.endpoint}/${instanceId}/steps/${stepId}/approve`, request)
                  .pipe(
                        handleApiResponse$<string>(),
                        catchError(err => throwError(() => err))
                  );
      };

      rejectStep(instanceId: string, stepId: string, userId: string, reason: string) {
            return this.http.post<ApiResponse<string>>(
                  `${this.endpoint}/${instanceId}/steps/${stepId}/reject`, 
                  { userId, reason }
            ).pipe(
                  handleApiResponse$<string>(),
                  catchError(err => throwError(() => err))
            );
      }
}