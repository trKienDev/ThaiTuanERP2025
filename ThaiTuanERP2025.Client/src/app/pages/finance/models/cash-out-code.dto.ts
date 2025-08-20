export interface CashOutCodeDto {
      id: string;
      code: string;
      name: string;
      groupId: string; // FK --> CashOutGroup
      groupName?: string;
      isActive: boolean;
      description?: string;
}
export type CreateCashoutCodeRequest = Omit<CashOutCodeDto, 'id' | 'isActive' | 'groupName'>;
export type UpdateCashoutCodeRequest = Partial<Omit<CashOutCodeDto, 'id' | 'groupName'>>;