export interface BudgetPlanModel {
      id: string;
      amount: number;
      status: string;
      departmentName: string;
      budgetCodeName: string;
      budgetPeriodName: string;
}
export interface CreateBudgetPlanModel {
      departmentId: string;
      budgetCodeId: string;
      budgetPeriodId: string;
      amount: number;
}