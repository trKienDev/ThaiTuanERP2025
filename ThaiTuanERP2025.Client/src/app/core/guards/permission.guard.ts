import { inject } from '@angular/core';
import { CanActivateFn, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

export const permissionGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
      const auth = inject(AuthService);
      const router = inject(Router);
      const requiredPerm = route.data['permission'] as string;

      if (auth.hasPermission(requiredPerm)) return true;

      router.navigate(['/unauthorized']);
      return false;
};
