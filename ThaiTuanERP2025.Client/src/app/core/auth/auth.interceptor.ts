import { inject, Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable, catchError, switchMap, throwError } from 'rxjs';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
      const authService = inject(AuthService);

      const token = authService.getToken();
      let authReq: HttpRequest<any> = req;

      // Nếu có token => thêm header Authorization
      if (token) {
            authReq = req.clone({
                  setHeaders: { Authorization: `Bearer ${token}` }
            });
      }

      return next(authReq).pipe(
            catchError((error: HttpErrorResponse) => {
                  // Nếu bị 401 và không phải login endpoint => thử refresh token
                  if (error.status === 401 && !req.url.includes('/auth/login')) {
                        return authService.refreshToken().pipe(
                              switchMap(newToken => {
                                    if (!newToken) throw error;

                                    // Sau khi refresh thành công → gắn token mới vào request
                                    const retryReq = req.clone({
                                          setHeaders: { Authorization: `Bearer ${authService.getToken()}` }
                                    });
                                    return next(retryReq);
                              }),
                              catchError(() => {
                                    alert('[auth.interceptor]: Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.');
                                    authService.logout();
                                    return throwError(() => error);
                              })
                        );
                  }

                  return throwError(() => error);
            })
      );
};