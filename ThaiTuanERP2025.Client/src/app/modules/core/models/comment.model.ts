import { UserBriefAvatarDto } from "../../account/models/user.model";

export interface CommentPayload {
      documentType: string;
      documentId: string;
      content: string;
      attachmentIds?: string[];
}

export interface CommentDto {
      documentType: string;
      documentId: string;
      content: string;
}

export interface CommentDetailDto {
      id: string;
      documentType: string;
      documentId: string;
      content: string;

      userId: string;
      user: UserBriefAvatarDto;
      createdAt: string;

      parentId: string;
      replies: CommentDetailDto[];

      // CLIENT ONLY
      _forceExpand?: boolean; 
}

