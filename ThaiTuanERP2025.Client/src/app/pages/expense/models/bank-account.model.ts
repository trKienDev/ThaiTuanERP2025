export interface BankAccountDto {
      id: string;
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      isActive: boolean;
      userId?: string | null;
      supplierId?: string | null;
}

export interface CreateUserBankAccountRequest {
      userId: string;
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
}

export interface CreateSupplierBankAccountRequest {
      supplierId: string;
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
}

export interface UpdateBankAccountRequest {
      bankName: string;
      accountNumber: string;
      beneficiaryName: string;
      isActive: boolean;
}