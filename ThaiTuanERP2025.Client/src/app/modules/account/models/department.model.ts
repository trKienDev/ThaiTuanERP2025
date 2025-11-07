import { UserDto } from "./user.model";

export interface DepartmentDto {
      id: string;
      code: string;
      name: string;
      managerUserId?: string | null;
      manager?: UserDto;
      parentId?: string | null;
      region: number;
}

export interface DepartmentRequest {
      code: string;
      name: string;
      managerId?: string | null;
      parentId?: string | null;
}

export interface SetDepartmentManagerRequest {
      primaryManagerId: string;
      viceManagerIds: string[] | null;
}