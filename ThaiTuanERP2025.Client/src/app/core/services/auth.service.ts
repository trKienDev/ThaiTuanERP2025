import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/api-response.model';
import { LoginResponse } from '../models/login-response.model';


@Injectable({ providedIn: 'root' })
export class AuthService {
      private readonly api = `${environment.apiUrl}/account`;

      constructor(private http: HttpClient) {}

      login(username: string, password: string): Observable<ApiResponse<LoginResponse>> {
            return this.http.post<ApiResponse<LoginResponse>>(`${this.api}/login`, { username, password });
      }

      saveToken(token: string) {
            localStorage.setItem('access_token', token);
      }

      getToken(): string | null {
            return localStorage.getItem('access_token');
      }

      logout() {
            localStorage.removeItem('access_token');
      }
}