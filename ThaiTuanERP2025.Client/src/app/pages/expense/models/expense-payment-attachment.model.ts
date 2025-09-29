export interface ExpensePaymentAttachmentDto {
      id: string;
      expensePaymentId: string;
      fileId?: string;

      objectKey: string;
      fileName: string;
      size: number;
      url?: string;
}

export interface ExpensePaymentAttachmentRequest {
      expensePaymentId: string;
      fileId?: string;

      objectKey: string;
      fileName: string;
      size: number;
      url?: string;
}