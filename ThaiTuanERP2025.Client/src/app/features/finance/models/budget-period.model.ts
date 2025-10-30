export interface BudgetPeriodDto {
      id: string;
      year: number;
      month: number;
      startDate: Date;
      endDate: Date;    
      isActive: boolean;
}

export interface BudgetPeriodRequest {
      year: number;
      month: number;
      startDate: Date;
      endDate: Date;
}