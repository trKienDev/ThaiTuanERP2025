// export type LedgerAccountTypeKind = 'none' | 'asset' | 'liability' | 'equity' | 'revenue' | 'expense'
export enum LedgerAccountTypeKind {
      none = 0,
      asset = 1,
      liability = 2,
      equity = 3,
      revenue = 4,
      expense = 5,
}

export interface LedgerAccountTypeDto {
      id: string;
      code: string;
      name: string;
      kind: LedgerAccountTypeKind;
      isActive: boolean;
      description?: string | null;
}

export interface LedgerAccountTypePayload {
      code: string;
      name: string;
      kind: LedgerAccountTypeKind;
      description?: string | null;
}
