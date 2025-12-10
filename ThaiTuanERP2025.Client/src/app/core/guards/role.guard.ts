import { inject } from '@angular/core';
import { CanActivateFn, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
      const auth = inject(AuthService);
      const router = inject(Router);
      const requiredRole = route.data['role'] as string;

      if (auth.hasRole(requiredRole)) return true;

      router.navigate(['/unauthorized']);
      return false;
};
