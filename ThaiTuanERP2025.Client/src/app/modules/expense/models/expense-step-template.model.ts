export type ExpenseFlowType = 'Single' | 'OneOfN';
export type ExpenseApproveMode = 'Standard' | 'Condition';
export type ExpenseStepResolverKey = 'None' | 'DepartmentManager';

export interface ExpenseStepTemplateDto {
      id: string;
      workflowTemplateId: string;
      name: string;
      order: number;
      flowType: string;
      slaHours: number;
      approveMode: string;
      approverIds: string[];
      resolverKey?: ExpenseStepResolverKey;
      resolverParams?: object;
}

export interface ExpenseStepTemplatePayload {
      name: string;
      order: number;
      flowType: ExpenseFlowType;
      slaHours: number;
      approverIds: string[] | string | null;
      approveMode: ExpenseApproveMode;
      resolverKey: ExpenseStepResolverKey;
      resolverParams?: object | null;
}

