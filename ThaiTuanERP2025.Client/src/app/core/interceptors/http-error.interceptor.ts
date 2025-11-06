import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { inject} from "@angular/core";
import { Router } from "@angular/router";
import { catchError, throwError } from "rxjs";
import { ToastService } from "../../shared/components/kit-toast-alert/kit-toast-alert.service";

export const HttpErrorInterceptor: HttpInterceptorFn = (req, next) => {
      const toast = inject(ToastService);
      const router = inject(Router);

      return next(req).pipe(
            catchError((error: HttpErrorResponse) => {
                  // === Kiểm tra mã lỗi HTTP ===
                  if (error.status === 403) {
                        console.error('http-error.interceptor.ts - Access Denied 403'); 
                        toast.errorRich('Bạn không có quyền thực hiện hành động này.');
                  }
                  else if (error.status === 401) {
                        toast.warningRich('Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại.');
                        router.navigate(['/login']);
                  }
                  else if (error.status === 500) {
                        toast.errorRich('Lỗi hệ thống, vui lòng thử lại sau.');
                  }
                  else if (error.status === 0) {
                        toast.errorRich('Không thể kết nối đến máy chủ.');
                  }

                  // Log lỗi ra console (phục vụ dev)
                  console.error('HTTP Error:', error);

                  // Ném lại lỗi để observable chain phía sau vẫn biết
                  return throwError(() => error);
            })
      );
};