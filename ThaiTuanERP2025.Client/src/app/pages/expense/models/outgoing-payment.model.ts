import { OutgoingBankAccountComponent } from "../pages/bank-account-shell-page/outgoing-bank-account/outgoing-bank-account.component";

export enum OutgoingPaymentStatus {
      pending = 0, // chờ tạo lệnh
      approved = 1, // đã duyệt
      created = 2, // đã tạo lệnh
      recorded = 3, // đã ghi sổ
      cancelled = 4, // đã hủy
}

export interface OutgoingPayment {
      name: string;
      description?: string;
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      totalOutgoingAmount: number;
      outgoingAmount: number;
      followerIds: string[];
      dueDate: Date;
      postingDate: Date;
      paymentDate: Date;
      status: OutgoingPaymentStatus | string;
      expensePaymentId: string;
      outgoingBankAccountId: string;
}

export interface OutgoingPaymentDto extends OutgoingPayment {
      id: string;
}

export interface OutgoingPaymentRequest extends OutgoingPayment {
      expensePaymentId: string;
      outgoingBankAccountId: string;
      dueDate: Date;
}

export interface OutgoingPaymentDetailDto extends OutgoingPaymentDto {
      postingDate: Date;
      paymentDate: Date;      
      dueDate: Date;
}

export interface OutgoingPaymentSummaryDto extends OutgoingPaymentDto {
      outgoingBankAccountName: string;
      expensePaymentName: string;
}
