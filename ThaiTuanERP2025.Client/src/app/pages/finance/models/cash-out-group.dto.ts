export interface CashOutGroupDto {
      id: string;
      code: string;
      name: string;
      isActive: boolean;
      description?: string;
}
export type CreateCashOutGroupRequest = Omit<CashOutGroupDto, 'id' | 'isActive'> & { isActive?: boolean };
export type UpdateCashOutGroupRequest = Partial<Omit<CashOutGroupDto, 'id'>>;