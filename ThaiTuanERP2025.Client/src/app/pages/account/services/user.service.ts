import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { CreateUserModel, UserModel } from "../models/user.model";
import { handleApiResponse$ } from "../../../core/utils/handle-api-response.operator";

@Injectable({ providedIn: 'root'})
export class UserService {
      private readonly API_URL = `${environment.apiUrl}/user`;
      constructor(private http: HttpClient) {}

      createUser(user: CreateUserModel): Observable<UserModel> {
            return this.http.post<ApiResponse<UserModel>>(this.API_URL, user)
                  .pipe(handleApiResponse$<UserModel>());
      } 

      getAllUsers(): Observable<UserModel[]> {
            return this.http.get<ApiResponse<UserModel[]>>(`${this.API_URL}/all`)
                  .pipe(handleApiResponse$<UserModel[]>());
      }

      getCurrentuser(): Observable <UserModel> {
            return this.http.get<ApiResponse<UserModel>>(`${this.API_URL}/me`)
                  .pipe(handleApiResponse$<UserModel>());
      }

      getUserById(id: string): Observable <UserModel> {
            return this.http.get<ApiResponse<UserModel>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<UserModel>());
      }

      updateUser(id: string, user: Partial<UserModel>): Observable<UserModel> {
            return this.http.put<ApiResponse<UserModel>>(`${this.API_URL}/${id}`, user)
                  .pipe(handleApiResponse$<UserModel>());
      }

      deleteUser(id: string): Observable<void> {
            return this.http.delete<ApiResponse<void>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<void>());
      }

      updateAvatar(file: File): Observable<string> {
            const formData = new FormData();
            return this.http.post<ApiResponse<string>>(`${this.API_URL}/upload-avatar`, formData)
                  .pipe(handleApiResponse$<string>());
      }
}