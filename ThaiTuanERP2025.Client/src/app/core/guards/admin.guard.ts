// src/app/core/guards/admin.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';

export const adminGuard: CanActivateFn = (route, state) => {
      const authService = inject(AuthService);
      const router = inject(Router);

      const userRole = authService.getUserRole(); // Ví dụ: lấy user từ localStorage hoặc BehaviorSubject
      if (userRole === 'admin') {
            return true;
      } else {
            // Có thể điều hướng về trang không có quyền hoặc trang chủ
            router.navigate(['/home']);
            return false;
      }
};