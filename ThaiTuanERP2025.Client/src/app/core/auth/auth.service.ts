import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, of, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { LoginResponseDto } from '../../layouts/login/login-response.model';
import { Router } from '@angular/router';
import { UserDto } from '../../modules/account/models/user.model';
import { ApiResponse } from '../../shared/models/api-response.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
      // ===== LocalStorage keys =====
      private readonly TOKEN_KEY = 'token';
      private readonly REFRESH_KEY = 'refreshToken';
      private readonly USER_KEY = 'user';
      private readonly ROLES_KEY = 'roles';
      private readonly PERMISSONS_KEY = 'permissions';     
      private readonly apiUrl = `${environment.server.apiUrl}/auth`;
      private isRefreshing = false;

      // ===== Reactive State =====
      private readonly currentUserSubject = new BehaviorSubject<Partial<UserDto> | null>(this.loadUserFromStorage());
      currentUser$ = this.currentUserSubject.asObservable();

      constructor(private readonly http: HttpClient, private readonly router: Router) {}

      // === Login ===
      login(employeeCode: string, password: string) {
            console.log('environment: ', environment);
            return this.http
                  .post<ApiResponse<LoginResponseDto>>(`${this.apiUrl}/login`, { employeeCode, password })
                  .pipe(
                        tap((response) => {
                              if (response.isSuccess && response.data) {
                                    this.loginSuccess(response.data);
                              } else {
                                    console.error('[AuthService] Login API failed', response.message);      
                              }
                        })
                  );
      }

      private loginSuccess(response: LoginResponseDto) {
            const user: Partial<UserDto> = {
                  id: response.userId,
                  username: response.username,
                  fullName: response.fullName,
                  roles: response.roles,
                  permissions: response.permissions
            };

            this.storeTokens(response.accessToken, response.refreshToken);

            localStorage.setItem(this.USER_KEY, JSON.stringify(user));
            localStorage.setItem(this.ROLES_KEY, JSON.stringify(response.roles));
            localStorage.setItem(this.PERMISSONS_KEY, JSON.stringify(response.permissions));

            // Cập nhật trạng thái reactive
            this.currentUserSubject.next(user);
      }

      // === Store & Get Tokens
      private storeTokens(access: string, refresh: string) {
            localStorage.setItem(this.TOKEN_KEY, access);
            localStorage.setItem(this.REFRESH_KEY, refresh);
      }

      getToken(): string | null {
            return localStorage.getItem(this.TOKEN_KEY);
      }
      getRefreshToken(): string | null {
            return localStorage.getItem(this.REFRESH_KEY);
      }

      // === Refresh token ====
      refreshToken() {
            const refreshToken = this.getRefreshToken();
            if (!refreshToken) return of(null);

            if (this.isRefreshing) return of(null); // tránh gọi trùng

            this.isRefreshing = true;
            console.log('[AuthService] Refreshing access token...');

            return this.http.post<LoginResponseDto>(`${this.apiUrl}/refresh-token`, { refreshToken })
                  .pipe(tap(
                        res => {
                              this.storeTokens(res.accessToken, res.refreshToken);
                              this.isRefreshing = false;
                        }),
                        catchError(err => {
                              console.warn('[AuthService] Refresh token failed', err);
                              this.isRefreshing = false;
                              this.logout();
                              return of(null);
                        })
                  );
      }

      // === Logout === 
      logout() {
            localStorage.clear();
            this.currentUserSubject.next(null);
            this.router.navigate(['/login']);
      }

      // === Check token expired ===
      isTokenExpired(): boolean {
            const token = this.getToken();

            if (!token) {
                  return true;
            }

            try {
                  const base64Payload = token.split('.')[1];
                  const payloadString = this.decodeBase64Url(base64Payload);
                  const payload = JSON.parse(payloadString);

                  const exp = payload.exp;
                  const now = Math.floor(Date.now() / 1000);
                  return exp < now;
            } catch(error) {
                  console.error(error);
                  return true;
            }
      }

      // === Load user ====
      private loadUserFromStorage(): UserDto | null {
            const data = localStorage.getItem(this.USER_KEY);
            if (!data) return null;
            try {
                  return JSON.parse(data);
            } catch {
                  return null;
            }
      }
      getCurrentUserFromServer() {
            return this.http.get<UserDto>(`${environment.server.apiUrl}user/me`).pipe(
                  tap((user) => {
                        // Đồng bộ lại localStorage và BehaviorSubject
                        localStorage.setItem(this.USER_KEY, JSON.stringify(user));
                        localStorage.setItem(this.ROLES_KEY, JSON.stringify(user.roles || []));
                        localStorage.setItem(this.PERMISSONS_KEY, JSON.stringify(user.permissions || []));
                        this.currentUserSubject.next(user);
                  }),
                  catchError((err) => {
                        console.error('[AuthService] getCurrentUserFromServer failed', err);
                        alert('[auth service] GetCurrentUser: Your session has expired. Please log in again.');
                        this.logout();
                        return of(null);
                  })
            );
      }

      getUser() {
            const user = localStorage.getItem(this.USER_KEY);
            return user ? JSON.parse(user) : null;
      }

      getUserRoles(): string[] {
            return JSON.parse(localStorage.getItem(this.ROLES_KEY) || '[]');
      }

      getUserPermissions(): string[] {
            return JSON.parse(localStorage.getItem(this.PERMISSONS_KEY) || '[]');
      }

      hasPermission(code: string): boolean {
            return this.getUserPermissions().includes(code);
      }

      hasRole(role: string): boolean {
            return this.getUserRoles().some(r => r.toLowerCase() === role.toLowerCase());
      }


      // Hàm kiểm tra và tự động logout nếu token hết hạn
      checkTokenValidity() {
            if (this.isTokenExpired()) {
                  console.warn('[AuthService] Token expired → auto logout');
                  this.logout();
            }
      }  

      initAfterReload() {
            const token = this.getToken();
            if (token && !this.currentUserSubject.value) {
                  this.getCurrentUserFromServer().subscribe();
            }
      }

      decodeBase64Url(input: string): string {
            let output = input.replaceAll('-', '+').replaceAll('_', '/');

            const pad = output.length % 4;
            if (pad === 2) output += '==';
            else if (pad === 3) output += '=';
            else if (pad !== 0) throw new Error('Invalid base64url string');

            return atob(output);
      }
}
