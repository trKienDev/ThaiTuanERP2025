import { inject, Injectable } from "@angular/core";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { ApprovalWorkflowTemplateDto, CreateApprovalWorkflowTemplateRequest, UpdateApprovalWorkflowTemplateRequest } from "../models/approval-workflow-template.model";
import { ApprovalWorkflowTemplateService } from "../services/approval-workflow-template.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root'})
export class ApprovalWorkflowTemplateFacade extends BaseCrudFacade<ApprovalWorkflowTemplateDto, CreateApprovalWorkflowTemplateRequest, UpdateApprovalWorkflowTemplateRequest> {
      constructor() {
            super(inject(ApprovalWorkflowTemplateService));
      }
      readonly approvalWorkflowTemplates$: Observable<ApprovalWorkflowTemplateDto[]> = this.list$;
}