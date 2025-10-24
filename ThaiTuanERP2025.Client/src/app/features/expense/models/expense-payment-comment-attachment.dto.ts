export interface ExpensePaymentCommentAttachment {
      fileName: string;
      fileUrl: string;
      fileSize: number;
      mimeType?: string;
}
export interface ExpensePaymentCommentAttachmentDto extends ExpensePaymentCommentAttachment {
      id: string;
}
export interface ExpensePaymentCommentAttachmentRequest extends ExpensePaymentCommentAttachment {
      fileId?: string;
}