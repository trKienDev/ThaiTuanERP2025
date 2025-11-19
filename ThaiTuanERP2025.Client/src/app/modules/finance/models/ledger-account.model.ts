export interface LedgerAccountDto {
      id: string;
      number: string;
      name: string;
      ledgerAccountTypeId: string | null;
      ledgerAccountTypeName?: string | null;
      parentLedgerAccountId?: string | null;  
      ledgerAccountBalanceType: number | string; // 0..3 | "Debit"...
      path: string;
      level: number;
      description?: string | null;
      isActive: boolean;
}

export interface LedgerAccountPayload {
      number: string;
      name: string;
      ledgerAccountTypeId: string;
      ledgerAccountBalanceType: number; // 0..3
      parentLedgerAccountId?: string | null;
      description?: string | null;
      isActive: boolean;
}

export enum LedgerAccountBalanceType {
      none = 0,
      debit = 1,
      credit = 2,
      both = 3,
}