import { ApiResponse } from './../../../../shared/models/api-response.model';
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable, switchMap } from "rxjs";
import { SetUserManagerRequest, UserBriefAvatarDto, UserDto, UserRequest } from "../../models/user.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { BaseApiService } from "../../../../shared/services/base-api.service";
import { FileAttachmentApiService } from "../../../file-attachment/services/file-attachment-api.service";
import { DriveApiService } from "../../../drive/drive-api.service";
import { FileAttachmentPayload } from "../../../file-attachment/models/file-attachment.model";
import { APP_MODULES } from "../../../../core/constants/app-modules.constants";
import { DOCUMENT_TYPE } from "../../../../core/constants/document-types.constants";

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
      setAvatar(file: File, module: string, entity: string): Observable<void> {
            return this.driveApi.uploadFile(file, module, entity).pipe(
                  switchMap((objectId) => {
                        const payload: FileAttachmentPayload = {
                              driveObjectId: objectId,
                              module: APP_MODULES.ACCOUNT,
                              document: DOCUMENT_TYPE.USER_AVATAR,
                        }
                        return this.fileApi.attachFile(payload).pipe(
                              switchMap((fileId) => {
                                    return this.http.put<ApiResponse<void>>(`${this.endpoint}/avatar`, { fileId })
                                          .pipe(handleApiResponse$<void>());
                              })
                        );
                  })
            );
      }
      
      setManagers(id: string, request: SetUserManagerRequest): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.endpoint}/${id}/managers`, request)
                  .pipe(handleApiResponse$<string>());
      }

}