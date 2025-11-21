export interface CashoutCodeDto {
      id: string;
      code: string;
      name: string;
      isActive: boolean;
      description?: string;

      cashoutGroupId: string; // FK --> CashOutGroup
      cashoutGroupName?: string;

      postingLedgerAccountId: string;
      postingLedgerAccountName: string;
}

export interface CashoutCodeTreeDto {
      id: string;
      name: string;
}

export interface CashoutCodePayload {
      name: string;
      groupId: string;
      ledgerAccountId: string;
      description?: string | null;
}
