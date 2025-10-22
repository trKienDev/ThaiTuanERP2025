import { UserDto } from "../../account/models/user.model";
import { ApproverMode, FlowType } from "./approval-step-template.model";

export enum StepStatus {
      Pending = 'Pending',
      Waiting = 'Waiting',
      Approved = 'Approved',
      Rejected = 'Rejected',
      Skipped = 'Skipped',
      Expired = 'Expired',
}

export interface ApprovalStepInstanceDto {
      id: string;
      workflowInstanceId: string;
      templateStepId?: string;
      name: string;
      order: number;
      flowType: FlowType;
      slaHours: number;
      approverMode: ApproverMode;

      resolvedApproverCandidateIds?: string[];
      defaultApproverId?: string;
      selectedApproverId?: string;

      status: string;
      StartedAt?: Date;
      dueAt?: Date;
      approvedAt?: Date;
      approvedBy?: string;
      rejectedAt?: Date;
      rejectedBy?: string;

      comment?: string;
      slaBreached?: boolean;
      history?: object;
}

export interface ApprovalStepInstanceDetailDto extends ApprovalStepInstanceDto {
      approverCandidates: UserDto[];

      approvedByUser?: UserDto;
      rejectedByUser?: UserDto;
      defaultApproverUser?: UserDto;

      currentStepOrder: number;
}

export interface ApproveStepRequest {
      userId: string;
      paymentId: string;
      comment?: string;
}
export interface RejectStepRequest extends ApproveStepRequest {}

export interface ApprovalStepInstanceStatusDto {
      status: StepStatus;
      startedAt?: Date;
      dueAt?: Date;
      
      approvedAt?: Date;
      approvedBy?: string;
      approvedByUser?: UserDto;

      rejectedAt?: Date;
      rejectedBy?: string;
      rejectedByUser?: UserDto;

      defaultApproverId: string;
      defaultApproverUser: UserDto;
}