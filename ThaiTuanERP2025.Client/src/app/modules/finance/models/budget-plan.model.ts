import { DepartmentBriefDto } from "../../account/models/department.model";
import { BudgetCodeDto } from "./budget-code.model";
import { BudgetPeriodDto } from "./budget-period.model";


export enum BudgetPlanStatus {
      draft = 0,
      reviewed = 1,
      approved = 2,
      rejected = 4,
      cancelled = 5,
}

export interface BudgetPlanDto {
      id: string;

      departmentId: string;
      department: DepartmentBriefDto;

      budgetPeriodId: string;
      budgetPeriod: BudgetPeriodDto;

      totalAmount: number;
      isReviewed: boolean;
      isApproved: boolean;
      createdAt: Date;

      SelectedReviewerId?: string | null;
      canReview: boolean;

      dueAt: string;

      details: BudgetPlanDetailDto[];

      status: BudgetPlanStatus;
}

export interface BudgetPlanDetailDto {
      id: string;
      budgetCodeId: string;
      budgetCode: BudgetCodeDto;
      amount: number;
}

export interface BudgetPlanReview extends BudgetPlanDto {
      isEditing: boolean;
      editedAmount: number;
}

// ====== REQUEST =====
export interface BudgetPlanRequest {
      departmentId: string;
      budgetPeriodId: string;
      selectedReviewerId: string;
      selectedApproverId: string;
      details: BudgetPlanDetailRequest[];
}

export interface BudgetPlanDetailRequest {
      budgetCodeId: string;
      amount: number;
      note: string | null;
}
