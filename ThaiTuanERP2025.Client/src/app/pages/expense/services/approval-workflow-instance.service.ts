import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { ApprovalWorkflowInstanceDto, ApprovalWorkflowInstanceRequest } from "../models/approval-workflow-instance.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";

@Injectable({ providedIn: 'root'})
export class ApprovalWorkflowInstanceService extends BaseCrudService<ApprovalWorkflowInstanceDto, ApprovalWorkflowInstanceRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/workflow-templates/steps`);
      }
}