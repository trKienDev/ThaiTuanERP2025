import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';

export const adminGuard: CanActivateFn = () => {
      const auth = inject(AuthService);
      const router = inject(Router);

      

      if (auth.hasRole('SuperAdmin')) return true;

      router.navigate(['/unauthorized']);
      return false;
};
