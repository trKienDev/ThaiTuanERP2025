export interface CashoutGroupDto {
      id: string;
      code: string;
      name: string;
      isActive: boolean;
      description?: string;
}
export interface CreateCashoutGroupRequest {
      name: string;
      description?: string | null;
      isActive: boolean;
      parentId?: string | null;
}
export interface UpdateCashoutGroupRequest extends CreateCashoutGroupRequest {
      id: string;
      code: string;
}