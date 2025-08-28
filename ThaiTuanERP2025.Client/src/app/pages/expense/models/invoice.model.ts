import { invoiceLineDto, invoiceLineRequest } from "./invoice-line.model";

export interface invoiceDto {
      id: string;
      invoiceNumber: string;
      invoiceName: string;
      issueDate: string;
      paymentDate?: string | null;

      sellerName?: string | null;
      sellerTaxCode: string;
      sellerAddress?: string;

      buyerName?: string;
      buyerTaxCode?: string;
      buyerAddress?: string;

      isDraft: boolean;

      invoiceLines: invoiceLineDto[];
      
      fileIds: string[];
      followerUserIds: string[];

      subTotal: number;
      totalVAT: number;
      totalWHT: number;
      grandTotal: number;
}

export interface CreateInvoiceDraftRequest {
      invoiceNumber: string;
      invoiceName: string;
      issueDate: string;
      paymentDate?: string | null;

      sellerName?: string | null; 
      sellerTaxCode: string;
      sellerAddress?: string | null;

      buyerName?: string | null;
      buyerTaxCode?: string | null;
      buyerAddress?: string | null;

      mainFileId?: string | null;
}

export interface AddInvoiceLineRequest {
      itemName: string;
      unit?: string | null;
      quantity: number;
      unitPrice: number;
      discountRate?: number | null;
      discountAmount?: number | null;
      vatRatePercent?: number | null;
      whtTypeId?: string | null;
}

export interface AttachInvoiceFileRequest {
      fileId: string;
}

export interface ReplaceMainInvoiceFileRequest {
      newFileId: string;
}