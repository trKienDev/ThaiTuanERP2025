import { DepartmentBriefDto } from "./department.model";
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
      roleNames: string[];

      permissions: PermissionDto[];
      phone?: number | null;

      departmentId?: string;  
      department?: DepartmentBriefDto;
      departmentName?: string;

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

export interface UserBriefAvatarDto {
      id: string;
      fullName: string;
      username: string;
      employeeCode: string;
      avatarFileId?: string;
      avatarFileObjectKey?: string;
}

export interface UserInforDto {
      id: string;
      fullName: string;
      username: string;
      employeeCode: string;
      avatarFileId?: string;
      avatarFileObjectKey?: string;

      roles: string[];
      departmentName?: string;  
      position: string;
      
      managerId?: string;
      managers?: UserBriefAvatarDto[];
}

export interface SetUserManagerRequest {
      managerIds: string[];
      primaryManagerId?: string | null;
}