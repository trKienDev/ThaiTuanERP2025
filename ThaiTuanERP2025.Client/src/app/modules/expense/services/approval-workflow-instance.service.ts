import { Injectable } from "@angular/core";
import { ApprovalWorkflowInstanceDto, ApprovalWorkflowInstanceRequest } from "../models/approval-workflow-instance.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";
import { catchError, throwError } from "rxjs";
import { ApproveStepRequest, RejectStepRequest } from "../models/approval-step-instance.model";
import { BaseApiService } from "../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root'})
export class ApprovalWorkflowInstanceApiService extends BaseApiService<ApprovalWorkflowInstanceDto, ApprovalWorkflowInstanceRequest> {
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

      rejectStep(instanceId: string, stepId: string, request: RejectStepRequest) {
            return this.http.post<ApiResponse<string>>(`${this.endpoint}/${instanceId}/steps/${stepId}/reject`, request)
                  .pipe(
                        handleApiResponse$<string>(),
                        catchError(err => throwError(() => err))
                  );
      }
}