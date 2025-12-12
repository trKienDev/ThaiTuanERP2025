import { inject, Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { ToastService } from "../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { FileImagePreviewDialog } from "./file-image-preview-dialog.component";
import { FilePdfPreviewDialog } from "./file-pdf-preview-dialog.component";
import { environment } from "../../../environments/environment";
import { firstValueFrom } from "rxjs";
import { FileApiService } from "./file-api.service";

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
export class FilePreviewService {

      private dialog = inject(MatDialog);
      private toast = inject(ToastService);
      private fileApi = inject(FileApiService);
      private readonly baseUrl = environment.baseUrl;

      /** Preview File object local (image / pdf / docx) – code của bạn giữ nguyên */
      previewLocalFile(file: File) {
            const fileType = file.type;
            const url = URL.createObjectURL(file);

            if (fileType.startsWith('image/')) {
                  this.dialog.open(FileImagePreviewDialog, { data: { src: url } });
                  return;
            }

            if (fileType === 'application/pdf') {
                  this.dialog.open(FilePdfPreviewDialog, { data: { src: url } });
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
                  this.dialog.open(FileImagePreviewDialog, { data: { src: url } });
                  return;
            }

            if (type === 'application/pdf') {
                  this.dialog.open(FilePdfPreviewDialog, { data: { src: url } });
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