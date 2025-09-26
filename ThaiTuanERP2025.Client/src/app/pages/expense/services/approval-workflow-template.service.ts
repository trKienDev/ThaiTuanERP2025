import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { ApprovalWorkflowTemplateDto, CreateApprovalWorkflowTemplateRequest, UpdateApprovalWorkflowTemplateRequest } from "../models/approval-workflow-template.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";

@Injectable({ providedIn: 'root' })
export class ApprovalWorkflowTemplateService extends BaseCrudService<ApprovalWorkflowTemplateDto, CreateApprovalWorkflowTemplateRequest, UpdateApprovalWorkflowTemplateRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/workflow-templates`);
      }
}