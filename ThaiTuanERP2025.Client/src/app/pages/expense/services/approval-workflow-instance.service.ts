import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { ApprovalWorkflowInstanceDto, CreateApprovalWorkflowInstanceRequest } from "../models/approval-workflow-instance.model";
import { UpdateApprovalWorkflowTemplateRequest } from "../models/approval-workflow-template.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";

@Injectable({ providedIn: 'root'})
export class ApprovalWorkflowInstanceService extends BaseCrudService<ApprovalWorkflowInstanceDto, CreateApprovalWorkflowInstanceRequest, UpdateApprovalWorkflowTemplateRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/workflow-templates/steps`);
      }
}