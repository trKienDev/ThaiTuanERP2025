export interface invoiceLineDto {
      id: string;
      itemName: string;
      unit?: string | null;
      quantity: number;
      unitPrice: number;
      discountRate?: number | null;
      netAmount: number;
      taxId?: string | null;
      vatAmount?: number | null;
      whtTypeId?: string | null;
      whtAmount?: string | null;
      lineTotal: number;
}

export interface invoiceLineRequest {
      itemName: string;
      unit?: string | null;
      quantity: number;
      unitPrice: number;
      discountRate?: number | null;
      discountAmount?: number | null;
      taxId?: string | null;
      whtTypeId?: string | null;
}