// src/app/pages/expense/guards/payment-detail-entry.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';

export const paymentDetailEntryGuard: CanActivateFn = () => {
      const router = inject(Router);

      // 1) Cho phép nếu điều hướng đến từ Drawer với state flag
      const nav = router.getCurrentNavigation();
      const fromReminder = nav?.extras?.state?.['fromReminder'] === true;

      // 2) Cho phép 1 lần nếu đã cấp vé trong sessionStorage
      const allowOnce = sessionStorage.getItem('allowPaymentDetailOnce') === '1';

      if (fromReminder || allowOnce) {
            // tiêu thụ vé 1 lần
            if (allowOnce) sessionStorage.removeItem('allowPaymentDetailOnce');
                  return true;
      }

      // 3) Không hợp lệ → chuyển hướng về trang mặc định (tuỳ bạn)
      return router.parseUrl('/expense/payment-request') as UrlTree;
};
