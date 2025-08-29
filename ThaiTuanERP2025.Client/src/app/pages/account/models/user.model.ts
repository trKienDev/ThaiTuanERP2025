import { DepartmentDto } from "./department.model";
import { UserRole } from "./user-roles.enum";

export interface UserDto {
      id?: string;
      fullName: string;
      username: string;
      employeeCode: string;
      email?: string;
      password: string;

      avatarFileId?: string;
      avatarFileObjectKey?: string;

      role: UserRole;
      phone?: string;
      departmentId?: string;  
      department?: DepartmentDto;
      position: string;
}

export interface CreateUserRequest {
      fullName: string;
      username: string;
      employeeCode: string;
      email?: string;
      password: string;
      role: string;
      phone?: string;
      departmentId?: string;
      position: string;
}

export interface UpdateUserRequest {
      fullName?: string;
      email?: string;
      phone?: string;
      position?: string;
      role?: UserRole;
      departmentId?: string;
      avatarFileId?: string;
}