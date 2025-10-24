export interface CashoutGroupDto {
      id: string;
      code: string;
      name: string;
      isActive: boolean;
      description?: string;
}
export interface CashoutGroupRequest {
      name: string;
      description?: string | null;
      isActive: boolean;
      parentId?: string | null;
}
