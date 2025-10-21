export interface BudgetPeriodModel {
      id: string;
      year: number;
      month: number;
      isActive: boolean;
      createdDate: string;
      updatedDate?: string;
}
export interface CreateBudgetPeriodModel {
      year: number;
      month: number;
}