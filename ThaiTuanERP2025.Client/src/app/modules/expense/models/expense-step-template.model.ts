export type ExpenseFlowType = 'Single' | 'OneOfN';
export type ExpenseApproveMode = 'Standard' | 'Condition';

export interface ExpenseStepTemplateDto {
      id: string;
      workflowTemplateId: string;
      name: string;
      order: number;
      flowType: ExpenseFlowType;
      slaHours: number;
      approverMode: ExpenseApproveMode;
      approverIds?: string[];
      resolverKey?: string;
      resolverParams?: object;
}

export interface ExpenseStepTemplatePayload {
      name: string;
      order: number;
      flowType: ExpenseFlowType;
      slaHours: number;
      approverIds: string[] | null;
      approveMode: ExpenseApproveMode;
      resolverKey?: string | null;
      resolverParams?: object | null;
}

