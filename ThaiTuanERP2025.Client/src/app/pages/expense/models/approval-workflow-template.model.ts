import { CreateApprovalStepTemplateRequest } from "./approval-step-template.model";

export interface ApprovalWorkflowTemplateDto {
      id: string;
      name: string;
      documentType: string;
      version: number;
      isActive: boolean;
}

export interface CreateApprovalWorkflowTemplateRequest {
      name: string;
      version: number;
      steps: CreateApprovalStepTemplateRequest[];
}

export interface UpdateApprovalWorkflowTemplateRequest extends CreateApprovalWorkflowTemplateRequest {
      id: string;
}

