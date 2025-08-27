import { UserDto } from "./user.model";

export interface GroupModel {
      id: string;
      name: string;
      description: string;
      adminId: string;
      adminName: string;
      memberCount: number;
      members: UserDto[];
}

export interface CreateGroupModel {
      name: string;
      description: string;
      adminUserId: string;
}