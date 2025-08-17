export interface SupplierDto {
      id: string;
      code: string;
      name: string;
      shortName: string;
      isActive: boolean;

      taxCode?: string;
      withholdingTaxType?: string;
      witholdingTaxRate: number;

      defaultCurrency: string;
      paymentTermDays: number;

      postingProfileId?: string;
      supplierGroupId?: string;

      email?: string;
      phone?: string;
      addressLine1?: string;
      addressLine2?: string;
      city?: string;
      stateOrProvince?: string;
      postalCode?: string;
      country?: string;
      note?: string;

      createdDate: string;
      createdByUserId: string;
      dateModified?: string;
}

export interface CreateSupplierRequest {
      code: string;
      name: string;
      defaultCurrency: string;

      shortName?: string;
      taxCode?: string;
      withholdingTaxType?: string;
      withholdingTaxRate?: number;
      paymentTermDays?: number;
      postingProfileId?: string;
      supplierGroupId?: string;
      email?: string;
      phone?: string;
      addressLine1?: string;
      addressLine2?: string;
      city?: string;
      stateOrProvince?: string;
      postalCode?: string;
      country?: string;
      note?: string;
}

export interface UpdateSupplierRequest {
      name: string;
      defaultCurrency: string;

      shortName?: string;
      taxCode?: string;
      withholdingTaxType?: string;
      withholdingTaxRate?: number;
      paymentTermDays?: number;
      postingProfileId?: string;
      supplierGroupId?: string;
      email?: string;
      phone?: string;
      addressLine1?: string;
      addressLine2?: string;
      city?: string;
      stateOrProvince?: string;
      postalCode?: string;
      country?: string;
      isActive: boolean;
      note?: string;
}



