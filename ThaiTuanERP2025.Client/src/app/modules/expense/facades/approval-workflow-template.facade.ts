import { inject, Injectable } from "@angular/core";
import { ApprovalWorkflowTemplateDto, ApprovalWorkflowTemplateRequest } from "../models/approval-workflow-template.model";
import { Observable } from "rxjs";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { ApprovalWorkflowTemplateApiService } from "../services/approval-workflow-template.service";

@Injectable({ providedIn: 'root'})
export class ApprovalWorkflowTemplateFacade extends BaseApiFacade<ApprovalWorkflowTemplateDto, ApprovalWorkflowTemplateRequest> {
      constructor() {
            super(inject(ApprovalWorkflowTemplateApiService));
      }
      readonly approvalWorkflowTemplates$: Observable<ApprovalWorkflowTemplateDto[]> = this.list$;
}