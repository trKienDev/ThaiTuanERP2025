export type LedgerAccountKind = 'Asset' | 'Liability' | 'Equity' | 'Revenue' | 'Expense';

export interface LedgerAccountTypeDto {
      id: string;
      code: string;
      name: string;
      kind: LedgerAccountKind;
      isActive: boolean;
      description?: string | null;
}

export interface CreateLedgerAccountTypeRequest {
      code: string;
      name: string;
      kind: LedgerAccountKind;
      description?: string | null;
}

export interface UpdateLedgerAccountTypeRequest extends CreateLedgerAccountTypeRequest {}