import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { CreateUserDto, UserDto } from "../dtos/user.dto";
import { catchError, map, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";

@Injectable({ providedIn: 'root'})
export class UserService {
      private readonly API_URL = `${environment.apiUrl}/user`;
      constructor(private http: HttpClient) {}

      createUser(user: CreateUserDto): Observable<ApiResponse<UserDto>> {
            return this.http.post<ApiResponse<UserDto>>(this.API_URL, user);
      } 

      getAllUsers(): Observable<UserDto[]> {
            return this.http.get<ApiResponse<UserDto[]>>(`${this.API_URL}/all`).pipe(
                  map(res => {
                        if(res.isSuccess && res.data) return res.data;
                        throw new Error(res.message || 'Không thể tải danh sách user');
                  }), 
                  catchError(err => throwError(() => new Error(err?.error?.message || 'Không thể tải danh sách users')))
            );
      }

      getCurrentuser(): Observable<ApiResponse<UserDto>> {
            return this.http.get<ApiResponse<UserDto>>(`${this.API_URL}/me`);
      }

      getUserById(id: string): Observable<ApiResponse<UserDto>> {
            return this.http.get<ApiResponse<UserDto>>(`${this.API_URL}/${id}`);
      }

      updateUser(id: string, user: Partial<UserDto>): Observable<ApiResponse<UserDto>> {
            return this.http.put<ApiResponse<UserDto>>(`${this.API_URL}/${id}`, user);
      }

      deleteUser(id: string): Observable<ApiResponse<void>> {
            return this.http.delete<ApiResponse<void>>(`${this.API_URL}/${id}`);
      }

      updateAvatar(file: File): Observable<ApiResponse<string>> {
            const formData = new FormData();
            return this.http.post<ApiResponse<string>>(`${this.API_URL}/upload-avatar`, formData);
      }
}