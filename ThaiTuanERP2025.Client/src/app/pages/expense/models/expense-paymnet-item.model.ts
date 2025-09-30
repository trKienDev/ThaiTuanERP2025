export interface ExpensePaymentItemDto {
      id: string;
      expensePaymentId: string;
      invoiceId?: string;
      budgetCodeId?: string;
      cashoutCodeId?: string;

      itemName: string;
      quantity: number;
      unitPrice: number;
      taxRate: number;
      amount: number;
      taxAmount: number;
      totalWithTax: number;
}

export interface ExpensePaymentItemRequest {
      invoiceId?: string;
      budgetCodeId?: string;
      cashoutCodeId?: string;

      itemName: string;
      quantity: number;
      unitPrice: number;
      taxRate: number;
      amount: number;
      taxAmount: number;
      totalWithTax: number;
}
