import { StoredFileMetadataDto } from "../../files/file-preview.service";
import { UserBriefAvatarDto } from "../../account/models/user.model";

export interface CommentPayload {
      documentType: string;
      documentId: string;
      content: string;
      attachmentIds?: string[];
      mentionIds?: string[];
}

export interface MentionState {
    active: boolean;
    keyword: string;
    caretX: number;
    caretY: number;
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

      attachments: CommentAttachmentDto[];
      
      mentions: CommentMentionDto[];
}

export interface CommentAttachmentDto {
      storedFile: StoredFileMetadataDto;
}

export interface CommentMentionDto {
      id: string;
      fullName: string;
}

