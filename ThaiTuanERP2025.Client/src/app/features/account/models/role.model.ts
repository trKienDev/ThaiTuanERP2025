export interface RoleDto {
      id: string;
      name: string;
      description?: string;
}

export interface RoleRequest extends RoleDto {}