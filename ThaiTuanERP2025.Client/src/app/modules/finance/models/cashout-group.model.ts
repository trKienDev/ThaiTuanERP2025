export interface CashoutGroupDto {
      id: string;
      code: string;
      name: string;
      isActive: boolean;
      description?: string;
}

export interface CashoutGroupTreeDto {
      id: string;
      parentId?: string;
      name: string;
      level: number;
      path?: string;
      description?: string;
}

export interface CashoutGroupPayload {
      name: string;
      parentId?: string | null;
      description?: string | null;
}
