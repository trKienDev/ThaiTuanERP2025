import { DepartmentDto } from "./department.model";
import { RoleDto } from "./role.model";

export interface UserDto {
      id: string;
      fullName: string;
      username: string;
      employeeCode: string;
      email?: string;
      password: string;

      avatarFileId?: string;
      avatarFileObjectKey?: string;

      role: RoleDto;
      phone?: number | null;
      departmentId?: string;  
      department?: DepartmentDto;
      position: string;

      managerId?: string;
      manager?: UserDto;
}

export interface UserRequest {
      fullName: string;
      username: string;
      employeeCode: string;
      email?: string | null;
      password: string;
      roleId?: string | null;
      phone?: number | null;
      departmentId?: string | null;
      position: string;
}

export interface SetUserManagerRequest {
      managerIds: string[];
      primaryManagerId?: string | null;
}