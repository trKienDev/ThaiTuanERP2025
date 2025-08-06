import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { catchError, map, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";
import { CreateUserModel, UserModel } from "../models/user.model";

@Injectable({ providedIn: 'root'})
export class UserService {
      private readonly API_URL = `${environment.apiUrl}/user`;
      constructor(private http: HttpClient) {}

      createUser(user: CreateUserModel): Observable<ApiResponse<UserModel>> {
            return this.http.post<ApiResponse<UserModel>>(this.API_URL, user);
      } 

      getAllUsers(): Observable<ApiResponse<UserModel[]>> {
            return this.http.get<ApiResponse<UserModel[]>>(`${this.API_URL}/all`);
      }

      getCurrentuser(): Observable<ApiResponse<UserModel>> {
            return this.http.get<ApiResponse<UserModel>>(`${this.API_URL}/me`);
      }

      getUserById(id: string): Observable<ApiResponse<UserModel>> {
            return this.http.get<ApiResponse<UserModel>>(`${this.API_URL}/${id}`);
      }

      updateUser(id: string, user: Partial<UserModel>): Observable<ApiResponse<UserModel>> {
            return this.http.put<ApiResponse<UserModel>>(`${this.API_URL}/${id}`, user);
      }

      deleteUser(id: string): Observable<ApiResponse<void>> {
            return this.http.delete<ApiResponse<void>>(`${this.API_URL}/${id}`);
      }

      updateAvatar(file: File): Observable<ApiResponse<string>> {
            const formData = new FormData();
            return this.http.post<ApiResponse<string>>(`${this.API_URL}/upload-avatar`, formData);
      }
}