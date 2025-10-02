import { HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { catchError, throwError } from "rxjs";
import { AuthService } from "../services/auth/auth.service";

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
      const authService = inject(AuthService);
      const router = inject(Router);

      return next(req).pipe(
            catchError(error => {
                  if (error.status === 401 || error.status === 403) {
                        authService.logout();
                        router.navigate(['/login']);
                  }
                  return throwError(() => error);
            })
      );
};