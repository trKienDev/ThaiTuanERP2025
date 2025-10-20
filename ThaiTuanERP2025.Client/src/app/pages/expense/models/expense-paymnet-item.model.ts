import { BudgetCodeDto } from "../../finance/models/budget-code.model";
import { CashoutCodeDto } from "../../finance/models/cashout-code.model";
import { InvoiceDto } from "./invoice.model";

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

export interface ExpensePaymentItemDetailDto extends ExpensePaymentItemDto {
      budgetCode?: BudgetCodeDto;
      cashoutCode?: CashoutCodeDto;
      invoice?: InvoiceDto;
}
