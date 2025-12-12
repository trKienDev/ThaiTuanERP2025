import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable, switchMap } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { SetUserManagerRequest, UserBriefAvatarDto, UserDto, UserRequest } from "../../models/user.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { BaseApiService } from "../../../../shared/services/base-api.service";
import { FileAttachmentApiService } from "../../../file-attachment/services/file-attachment-api.service";
import { DriveApiService } from "../../../drive/drive-api.service";

@Injectable({ providedIn: 'root'})
export class UserApiService extends BaseApiService<UserDto, UserRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.server.apiUrl}/user`);
      }

      private readonly driveApi = inject(DriveApiService);
      private readonly fileApi = inject(FileAttachmentApiService);

      // ===== GET =====
      getCurrentuser(): Observable <UserDto> {
            return this.http.get<ApiResponse<UserDto>>(`${this.endpoint}/me`)
                  .pipe(handleApiResponse$<UserDto>());
      }

      getManagerIds(id: string): Observable<string[]> {
            return this.http.get<ApiResponse<string[]>>(`${this.endpoint}/${id}/managers/ids`)
                  .pipe(handleApiResponse$<string[]>());
      }

      getMyManagers(): Observable<UserBriefAvatarDto[]> {
            return this.http.get<ApiResponse<UserBriefAvatarDto[]>>(`${this.endpoint}/me/managers`)
                  .pipe(handleApiResponse$<UserBriefAvatarDto[]>());
      }

      getDepartmentManagersByUser(): Observable<UserBriefAvatarDto[]> {
            return this.http.get<ApiResponse<UserBriefAvatarDto[]>>(`${this.endpoint}/me/department/managers`)
                  .pipe(handleApiResponse$<UserBriefAvatarDto[]>());
      }

      search(keyword: string): Observable<UserBriefAvatarDto[]> {
            return this.http.get<ApiResponse<UserBriefAvatarDto[]>>(`${this.endpoint}/search`)
                  .pipe(handleApiResponse$<UserBriefAvatarDto[]>());
      }

      
      // ==== PUT ====
      updateAvatar(file: File, userId: string): Observable<string> {
            return this.driveApi.uploadFile(file);
      }
      
      setManagers(id: string, request: SetUserManagerRequest): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.endpoint}/${id}/managers`, request)
                  .pipe(handleApiResponse$<string>());
      }

}