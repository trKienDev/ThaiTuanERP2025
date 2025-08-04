import { UserDto } from "./user.dto";

export interface GroupDto {
      id: string;
      name: string;
      description: string;
      adminId: string;
      adminName: string;
      memberCount: number;
      members: UserDto[];
}

export interface CreateGroupDto {
      name: string;
      description: string;
      adminUserId: string;
}