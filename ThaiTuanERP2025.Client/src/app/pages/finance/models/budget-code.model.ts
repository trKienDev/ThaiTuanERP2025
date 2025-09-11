export interface BudgetCodeDto {
      id: string;
      code: string;
      name: string;
      budgetGroupId: string;
      budgetGroupName: string;
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

export interface BudgetCodeWithAmountDto {
      id: string;
      code: string;
      name: string;
      year: number;
      month: number;
      budgetPlanId: string | null;
      amount: number | null;
      budgetGroupName: string | null;
}