export interface ApprovalWorkflowTemplateDto {
      id: string;
      name: string;
      documentType: string;
      version: number;
      isActive: boolean;
}

export interface CreateApprovalWorkflowTemplateRequest {
      name: string;
      documentType: string;
      version: number;
      isActive: boolean;
}

export interface UpdateApprovalWorkflowTemplateRequest extends CreateApprovalWorkflowTemplateRequest {
      id: string;
}

