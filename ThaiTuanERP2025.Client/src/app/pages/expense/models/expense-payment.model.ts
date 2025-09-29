import { UserDto } from "../../account/models/user.model";
import { ExpensePaymentAttachmentDto, ExpensePaymentAttachmentRequest } from "./expense-payment-attachment.model";
import { ExpensePaymentFollowerDto } from "./expense-payment-followers.model";
import { ExpensePaymentItemDto, ExpensePaymentItemRequest } from "./expense-paymnet-item.model";
import { SupplierDto } from "./supplier.model";

export type PayeeType = 'supplier' | 'employee';
export enum ExpensePaymentStatus {
      draft = 0,
      submitted = 1,
      approved = 2,
      rejected = 3,
      cancelled = 4,
      paid = 5,
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

      status: ExpensePaymentStatus;

      items: ExpensePaymentItemDto[];
      attachments: ExpensePaymentAttachmentDto[];
      followers: ExpensePaymentFollowerDto[];
}

export interface ExpensePaymentRequest {
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

      status: ExpensePaymentStatus;

      items: ExpensePaymentItemRequest[];
      attachments: ExpensePaymentAttachmentRequest[];
      followerIds: string[];
}