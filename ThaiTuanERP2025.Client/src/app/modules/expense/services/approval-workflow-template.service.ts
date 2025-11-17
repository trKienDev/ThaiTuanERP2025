import { Injectable } from "@angular/core";
import { ApprovalWorkflowTemplateDto, ApprovalWorkflowTemplateRequest } from "../models/approval-workflow-template.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { BaseApiService } from "../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class ApprovalWorkflowTemplateApiService extends BaseApiService<ApprovalWorkflowTemplateDto, ApprovalWorkflowTemplateRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/workflow-templates`);
      }
}