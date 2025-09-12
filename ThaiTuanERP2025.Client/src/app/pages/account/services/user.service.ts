import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable, switchMap } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import {  CreateUserRequest, UpdateUserRequest, UserDto } from "../models/user.model";
import { FileService } from "../../../core/services/file.service";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root'})
export class UserService {
      private readonly API_URL = `${environment.apiUrl}/user`;
      constructor(private http: HttpClient, private fileService: FileService) {}

      createUser(user: CreateUserRequest): Observable<UserDto> {
            return this.http.post<ApiResponse<UserDto>>(this.API_URL, user)
                  .pipe(handleApiResponse$<UserDto>());
      } 

      getAllUsers(): Observable<UserDto[]> {
            return this.http.get<ApiResponse<UserDto[]>>(`${this.API_URL}/all`)
                  .pipe(handleApiResponse$<UserDto[]>());
      }

      getCurrentuser(): Observable <UserDto> {
            return this.http.get<ApiResponse<UserDto>>(`${this.API_URL}/me`)
                  .pipe(handleApiResponse$<UserDto>());
      }

      getUserById(id: string): Observable <UserDto> {
            return this.http.get<ApiResponse<UserDto>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<UserDto>());
      }

      updateUser(id: string, user: UpdateUserRequest): Observable<UserDto> {
            return this.http.put<ApiResponse<UserDto>>(`${this.API_URL}/${id}`, user)
                  .pipe(handleApiResponse$<UserDto>());
      }

      deleteUser(id: string): Observable<void> {
            return this.http.delete<ApiResponse<void>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<void>());
      }

      updateAvatar(file: File, userId: string): Observable<string> {
            return this.fileService.uploadFile(file, 'account', 'user', userId, false).pipe(
                  switchMap((uploadResult) => {
                        console.log('upload result: ', uploadResult);
                        const body = { fileId: uploadResult.id };
                        return this.http.put<ApiResponse<string>>(`${this.API_URL}/${userId}/avatar`, body)
                              .pipe(handleApiResponse$<string>());
                  })
            );
      }
}