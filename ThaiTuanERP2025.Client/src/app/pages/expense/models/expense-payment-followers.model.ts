export interface ExpensePaymentFollowerDto {
      id: string;
      expensePaymentId: string;
      userId: string;
}

export interface ExpensePaymentFollowerRequest {
      expensePaymentId: string;
      userId: string;
}