import { UserBriefAvatarDto } from "../../account/models/user.model";

export interface CommentPayload {
      module: string;
      entity: string;
      entityId: string;
      content: string;
}

export interface CommentDto {
      module: string;
      entity: string;
      entityId: string;
      content: string;
}

export interface CommentDetailDto {
      module: string;
      entity: string;
      entityId: string;
      content: string;

      userId: string;
      user: UserBriefAvatarDto;
      createdAt: string;
}

