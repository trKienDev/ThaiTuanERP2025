import { DepartmentBriefDto } from "../../account/models/department.model";
import { BudgetCodeDto } from "./budget-code.model";
import { BudgetPeriodDto } from "./budget-period.model";

export interface BudgetPlanDto {
      id: string;
      amount: number;
      createdDate: Date;
      isReviewed: boolean;
      isApproved: boolean;
      
      departmentId: string;
      department: DepartmentBriefDto;

      budgetCodeId: string;
      budgetCode: BudgetCodeDto;

      budgetPeriodId: string;
      budgetPeriod: BudgetPeriodDto;
}

export interface BudgetPlanRequest {
      departmentId: string;
      budgetCodeId: string;
      budgetPeriodId: string;
      amount: number;
}
