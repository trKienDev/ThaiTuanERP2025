export interface TaxDto {
      id: string;
      policyName: string;
      rate: number;
      postingLedgerAccountId: string;
      isActive: boolean;
      description?: string;

      postingLedgerAccountCode?: string;
      postingLedgerAccountName?: string;
}


export type CreateTaxRequest = Omit<TaxDto, 'id' | 'postingLedgerAccountCode' | 'postingLedgerAccountName'>;
export type UpdateTaxRequest = Partial<Omit<TaxDto, 'postingLedgerAccountCode' | 'postingLedgerAccountName'>>;