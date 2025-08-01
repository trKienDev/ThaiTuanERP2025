import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../models/api-response.model';
import { LoginResponse } from '../../models/login-response.model';


@Injectable({ providedIn: 'root' })
export class AuthService {
      private readonly API_URL = `${environment.apiUrl}/account`;
      private readonly TOKEN_KEY = 'access_token';
      private readonly ROLE_KEY = 'user_role';

      private tokenSubject = new BehaviorSubject<string | null>(null);
      private roleSubject = new BehaviorSubject<string | null>(null);

      public token$ = this.tokenSubject.asObservable();
      public role$ = this.tokenSubject.asObservable();

      constructor(private http: HttpClient) {
            // Load from localStorage on app start
            const storedToken = localStorage.getItem(this.TOKEN_KEY);
            const storedRole = localStorage.getItem(this.ROLE_KEY);

            if (storedToken) this.tokenSubject.next(storedToken);
            if(storedRole) this.roleSubject.next(storedRole);
      }

      login(username: string, password: string): Observable<ApiResponse<LoginResponse>> {
            return this.http.post<ApiResponse<LoginResponse>>(`${this.API_URL}/login`, { username, password });
      }
      loginSuccess(token: string, role: string) {
            localStorage.setItem(this.TOKEN_KEY, token);
            localStorage.setItem(this.ROLE_KEY, role);
            this.tokenSubject.next(token);
            this.roleSubject.next(role);
      }
      logout() {
            localStorage.removeItem(this.TOKEN_KEY);
            localStorage.removeItem(this.ROLE_KEY);
            this.tokenSubject.next(null);
            this.roleSubject.next(null);
      }

      // Ưu tiên lấy từ BehaviorSubject nếu đã có
      // ==> Fallback sang localStorage nếu chưa có (reload trang)
      getToken(): string | null { 
            const token = this.tokenSubject.value;
            if(token) return token;

            const stored = localStorage.getItem(this.TOKEN_KEY);
            if(stored) this.tokenSubject.next(stored);
            return stored;
      }
      getUserRole(): string | null {
            const role = this.roleSubject.value;
            if(role) return role;

            const stored = localStorage.getItem(this.ROLE_KEY);
            if(stored) this.roleSubject.next(stored);
            return stored;
      }
      isAdmin(): boolean {
            return this.roleSubject.value === 'admin';
      }

      isLoggedIn(): boolean {
            return !!this.getToken();
      }
}