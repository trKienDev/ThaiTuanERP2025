export interface BudgetPlanDto {
      id: string;
      amount: number;
      status: string;
      departmentName: string;
      budgetCodeName: string;
      budgetPeriodName: string;
}
export interface CreateBudgetPlanRequest {
      departmentId: string;
      budgetCodeId: string;
      budgetPeriodId: string;
      amount: number;
}
export interface UpdateBudgetPlanRequest extends CreateBudgetPlanRequest {
      id: string;
}