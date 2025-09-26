import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { ApprovalStepTemplateDto, CreateApprovalStepTemplateRequest, UpdateApprovalStepTemplateRequest } from "../models/approval-step-template.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";

@Injectable({ providedIn: 'root'})
export class ApprovalStepTemplateService extends BaseCrudService<ApprovalStepTemplateDto, CreateApprovalStepTemplateRequest, UpdateApprovalStepTemplateRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/workflow-templates/steps`);
      }
}