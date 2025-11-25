export interface BudgetPeriodDto {
      id: string;
      year: number;
      month: number;
      startDate: Date;
      endDate: Date;    
      isActive: boolean;
}

export interface BudgetPeriodLookupDto {
      id: string;
      year: number;
      month: number;
}


export interface BudgetPeriodPayload {
      year: number;
      month: number;
      startDate: Date;
      endDate: Date;
}

export interface UpdateBudgetPeriodPayload {
      startDate: string;
      endDate: string;
}