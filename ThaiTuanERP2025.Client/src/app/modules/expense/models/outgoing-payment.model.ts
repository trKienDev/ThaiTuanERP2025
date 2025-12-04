import { UserBriefAvatarDto, UserDto } from "../../account/models/user.model";
import { ExpensePaymentItemLookupDto } from "./expense-payment-item.model";
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

export interface OutgoingPaymentDto {
      id: string;
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

export interface OutgoingPaymentDetailDto {
      id: string;
      subId: string;
      name: string;
      description?: string;
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      totalOutgoingAmount: number;
      outgoingAmount: number;
      status: OutgoingPaymentStatus;

      dueAt: Date;
      postingAt: Date;
      paymentAt: Date;

      expensePaymentId: string;
      expensePaymentName: string;
      expensePaymentAmount: number;
      expensePaymentItems: ExpensePaymentItemLookupDto[];

      outgoingBankAccountId: string;
      outgoingBankAccountName: string;
      
      employeeId?: string | null;
      employee?: UserDto | null;

      supplierId?: string | null;
      supplierName: string;

      createdByUserId: string;
      createdByUser: UserDto;
      createdAt: Date;   

      followerIds: string[];
      followers: UserBriefAvatarDto[];
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
