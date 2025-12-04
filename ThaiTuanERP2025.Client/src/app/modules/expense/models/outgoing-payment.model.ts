import { UserDto } from "../../account/models/user.model";
import { ExpensePaymentDetailDto } from "./expense-payment.model";
import { OutgoingBankAccountDto } from "./outgoing-bank-account.model";
import { SupplierDto } from "./supplier.model";

export enum OutgoingPaymentStatus {
      pending = 0, // chờ tạo lệnh
      approved = 1, // đã duyệt
      created = 2, // đã tạo lệnh
      cancelled = 3, // đã hủy
}

export interface OutgoingPaymentPayload {
      name: string;
      outgoingAmount: number;
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      dueAt: Date;
      outgoingBankAccountId: string;
      expensePaymentId: string;
      description?: string;
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
      status: OutgoingPaymentStatus;
      expensePaymentId: string;
      outgoingBankAccountId: string;
      employeeId?: string | null;
      supplierId?: string | null;
}

export interface OutgoingPaymentDto extends OutgoingPayment {
      id: string;
}

export interface OutgoingPaymentDetailDto extends OutgoingPaymentDto {
      subId: string;
      createdByUser: UserDto;
      createdDate: Date;   
      
      supplier?: SupplierDto | null;
      employee?: UserDto | null;
      outgoingBankAccount: OutgoingBankAccountDto;
      expensePayment: ExpensePaymentDetailDto;
}

export interface OutgoingPaymentBriefDto {
      id: string;
      name: string;
      status: OutgoingPaymentStatus;
      postingAt: Date;
      outgoingAmount: number;
}

export interface OutgoingPaymentLookupDto {
      id: string;
      name: string;
      status: OutgoingPaymentStatus;
      dueAt: string;
      postingAt: string;
      outgoingAmount: number;
      
      expensePaymentId: string;
      expensePaymentName: string;

      outgoingBankAccountId: string;      
      outgoingBankAccountName: string;

      supplierId?: string | null;
      supplierName?: string;
}
