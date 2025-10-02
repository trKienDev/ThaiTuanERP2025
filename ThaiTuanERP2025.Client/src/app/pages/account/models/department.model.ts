import { UserDto } from "./user.model";

export interface DepartmentDto {
      id: string;
      code: string;
      name: string;
      managerUserId?: string | null;
      manager: UserDto;
      parentId?: string | null;
      region: number;
}

export interface DepartmentRequest {
      code: string;
      name: string;
      managerId?: string | null;
      parentId?: string | null;
      region: number;
}

export enum DepartmentRegion {
      none = 0,
      north = 1,
      middle = 2,
      south = 3,
}

export interface SetDepartmentManagerRequest {
      departmentId: string;
      managerId: string;
}