import { ApprovalStepTemplateRequest } from "./approval-step-template.model";

export interface ApprovalWorkflowTemplateDto {
      id: string;
      name: string;
      documentType: string;
      version: number;
      isActive: boolean;
}

export interface ApprovalWorkflowTemplateRequest {
      name: string;
      version: number;
      steps: ApprovalStepTemplateRequest[];
}
