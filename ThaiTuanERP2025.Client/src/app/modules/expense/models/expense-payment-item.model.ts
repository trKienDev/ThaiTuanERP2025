import { BudgetCodeDto, BudgetCodeLookupDto } from "../../finance/models/budget-code.model";
import { CashoutCodeDto } from "../../finance/models/cashout-code.model";
import { InvoiceDto } from "./invoice.model";


export interface ExpensePaymentItemPayload {
      itemName: string;
      invoiceStoredFileId?: string | undefined;
      budgetPlanDetailId: string;
      quantity: number;
      unitPrice: number;
      taxRate: number;
      amount: number;
      taxAmount: number;
      totalWithTax: number;
}

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

export interface ExpensePaymentItemLookupDto {
      itemName: string;
      quantity: number;
      unitPrice: number;
      taxRate: number;
      amount: number;
      taxAmount: number;
      totalWithTax: number;

      budgetCode: BudgetCodeLookupDto;
      cashoutCodeName: string;
}

export interface ExpensePaymentItemDetailDto extends ExpensePaymentItemDto {
      budgetCode?: BudgetCodeDto;
      cashoutCode?: CashoutCodeDto;
      invoice?: InvoiceDto;
}
