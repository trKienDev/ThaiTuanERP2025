export type UploadStatus = 'queued' | 'uploading' | 'done' | 'error';

export interface UploadItem {
      file: File;
      name: string;
      size: number;
      progress: number;     // 0..100
      status: UploadStatus;
      objectKey?: string;
      fileId?: string;
      url?: string;
}

export interface UploadMeta {
      module: string;
      entity: string;
      entityId?: string;
      isPublic?: boolean;
}

export interface UploadFileResult {
      id?: string;
      objectKey?: string;
      url?: string;
}