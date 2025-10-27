export interface PermissionDto {
      id: string;
      name: string;
      code: string;
      description: string;
}

export interface PermissionRequest {
      name: string;
      code: string;
      description: string;
}