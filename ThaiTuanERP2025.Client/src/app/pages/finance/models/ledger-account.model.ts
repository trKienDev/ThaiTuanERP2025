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

export interface CreateLedgerAccountRequest {
      number: string;
      name: string;
      ledgerAccountTypeId: string;
      ledgerAccountBalanceType: number; // 0..3
      parentLedgerAccountId?: string | null;
      description?: string | null;
      isActive: boolean;
}

export interface UpdateLedgerAccountRequest extends CreateLedgerAccountRequest {}

export interface LedgerAccountTreeDto {
      id: string;
      number: string;
      name: string;
      ledgerAccountTypeId: string;
      ledgerAccountTypeName?: string | null;
      ledgerAccountBalanceType: number | string; // 0..3 or "Debit"...
      description?: string | null;
      isActive: boolean;
      children?: LedgerAccountTreeDto[];
}

export interface LedgerAccountRow {
      id: string;
      number: string;
      name: string;
      typeId: string | null;
      typeName?: string | null;
      balanceType: number | string;
      description?: string | null;
      level: number;
      hasChildren: boolean;
      childrenCount: number;
      isActive: boolean;
      parentId?: string | null;
}

export type LedgerAccountBalanceTypeEnum = 1 | 2 | 3 | 4; // Debit,  Credit, Both, None 