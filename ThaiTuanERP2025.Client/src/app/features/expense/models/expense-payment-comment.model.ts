import { UserDto } from "../../account/models/user.model";
import { ExpensePaymentAttachmentDto } from "./expense-payment-attachment.model";
import { ExpensePaymentCommentAttachmentRequest } from "./expense-payment-comment-attachment.dto";
import { ExpensePaymentCommentTagDto } from "./expense-payment-comment-tag.model";

export interface ExpensePaymentComment {
      expensePaymentId: string;
      parentCommentId?: string;
      content: string;  
      taggedUserIds?: string[];
}
export interface ExpensePaymentCommentRequest extends ExpensePaymentComment {
      attachments?: ExpensePaymentCommentAttachmentRequest[];
}
export interface ExpensePaymentCommentDto extends ExpensePaymentComment {
      id: string;
      isEdited: boolean;
      commentType: number;
      createdByUserId: string;
      createdByUser: UserDto;
      createdDate: Date;
      attachments?: ExpensePaymentAttachmentDto[];
      tags: ExpensePaymentCommentTagDto[];
      replies: ExpensePaymentCommentDto[];
}
