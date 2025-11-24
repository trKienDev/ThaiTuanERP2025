import { ApprovalStepInstanceDetailDto, ApprovalStepInstanceDto, ApprovalStepInstanceStatusDto } from "./expense-step-instance.model";

export interface ApprovalWorkflowInstanceDto {
      id: string;
      templateId: string;
      templateVersion: number;
      documentType: string;
      documentNumber: number;
      documentId: string;
      createdByUserId: string;
      createdAt: Date;
      status: string;
      currentStepOrder: number;
      amount?: number;
      currency?: string;
      budgetCode?: string;
      costCenter?: string;
}

export interface ApprovalWorkflowInstanceRequest {
      templateId: string;
      documenID: string;
      documentType: string;
      creatorId: string;
      amount?: number;
      currency?: string;
      budgetCode?: string;
      costCenter?: string;
      startImmediately: boolean;
}

export interface ApprovalWorkflowInstanceDetailDto {
      workflowInstance: ApprovalWorkflowInstanceDto;
      steps: ApprovalStepInstanceDetailDto[];
}

export interface ApprovalWorkflowIntanceStatusDto {
      status: number;
      currentStepOrder: number;     
      steps: ApprovalStepInstanceStatusDto[];
}