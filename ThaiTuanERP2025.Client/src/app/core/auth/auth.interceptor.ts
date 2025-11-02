import { inject, Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable, catchError, switchMap, throwError } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
      constructor(private authService: AuthService) {}

      intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
            const token = this.authService.getToken();
            let authReq = req;

            if (token) {
                  authReq = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
            }

            return next.handle(authReq).pipe(
                  catchError((error: HttpErrorResponse) => {
                        if (error.status === 401 && !req.url.includes('/auth/login')) {
                              // Thử refresh token
                              return this.authService.refreshToken().pipe(
                                    switchMap(newToken => {
                                          if (!newToken) throw error;
                                          const retryReq = req.clone({
                                                setHeaders: { Authorization: `Bearer ${this.authService.getToken()}` }
                                          });
                                          return next.handle(retryReq);
                                    }),
                                    catchError(() => {
                                          this.authService.logout();
                                          return throwError(() => error);
                                    })
                              );
                        }
                        return throwError(() => error);
                  })
            );
      }
}