import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable, switchMap } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { SetUserManagerRequest, UserBriefAvatarDto, UserDto, UserRequest } from "../../models/user.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { FileService } from "../../../../shared/services/file.service";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root'})
export class UserApiService extends BaseApiService<UserDto, UserRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/user`);
      }

      private readonly fileService = inject(FileService);

      getCurrentuser(): Observable <UserDto> {
            return this.http.get<ApiResponse<UserDto>>(`${this.endpoint}/me`)
                  .pipe(handleApiResponse$<UserDto>());
      }

      updateAvatar(file: File, userId: string): Observable<string> {
            return this.fileService.uploadFile(file, 'account', 'user', userId, false).pipe(
                  switchMap((uploadResult) => {
                        const body = { fileId: uploadResult.data?.id };
                        return this.http.put<ApiResponse<string>>(`${this.endpoint}/${userId}/avatar`, body)
                              .pipe(handleApiResponse$<string>());
                  })
            );
      }

      getManagerIds(id: string): Observable<string[]> {
            return this.http.get<ApiResponse<string[]>>(`${this.endpoint}/${id}/managers/ids`)
                  .pipe(handleApiResponse$<string[]>());
      }

      getMyManagers(): Observable<UserBriefAvatarDto[]> {
            return this.http.get<ApiResponse<UserBriefAvatarDto[]>>(`${this.endpoint}/me/managers`)
                  .pipe(handleApiResponse$<UserBriefAvatarDto[]>());
      }

      setManagers(id: string, request: SetUserManagerRequest): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.endpoint}/${id}/managers`, request)
                  .pipe(handleApiResponse$<string>());
      }

      getDepartmentManagersByUser(): Observable<UserBriefAvatarDto[]> {
            return this.http.get<ApiResponse<UserBriefAvatarDto[]>>(`${this.endpoint}/me/department/managers`)
                  .pipe(handleApiResponse$<UserBriefAvatarDto[]>());
      }
}