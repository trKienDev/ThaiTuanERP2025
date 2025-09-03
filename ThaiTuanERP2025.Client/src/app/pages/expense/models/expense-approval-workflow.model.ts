export type StepFlowType = 'any' | 'sequential' | 'all';

export interface WorkflowStepRequest {
      title: string;
      order: number; // 1..N theo thứ tự trên canvas
      candidateUserIds: string[];
      flowType: StepFlowType;
      slaHours?: number | null;
      description?: string | null;
}

export interface CreateExpenseApprovalWorkflowRequest {
      name: string;
      isActive: boolean;
      steps: WorkflowStepRequest[];
}

export interface ExpenseApprovalWorkflowDto {
      id: string;
      name: string;
      isActive: boolean;
      steps: Array<{
            id: string;
            title: string;
            order: number;
            flowType: StepFlowType;
            slaHours?: number | null;
      }>;
}