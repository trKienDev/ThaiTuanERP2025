import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../models/api-response.model";
import { UploadFileResult } from "../../models/upload-file-result.model";
import { handleApiResponse$ } from "../../utils/handle-api-response.operator";

@Injectable({ providedIn: 'root' }) 
export class FileService {
      private readonly API_URL = `${environment.apiUrl}/files`;
      constructor(private http: HttpClient) {}

      uploadFile(file: File, module: string, entity: string, entityId: string, isPublic: boolean = false): Observable<UploadFileResult> {
            const formData = new FormData();
            formData.append('file', file);
            formData.append('module', module);
            formData.append('entity', entity);
            if(entityId) 
                  formData.append('entityId', entityId);
            formData.append('isPublic', String(isPublic));

            return this.http.post<ApiResponse<UploadFileResult>>(`${this.API_URL}/upload-single`, formData)
                  .pipe(handleApiResponse$<UploadFileResult>());
      }

      uploadMultipleFiles(files: File[], module: string, entity: string, entityId: string, isPublic: boolean = false): Observable<UploadFileResult[]> {
            const formData = new FormData();
            files.forEach(file => formData.append('file', file));
            formData.append('module', module);
            formData.append('entity', entity);
            if(entityId)
                  formData.append('entityId', entityId);
            formData.append('isPublic', String(isPublic));

            return this.http.post<ApiResponse<UploadFileResult[]>>(`${this.API_URL}/upload-multiple`, formData)
                  .pipe(handleApiResponse$<UploadFileResult[]>());
      }

      getDownloadUrl(fileId: string): Observable<string> {
            return this.http.get<ApiResponse<string>>(`${this.API_URL}/${fileId}/download-url`)
                  .pipe(handleApiResponse$<string>());
      }

      softDelete(fileId: string): Observable<string> {
            return this.http.delete<ApiResponse<string>>(`${this.API_URL}/${fileId}`)
                  .pipe(handleApiResponse$<string>());
      }

      hardDelete(fileId: string): Observable<string> {
            return this.http.delete<ApiResponse<string>>(`${this.API_URL}/${fileId}/hard`)
                  .pipe(handleApiResponse$<string>());
      }
}