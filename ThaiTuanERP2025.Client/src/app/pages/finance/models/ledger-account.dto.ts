export type LedgerAccountBalanceType = 'Debit' | 'Credit' | 'Both' | 'None';
export interface LedgerAccountDto {
      id: string;
      code: string;
      name: string;
      typeId: string; // FK -> LedgerAccountType
      typeName?: string; // convenience for views
      isActive: boolean;
      description?: string;
      balanceType: LedgerAccountBalanceType;
}
export type CreateLedgerAccountRequest = Omit<LedgerAccountDto, 'id' | 'isActive' | 'typeName'> & { isActive?: boolean }
export type UpdateLedgerAccountRequest = Partial<Omit<LedgerAccountDto, 'id' | 'typeName'>>;