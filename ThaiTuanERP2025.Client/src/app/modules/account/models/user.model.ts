import { DepartmentBriefDto, DepartmentDto } from "./department.model";
import { PermissionDto } from "./permission.model";
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

      roles: RoleDto[];
      permissions: PermissionDto[];
      phone?: number | null;
      departmentId?: string;  
      department?: DepartmentBriefDto;
      position: string;

      managerId?: string;
      managers?: UserDto[];
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