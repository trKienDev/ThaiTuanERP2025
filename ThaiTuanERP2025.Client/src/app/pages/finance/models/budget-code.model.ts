export interface BudgetCodeDto {
      id: string;
      code: string;
      name: string;
      budgetGroupId: string;
      budgetGroupName: string;
      isActive: boolean;
      createdDate: string;
}

export interface BudgetCodeRequest {
      code: string;
      name: string;
      budgetGroupId: string;
      cashoutCodeId: string;
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