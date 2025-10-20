export interface InvoiceFileDto {
      id: string;
      invoiceId: string;
      fileId: string;
      objectKey: string;
      isMain: boolean;
      createdAt: Date;
}
