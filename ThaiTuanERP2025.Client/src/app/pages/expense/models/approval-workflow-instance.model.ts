import { ApprovalStepInstanceDto } from "./approval-step-instance.model";

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
      currentStepOrder?: number;
      amount?: number;
      currency?: string;
      budgetCode?: string;
      costCenter?: string;
}

export interface CreateApprovalWorkflowInstanceRequest {
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
      Steps: ApprovalStepInstanceDto[];
}