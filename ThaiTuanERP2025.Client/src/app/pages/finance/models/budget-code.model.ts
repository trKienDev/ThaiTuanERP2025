export interface BudgetCodeModel {
      id: string;
      code: string;
      name: string;
      budgetGroupId: string;
      isActive: boolean;
      createdDate: string;
}
export interface CreateBudgetCodeModel {
      code: string;
      name: string;
      budgetGroupId: string;
}