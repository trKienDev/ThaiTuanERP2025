import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { User } from "../models/user.model";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";

@Injectable({ providedIn: 'root'})
export class UserService {
      private readonly API_URL = `${environment.apiUrl}/user`;
      constructor(private http: HttpClient) {}

      createUser(user: Partial<User>): Observable<ApiResponse<User>> {
            return this.http.post<ApiResponse<User>>(this.API_URL, user);
      } 

      getAllUsers(): Observable<ApiResponse<User[]>> {
            return this.http.get<ApiResponse<User[]>>(this.API_URL);
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