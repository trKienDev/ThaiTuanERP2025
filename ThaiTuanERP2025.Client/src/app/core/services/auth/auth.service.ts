import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../shared/models/api-response.model';
import { LoginResponseDto } from '../../../layouts/login/login-response.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
      private readonly TOKEN_KEY = 'token';
      private readonly USER_KEY = 'user';
      private readonly ROLES_KEY = 'roles';
      private readonly PERMS_KEY = 'permissions';

      private tokenSubject = new BehaviorSubject<string | null>(this.getToken());
      public token$ = this.tokenSubject.asObservable();

      constructor(private http: HttpClient) {}

      login(employeeCode: string, password: string) {
            return this.http.post<ApiResponse<LoginResponseDto>>(`${environment.apiUrl}/account/login`, {
                  employeeCode,
                  password
            });
      }

      loginSuccess(response: LoginResponseDto) {
            localStorage.setItem(this.TOKEN_KEY, response.accessToken);
            localStorage.setItem(this.USER_KEY, JSON.stringify({
                  id: response.userId,
                  username: response.username,
                  fullName: response.fullName
            }));
            localStorage.setItem(this.ROLES_KEY, JSON.stringify(response.roles));
            localStorage.setItem(this.PERMS_KEY, JSON.stringify(response.permissions));

            this.tokenSubject.next(response.accessToken);
      }

      logout() {
            localStorage.clear();
            this.tokenSubject.next(null);
      }

      getToken(): string | null {
            return localStorage.getItem(this.TOKEN_KEY);
      }

      getUser() {
            const user = localStorage.getItem(this.USER_KEY);
            return user ? JSON.parse(user) : null;
      }

      getUserRoles(): string[] {
            var roles = JSON.parse(localStorage.getItem(this.ROLES_KEY) || '[]');
            return roles;
      }

      getUserPermissions(): string[] {
            return JSON.parse(localStorage.getItem(this.PERMS_KEY) || '[]');
      }

      hasPermission(code: string): boolean {
            return this.getUserPermissions().includes(code);
      }

      hasRole(role: string): boolean {
            return this.getUserRoles().some(r => r.toLowerCase() === role.toLowerCase());
      }
}
