import { Department } from "./department.model";
import { UserRole } from "./user-roles.enum";

export interface User {
      id?: string;
      fullName: string;
      username: string;
      employeeCode: string;
      email?: string;
      password?: string;
      avatarUrl?: string;
      role: UserRole;
      phone?: number;
      department?: Department;
      position: string;
}