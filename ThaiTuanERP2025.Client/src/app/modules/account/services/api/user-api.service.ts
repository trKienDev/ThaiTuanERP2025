import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable, switchMap } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { SetUserManagerRequest, UserBriefAvatarDto, UserDto, UserRequest } from "../../models/user.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { BaseApiService } from "../../../../shared/services/base-api.service";
import { FileApiService } from "../../../files/file-api.service";

@Injectable({ providedIn: 'root'})
export class UserApiService extends BaseApiService<UserDto, UserRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/user`);
      }

      private readonly fileApi = inject(FileApiService);

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
            return this.fileApi.uploadFile(file, 'account', 'user-avatar', userId).pipe(
                  switchMap((uploadResult) => {
                        const body = { fileId: uploadResult.data?.id };
                        return this.http.put<ApiResponse<string>>(`${this.endpoint}/${userId}/avatar`, body)
                              .pipe(handleApiResponse$<string>());
                  })
            );
      }
      
      setManagers(id: string, request: SetUserManagerRequest): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.endpoint}/${id}/managers`, request)
                  .pipe(handleApiResponse$<string>());
      }

}