export interface SupplierPayload {
      name: string,
      taxCode?: string | null;
}
export interface SupplierDto {
      id: string;
      name: string;
      taxCode?: string | null;
      isActive: boolean;
}

export interface SupplierBeneficiaryInforDto {
      beneficiaryAccountNumber?: string | null;
      beneficiaryName?: string | null;
      beneficiaryBankName?: string | null;
} 