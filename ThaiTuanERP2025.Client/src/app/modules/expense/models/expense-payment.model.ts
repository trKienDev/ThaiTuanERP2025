import { UserBriefAvatarDto } from "../../account/models/user.model";
import { ExpenseWorkflowInstanceDetailDto } from "./expense-workflow-instance.model";
import { ExpensePaymentAttachmentDto } from "./expense-payment-attachment.model";
import { ExpensePaymentItemDetailDto, ExpensePaymentItemLookupDto, ExpensePaymentItemPayload } from "./expense-payment-item.model";
import { SupplierDto } from "./supplier.model";
import { OutgoingPaymentLookupDto } from "./outgoing-payment.model";


export enum PayeeType {
      supplier = 1,
      employee = 2,
}

export enum ExpensePaymentStatus {
      draft = 0,
      submitted = 1,
      pending = 2,
      approved = 3,
      rejected = 4,
      cancelled = 5,
      readyForPayment = 6,
      partiallyPaid = 7,
      fullyPaid = 8,
}

export interface ExpensePaymentPayload {
      name: string;
      payeeType: PayeeType;
      supplierId?: string;
      
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      description?: string;
      
      dueDate: Date;
      hasGoodsReceipt: boolean;

      items: ExpensePaymentItemPayload[];
      followerIds: string[];
      
      managerApproverId: string;
      attachmentIds?: string[] | null;
}

export interface ExpensePaymentDto {
      id: string;
      name: string;
      payeeType: PayeeType;
      supplierId?: string;
      supplier: SupplierDto;
      description?: string;
      
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      
      dueAt: string;
      hasGoodReceipt: boolean;
      totalAmount: number;
      totalTax: number;
      totalWithTax: number;

      items: ExpensePaymentItemDetailDto[];
      attachments: ExpensePaymentAttachmentDto[];
}

export interface ExpensePaymentDetailDto {
      id: string;
      name: string;
      subId: string;
      description?: string;
      workflowInstance: ExpenseWorkflowInstanceDetailDto;
      items: ExpensePaymentItemLookupDto[];
      attachments: ExpensePaymentAttachmentDto[];
      outgoingPayments: OutgoingPaymentLookupDto[];

      status: ExpensePaymentStatus;

      payeeType: PayeeType;
      supplierId?: string;
      supplierName?: string;

      bankName: string;
      accountNumber: string;
      beneficiaryName: string;

      dueAt: string;
      hasGoodsReceipt: boolean;

      totalAmount: number;
      totalTax: number;
      totalWithTax: number;
      
      outgoingAmountPaid: number;
      remainingOutgoingAmount: number;

      createdByUser: UserBriefAvatarDto;
      createdAt: string;

      followers: UserBriefAvatarDto[];
}

export interface ExpensePaymentLookupDto {
      id: string;
      name: string;
      status: ExpensePaymentStatus;
      totalAmount: number;
      totalTax: number;
      totalWithTax: number;
      outgoingAmountPaid: number;
      remainingOutgoingAmount: number;

      createdByUserId: string;
      createdByUser: UserBriefAvatarDto;

      createdAt: string;
}