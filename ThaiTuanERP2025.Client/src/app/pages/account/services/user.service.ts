import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { User } from "../models/user.model";
import { catchError, map, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";

@Injectable({ providedIn: 'root'})
export class UserService {
      private readonly API_URL = `${environment.apiUrl}/user`;
      constructor(private http: HttpClient) {}

      createUser(user: Partial<User>): Observable<ApiResponse<User>> {
            return this.http.post<ApiResponse<User>>(this.API_URL, user);
      } 

      getAllUsers(): Observable<User[]> {
            return this.http.get<ApiResponse<User[]>>(`${this.API_URL}/all`).pipe(
                  map(res => {
                        if(res.isSuccess && res.data) return res.data;
                        throw new Error(res.message || 'Không thể tải danh sách user');
                  }), 
                  catchError(err => throwError(() => new Error(err?.error?.message || 'Không thể tải danh sách users')))
            );
      }

      getUserById(id: string): Observable<ApiResponse<User>> {
            return this.http.get<ApiResponse<User>>(`${this.API_URL}/${id}`);
      }

      updateUser(id: string, user: Partial<User>): Observable<ApiResponse<User>> {
            return this.http.put<ApiResponse<User>>(`${this.API_URL}/${id}`, user);
      }

      deleteUser(id: string): Observable<ApiResponse<void>> {
            return this.http.delete<ApiResponse<void>>(`${this.API_URL}/${id}`);
      }
}