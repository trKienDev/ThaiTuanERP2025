export type LedgerAccountBalanceType = 'Debit' | 'Credit' | 'Both' | 'None';

export interface LedgerAccountTreeDto {
      id: string;
      code: string;  // ← map từ Number 
      name: string;
      description?: string | null;
      isActive: boolean;
      ledgerAccountTypeId: string;
      parentId?: string | null;
      level: number;
      path: string;
      balanceType: LedgerAccountBalanceType;
      children?: LedgerAccountTreeDto[]; // sẽ tự build từ flat array
}
