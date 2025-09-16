import { UserDto } from "./user.model";

export interface DivisionDto {
      id: string;
      name: string;
      description?: string;
      headUserId: string;
      headUser: UserDto;
      departmentCount: number;
      active: boolean;
}

export interface DivisionSummaryDto {
      id: string;
      name: string;
      description?: string;
      headUserName: string;
      departmentCount: number;
      active: boolean;
}

export interface CreateDivisionRequest {
      name: string;
      description?: string;
      headUserId: string;
}

export interface UpdateDivisionRequest {
      id: string;
      name: string;
      description?: string;
      headUserId: string;
}