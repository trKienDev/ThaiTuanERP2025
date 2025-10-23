export interface BudgetPeriodDto {
      id: string;
      year: number;
      month: number;
      isActive: boolean;
      budgetPreparationDate: Date;
}
export interface BudgetPeriodRequest {
      year: number;
      month: number;
      budgetPreparationDate: Date;
}