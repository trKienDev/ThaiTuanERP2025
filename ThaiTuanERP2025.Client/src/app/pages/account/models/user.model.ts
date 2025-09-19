import { DepartmentDto } from "./department.model";
import { UserRole } from "./user-roles.enum";

export interface UserDto {
      id: string;
      fullName: string;
      username: string;
      employeeCode: string;
      email?: string;
      password: string;

      avatarFileId?: string;
      avatarFileObjectKey?: string;

      role: UserRole;
      phone?: number | null;
      departmentId?: string;  
      department?: DepartmentDto;
      position: string;

      managerId?: string;
      manager?: UserDto;
}

export interface CreateUserRequest {
      fullName: string;
      username: string;
      employeeCode: string;
      email?: string | null;
      password: string;
      role: string;
      phone?: number | null;
      departmentId?: string | null;
      position: string;
}

export interface UpdateUserRequest extends CreateUserRequest {}