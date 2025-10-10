import { UserDto } from "../../account/models/user.model";
import { ApprovalWorkflowInstanceDetailDto, ApprovalWorkflowInstanceDto } from "./approval-workflow-instance.model";
import { ExpensePaymentAttachmentDto, ExpensePaymentAttachmentRequest } from "./expense-payment-attachment.model";
import { ExpensePaymentFollowerDto } from "./expense-payment-followers.model";
import { ExpensePaymentItemDetailDto, ExpensePaymentItemDto, ExpensePaymentItemRequest } from "./expense-paymnet-item.model";
import { InvoiceDto } from "./invoice.model";
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
      description?: string;
      
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      
      paymentDate: Date;
      hasGoodReceipt: boolean;
      totalAmount: number;
      totalTax: number;
      totalWithTax: number;

      items: ExpensePaymentItemDetailDto[];
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
      description?: string;
      
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
      subId: string;

      createdByUserId: string;
      createdByUser: UserDto;

      

      createdByDepartmentId: string;
      createdByDepartmentName?: string;

      status: number;

      createdDate: Date;
      updatedBy?: UserDto;
      updatedAt?: Date;

      workflowInstanceDetail?: ApprovalWorkflowInstanceDetailDto;
}