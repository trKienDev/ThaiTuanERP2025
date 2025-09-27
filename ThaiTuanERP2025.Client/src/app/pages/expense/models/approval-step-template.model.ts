export type FlowType = 'single' | 'one-of-n';
export type ApproverMode = 'standard' | 'condition';

export interface ApprovalStepTemplateDto {
      id: string;
      workflowTemplateId: string;
      name: string;
      order: number;
      flowType: FlowType;
      slaHours: number;
      approverMode: ApproverMode;
      approverIds?: string[];
      resolverKey?: string;
      resolverParams?: object;
      allowOverride: boolean;
}

export interface CreateApprovalStepTemplateRequest {
      name: string;
      order: number;
      flowType: FlowType;
      slaHours: number;
      approverIds: string[] | null;
      approverMode: ApproverMode;
      resolverKey?: string | null;
      resolverParams?: any | null;
      allowOverride?: boolean;
}

export interface UpdateApprovalStepTemplateRequest extends CreateApprovalStepTemplateRequest {
      id: string;
}