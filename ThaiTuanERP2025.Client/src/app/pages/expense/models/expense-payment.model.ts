import { UserDto } from "../../account/models/user.model";
import { ApprovalWorkflowInstanceDto } from "./approval-workflow-instance.model";
import { ExpensePaymentAttachmentDto, ExpensePaymentAttachmentRequest } from "./expense-payment-attachment.model";
import { ExpensePaymentFollowerDto } from "./expense-payment-followers.model";
import { ExpensePaymentItemDto, ExpensePaymentItemRequest } from "./expense-paymnet-item.model";
import { invoiceDto } from "./invoice.model";
import { SupplierDto } from "./supplier.model";

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
}

export interface ExpensePaymentDto {
      id: string;
      name: string;
      payeeType: PayeeType;
      supplierId?: string;
      supplier: SupplierDto;
      
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      
      paymentDate: Date;
      hasGoodReceipt: boolean;
      totalAmount: number;
      totalTax: number;
      totalWithTax: number;

      items: ExpensePaymentItemDto[];
      attachments: ExpensePaymentAttachmentDto[];
      followers: ExpensePaymentFollowerDto[];
}

export interface ExpensePaymentRequest {
      name: string;
      payeeType: PayeeType;
      supplierId?: string;
      
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      
      paymentDate: Date;
      hasGoodsReceipt: boolean;
      totalAmount: number;
      totalTax: number;
      totalWithTax: number;

      status: ExpensePaymentStatus;

      items: ExpensePaymentItemRequest[];
      attachments: ExpensePaymentAttachmentRequest[];
      followerIds: string[];

      managerApproverId: string;
}

export interface ExpensePaymentDetailDto extends ExpensePaymentDto {
      createdByUserId: string;
      createdByUserName: string;
      createdByFullName?: string;
      createdByDepartmentId: string;
      createdByDepartmentName?: string;

      status: number;

      createdAt: Date;
      updatedBy?: UserDto;
      updatedAt?: Date;

      invoices: invoiceDto[];
      workflowInstance?: ApprovalWorkflowInstanceDto;
}