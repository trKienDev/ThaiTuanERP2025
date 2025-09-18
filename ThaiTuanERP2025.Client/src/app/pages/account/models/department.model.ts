export interface DepartmentDto {
      id: string;
      code: string;
      name: string;
      managerId?: string;
      parentId?: string;
      region: number;
}

export interface CreateDepartmentRequest {
      code: string;
      name: string;
      managerId?: string;
      parentId?: string;
      region: number;
}

export interface UpdateDepartmentRequest extends CreateDepartmentRequest {
      id: string;
}

export enum DepartmentRegion {
      none = 0,
      north = 1,
      middle = 2,
      south = 3,
}
