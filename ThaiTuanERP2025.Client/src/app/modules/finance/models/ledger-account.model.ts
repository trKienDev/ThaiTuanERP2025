// DTO
export interface LedgerAccountDto
{
      id: string
      number: string;
      name: string;
      ledgerAccountTypeId?: string;
      parentLedgerAccountId?: string;
      description?: string;
      level: number
      path: string;
      balanceType: LedgerAccountBalanceType;
      isActive: boolean;
}

export interface LedgerAccountTreeDto
{
      id: string;
      parentId?: string;
      number: string
      name: string;
      balanceType: LedgerAccountBalanceType;
      
      level: number
      path: string;

      ledgerAccountTypeName?: string;
      description?: string;
}

// Payload
export interface LedgerAccountPayload {
      number: string;
      name: string;
      balanceType: LedgerAccountBalanceType;
      ledgerAccountTypeId?: string | null;
      parentLedgerAccountId?: string | null;
      description?: string | null;
}

// Enum
export enum LedgerAccountBalanceType {
      none = 0,
      debit = 1,
      credit = 2,
      both = 3,
}