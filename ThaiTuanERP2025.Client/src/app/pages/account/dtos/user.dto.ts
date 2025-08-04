import { Department } from "./department.model";
import { UserRole } from "./user-roles.enum";

export interface UserDto {
      id?: string;
      fullName: string;
      username: string;
      employeeCode: string;
      email?: string;
      password: string;
      avatarUrl?: string;
      role: UserRole;
      phone?: string;
      departmentId?: string;  
      department?: Department;
      position: string;
}

export interface CreateUserDto {
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