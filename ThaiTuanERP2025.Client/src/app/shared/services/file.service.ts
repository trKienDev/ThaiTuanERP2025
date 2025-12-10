import { Injectable, inject } from "@angular/core";
import { HttpClient, HttpEvent, HttpEventType, HttpRequest } from "@angular/common/http";
import { Observable, catchError, map, throwError } from "rxjs";
import { ApiResponse } from "../models/api-response.model";
import { UploadFileResult } from "../models/upload-file-result.model";
import { environment } from "../../../environments/environment";

export type UploadEventProgress = { type: 'progress'; percent: number };
export type UploadEventDone = { type: 'done'; data?: UploadFileResult };

@Injectable({ providedIn: "root" })
export class FileService {
      private http = inject(HttpClient);
      private API_URL = `${environment.baseUrl}/api/files`;

      /** Single upload (field "file") + progress, khớp FilesController.UploadSingle */
      uploadFileWithProgress$(
            file: File,
            meta: { module: string; entity: string; entityId?: string; isPublic?: boolean }
      ): Observable<UploadEventProgress | UploadEventDone> {
            const form = new FormData();
            form.append("file", file); // <-- field "file"
            form.append("module", meta.module);
            form.append("entity", meta.entity);
            if (meta.entityId) form.append("entityId", meta.entityId);
            form.append("isPublic", String(!!meta.isPublic));

            const req = new HttpRequest("POST", `${this.API_URL}/upload-single`, form, { reportProgress: true });

            return this.http.request(req).pipe(
                  map<HttpEvent<any>, UploadEventProgress | UploadEventDone>((evt) => {
                        if (evt.type === HttpEventType.UploadProgress) {
                              const percent = evt.total ? Math.round((evt.loaded / evt.total) * 100) : 0;
                              return { type: "progress", percent };
                        }
                        if (evt.type === HttpEventType.Response) {
                              const api = evt.body as ApiResponse<UploadFileResult> | undefined;
                              const data: UploadFileResult | undefined = api?.data ?? undefined; // tránh null
                              return { type: "done", data };
                        }
                        // Sent/ResponseHeader/etc.
                        return { type: "progress", percent: 0 };
                  }),
                  catchError((e) => throwError(() => e))
            );
      }

      /** Single upload (không cần progress) – giữ lại nếu chỗ khác đang dùng */
      uploadFile(file: File, module: string, entity: string, entityId?: string) {
            const formData = new FormData();
            formData.append("file", file); // <-- field "file"
            formData.append("module", module);
            formData.append("entity", entity);
            if (entityId) formData.append("entityId", entityId);

            return this.http.post<ApiResponse<UploadFileResult>>(`${this.API_URL}/upload-single`, formData);
      }

      /** Multiple upload – field phải là "files" để khớp IFormFile List files */
      uploadMultipleFiles(files: File[], module: string, entity: string, entityId?: string, isPublic = false) {
            const formData = new FormData();
            files.forEach((f) => formData.append("files", f)); // <-- field "files"
            formData.append("module", module);
            formData.append("entity", entity);
            if (entityId) formData.append("entityId", entityId);
            formData.append("isPublic", String(isPublic));

            return this.http.post<ApiResponse<UploadFileResult[]>>(`${this.API_URL}/upload-multiple`, formData);
      }

      /** Lấy presigned URL để tải/xem file (tùy backend) */
      getDownloadUrl$(idOrObjectKey: string) {
            // nếu backend nhận ID DB: truyền id; nếu nhận objectKey: sửa path cho đúng
            return this.http.get<ApiResponse<string>>(`${this.API_URL}/${encodeURIComponent(idOrObjectKey)}/download-url`);
      }

      /** Download file private theo StoredFile.Id (Guid) */
      downloadById$(fileId: string): Observable<Blob> {
            return this.http.get(`${this.API_URL}/${encodeURIComponent(fileId)}/download`, { responseType: 'blob' });
      }

      /** Xóa mềm theo objectKey (hoặc id – sửa path nếu backend dùng id) */
      softDelete$(idOrObjectKey: string) {
            return this.http.delete<ApiResponse<string>>(`${this.API_URL}/${encodeURIComponent(idOrObjectKey)}`);
      }

      /** Xóa cứng (nếu cần) */
      hardDelete$(idOrObjectKey: string) {
            return this.http.delete<ApiResponse<string>>(`${this.API_URL}/${encodeURIComponent(idOrObjectKey)}/hard`);
      }
}
