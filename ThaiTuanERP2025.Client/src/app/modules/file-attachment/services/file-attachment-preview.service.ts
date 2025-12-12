import { inject, Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { firstValueFrom } from "rxjs";
import { FileAttachmentApiService } from "./file-attachment-api.service";
import { environment } from "../../../../environments/environment";
import { ToastService } from "../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { ImageAttachmentPreviewDialog } from "../components/image-attachment-preview-dialog.component";
import { PdfAttachmentPreviewDialog } from "../components/pdf-attachment-preview-dialog.component";

export interface StoredFileDownloadDto {
      fileId: string; 
      objectKey: string;
      fileName: string;
      contentType?: string; // optional FE-side
}
export interface StoredFileMetadataDto {
      fileId: string | null;
      objectKey: string | null;
      fileName: string | null;
}

@Injectable({ providedIn: 'root' })
export class FileAttachmentPreviewService {

      private dialog = inject(MatDialog);
      private toast = inject(ToastService);
      private fileApi = inject(FileAttachmentApiService);
      private readonly baseUrl = environment.server.baseUrl;

      /** Preview File object local (image / pdf / docx) – code của bạn giữ nguyên */
      previewLocalFile(file: File) {
            const fileType = file.type;
            const url = URL.createObjectURL(file);

            if (fileType.startsWith('image/')) {
                  this.dialog.open(ImageAttachmentPreviewDialog, { data: { src: url } });
                  return;
            }

            if (fileType === 'application/pdf') {
                  this.dialog.open(PdfAttachmentPreviewDialog, { data: { src: url } });
                  return;
            }

            this.toast.info('Trình duyệt không thể preview file này, hệ thống sẽ tải xuống');
            this.download(url, file.name);
      }

      /** Preview Blob từ server – code hiện tại của bạn */
      previewBlob(blob: Blob, fileName = 'file') {
            const url = URL.createObjectURL(blob);
            const type = blob.type;

            if (type.startsWith('image/')) {
                  this.dialog.open(ImageAttachmentPreviewDialog, { data: { src: url } });
                  return;
            }

            if (type === 'application/pdf') {
                  this.dialog.open(PdfAttachmentPreviewDialog, { data: { src: url } });
                  return;
            }

            this.download(url, fileName);
      }

      /**
       * Preview file được lưu trên server (StoredFile):
       * - nếu public → mở link trực tiếp /files/public/{objectKey}
       * - nếu private → gọi API download (Blob) → previewBlob
       */
      async previewStoredFile(info: StoredFileDownloadDto): Promise<void> {
            const { fileId, objectKey, fileName } = info;

            // Case 2: file private → bắt buộc cần fileId để gọi /api/files/{id}/download
            if (!fileId) {
                  this.toast.errorRich('Không tìm thấy fileId để tải file từ server');
                  return;
            }

            try {
                  const blob = await firstValueFrom(this.fileApi.downloadById$(fileId));

                  this.previewBlob(blob, fileName ?? 'file');
            } catch (error) {
                  console.error('[FilePreviewService] previewStoredFile error: ', error);
                  this.toast.errorRich('Không thể tải file từ server');
            }
      }

      private download(url: string, fileName: string) {
            const a = document.createElement('a');
            a.href = url;
            a.download = fileName;
            a.click();
      }
}