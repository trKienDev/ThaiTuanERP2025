import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
      const authService = inject(AuthService);
      const token = authService.getToken();

      if (token) {
            const authReq = req.clone({
                  setHeaders: {
                        Authorization: `Bearer ${token}`
                  }
            });
            return next(authReq);
      }

      return next(req);
};
