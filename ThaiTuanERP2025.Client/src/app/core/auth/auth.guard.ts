import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { AuthService } from './auth.service';
import { Observable, of, map, switchMap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
      constructor(private authService: AuthService, private router: Router) {}

      canActivate(): Observable<boolean | UrlTree> {
            const token = this.authService.getToken();

            if (!token) {
                  return of(this.router.createUrlTree(['/login']));
            }

            if (this.authService.isTokenExpired()) {
                  return this.authService.refreshToken().pipe(
                        switchMap(res => {
                              if (res) return of(true);
                              this.authService.logout();
                              return of(this.router.createUrlTree(['/login']));
                        })
                  );
            }

            // Nếu user chưa được load (ví dụ vừa reload)
            if (!this.authService.getUser()) {
                  return this.authService.getCurrentUserFromServer().pipe(
                        map(user => {
                              if (user) return true;
                              this.authService.logout();
                              return this.router.createUrlTree(['/login']);
                        })
                  );
            }

            return of(true);
      }
}