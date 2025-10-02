export interface TaxDto {
      id: string;
      policyName: string;
      rate: number; // 0..1
      postingLedgerAccountId: string;
      postingLedgerAccountNumber: string;
      postingLedgerAccountName: string;
      description?: string | null;
      isActive: boolean;
}

export interface TaxRequest {
      policyName: string;
      rate: number; // 0..1
      postingLedgerAccountId: string;
      description?: string | null;
      isActive: boolean;
}
