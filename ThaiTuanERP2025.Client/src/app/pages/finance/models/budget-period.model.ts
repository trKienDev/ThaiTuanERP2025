export interface BudgetPeriodModel {
      id: string;
      year: number;
      month: number;
      isActive: boolean;
      updatedDate?: string;
}
export interface CreateBudgetPeriodModel {
      year: number;
      month: number;
}