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
export interface CashoutCodeRequest {
      name: string;
      cashoutGroupId: string;
      postingLedgerAccountId: string;
      description?: string | null;
}
