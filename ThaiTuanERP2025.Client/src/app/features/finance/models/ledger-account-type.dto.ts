export interface LedgerAccountTypeDto {
      id: string;
      code: string;
      name: string;
      ledgerAccountTypeKind: number;
      isActive: boolean;
      description?: string | null;
}

export interface LedgerAccountTypeRequest {
      code: string;
      name: string;
      ledgerAccountTypeKind: number;
      description?: string | null;
}
