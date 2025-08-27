export interface PartnerBankAccountDto {
      id: string;
      supplierId: string;
      accountNumber: string;
      bankName: string;
      accountHolder?: string;
      swiftCode?: string;
      note?: string;
      isActive: boolean;

      createdDate: string;
      createdByUserId: string;
      dateModified?: string;
}

export interface UpsertPartnerBankAccountRequest {
      accountNumber: string;
      bankName: string;
      accountHolder?: string;
      swiftCode?: string;
      note?: string;
      isActive: boolean;
}


