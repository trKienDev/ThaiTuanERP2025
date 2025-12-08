import { StoredFileMetadataDto } from "../../../core/services/file-preview.service";
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

      attachments: CommentAttachmentDto[];
}

export interface CommentAttachmentDto {
      storedFile: StoredFileMetadataDto;
}

