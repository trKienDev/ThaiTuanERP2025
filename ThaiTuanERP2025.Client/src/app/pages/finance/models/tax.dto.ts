export interface TaxDto {
      policyName: string;
      rate: number;
      postingLedgerAccountId: string;
      isActive: boolean;
      description?: string | null;
}

export interface TaxRequestDto extends TaxDto {
      id: string;
      postingLedgerAccountNumber?: string;
      postingLedgerAccountName?: string;
}