import { UserModel } from "./user.model";

export interface GroupModel {
      id: string;
      name: string;
      description: string;
      adminId: string;
      adminName: string;
      memberCount: number;
      members: UserModel[];
}

export interface CreateGroupModel {
      name: string;
      description: string;
      adminUserId: string;
}