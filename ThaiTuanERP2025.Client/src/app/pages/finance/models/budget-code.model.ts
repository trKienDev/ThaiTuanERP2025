export interface BudgetCodeDto {
      id: string;
      code: string;
      name: string;
      budgetGroupId: string;
      isActive: boolean;
      createdDate: string;
}
export interface CreateBudgetCodeRequest {
      code: string;
      name: string;
      budgetGroupId: string;
}
export interface UpdateBudgetCodeRequest extends CreateBudgetCodeRequest {
      id: string;
}