export interface RoleDto {
      id: string;
      name: string;
      description?: string;
      isActive: boolean;
}

export interface RoleRequest extends RoleDto {}