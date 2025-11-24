import { UserDto } from "../../account/models/user.model";
import { ApprovalWorkflowInstanceDetailDto, ApprovalWorkflowIntanceStatusDto } from "./expense-workflow-instance.model";
import { ExpensePaymentAttachmentDto, ExpensePaymentAttachmentRequest } from "./expense-payment-attachment.model";
import { ExpensePaymentItemDetailDto, ExpensePaymentItemRequest } from "./expense-paymnet-item.model";
import { OutgoingPaymentStatusDto } from "./outgoing-payment.model";
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
      readyForPayment = 6,
      partiallyPaid = 7,
      fullyPaid = 8,
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
      
      dueDate: Date;
      hasGoodReceipt: boolean;
      totalAmount: number;
      totalTax: number;
      totalWithTax: number;

      items: ExpensePaymentItemDetailDto[];
      attachments: ExpensePaymentAttachmentDto[];
}

export interface ExpensePaymentRequest {
      name: string;
      payeeType: PayeeType;
      supplierId?: string;
      
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      description?: string;
      
      dueDate: Date;
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
      outgoingAmountPaid: number;
      remainingOutgoingAmount: number;

      createdDate: Date;
      updatedBy?: UserDto;
      updatedAt?: Date;

      followers: UserDto[];

      workflowInstanceDetail?: ApprovalWorkflowInstanceDetailDto;

      outgoingPayments: OutgoingPaymentStatusDto[];
}

export interface ExpensePaymentSummaryDto {
	id: string;
	name: string;

	payeeType: PayeeType;
	supplierId?: string;
	supplier?: SupplierDto;

	bankName: string;
	accountNumber: string;
	beneficiaryName: string;

	dueDate: Date;
	hasGoodsReceipt: boolean;
	description?: string;

	totalAmount: number;
	totalTax: number;
	totalWithTax: number;

      outgoingAmountPaid: number;

	status: number;

	createdByUser: UserDto;
      createdDate: Date;

      workflowInstanceStatus: ApprovalWorkflowIntanceStatusDto;
}
