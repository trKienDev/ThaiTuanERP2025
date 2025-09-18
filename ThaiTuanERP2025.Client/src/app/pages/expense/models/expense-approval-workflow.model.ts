export type FlowType = 'single' | 'one-of-n';
export interface ApprovalStepRequest {
      name: string;
      approverIds: string[];
      flowType: FlowType;
      sla: number;
      order: number;
}