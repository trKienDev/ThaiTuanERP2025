import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable, switchMap } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { SetUserManagerRequest, UserDto, UserRequest } from "../models/user.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";
import { FileService } from "../../../shared/services/file.service";
import { BaseCrudService } from "../../../shared/services/base-crud.service";

@Injectable({ providedIn: 'root'})
export class UserService extends BaseCrudService<UserDto, UserRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/user`);
      }

      private fileService = inject(FileService);

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

      getManagers(id: string): Observable<UserDto[]> {
            return this.http.get<ApiResponse<UserDto[]>>(`${this.endpoint}/${id}/managers`)
                  .pipe(handleApiResponse$<UserDto[]>());
      }

      setManagers(id: string, request: SetUserManagerRequest): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.endpoint}/${id}/managers`, request)
                  .pipe(handleApiResponse$<string>());
      }
}