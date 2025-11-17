import { Injectable } from "@angular/core";
import { ApprovalStepTemplateDto, ApprovalStepTemplateRequest } from "../models/approval-step-template.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { BaseApiService } from "../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root'})
export class ApprovalStepTemplateApiService extends BaseApiService<ApprovalStepTemplateDto, ApprovalStepTemplateRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/workflow-templates/steps`);
      }
}