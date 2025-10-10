import { UserDto } from "../../account/models/user.model";
import { ApproverMode, FlowType } from "./approval-step-template.model";

export interface ApprovalStepInstanceDto {
      id: string;
      workflowInstanceId: string;
      templateStepId?: string;
      name: string;
      order: number;
      flowType: FlowType;
      slaHours: number;
      approverMode: ApproverMode;

      resolvedApproverCandidateIds?: string;
      defaultApproverId?: string;
      selectedApproverId?: string;

      status: string;
      StartedAt?: Date;
      dueAt?: Date;
      ApprovedAt?: Date;
      ApprovedBy?: string;
      rejectedAt?: Date;
      rejectedBy?: string;

      comment?: string;
      slaBreached?: boolean;
      history?: object;
}

export interface ApprovalStepInstanceDetailDto extends ApprovalStepInstanceDto {
      approverCandidates: UserDto[];
}

export interface ApproveStepRequest {
      userId: string;
      paymentId: string;
      comment?: string;
}
