import { UserDto } from "./user.model";

export interface DepartmentDto {
      id: string;
      code: string;
      name: string;
      parentId?: string | null;
      primaryManager?: UserDto | null;
      viceManagers: UserDto[];
}

export interface DepartmentBriefDto {
      id: string;
      code: string;
      name: string;
}

export interface DepartmentRequest {
      code: string;
      name: string;
      managerId?: string | null;
      parentId?: string | null;
}

export interface SetDepartmentManagerPayload {
      primaryManagerId: string;
      viceManagerIds: string[] | null;
      cascadeToMembers: boolean;  // áp dụng thay đổi manager cho các thành viên trong phòng ban 
      replaceMode: boolean; // xóa các manager cũ của từng thành viên hay giữ lại
}