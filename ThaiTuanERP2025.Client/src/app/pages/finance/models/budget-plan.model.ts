export interface BudgetPlanDto {
      id: string;
      amount: number;
      status: string;
      departmentName: string;
      budgetCodeName: string;
      budgetPeriodName: string;
}
export interface BudgetPlanRequest {
      departmentId: string;
      budgetCodeId: string;
      budgetPeriodId: string;
      amount: number;
}
