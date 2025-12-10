import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { inject} from "@angular/core";
import { Router } from "@angular/router";
import { catchError, throwError } from "rxjs";
import { ToastService } from "../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { environment } from "../../../environments/environment";


export const HttpErrorInterceptor: HttpInterceptorFn = (req, next) => {
      const toast = inject(ToastService);
      const router = inject(Router);

      return next(req).pipe(
            catchError((error: HttpErrorResponse) => {
                  // === Lỗi kỹ thuật, toàn hệ thống ===
                  switch (error.status) {
                        case 0:
                              toast.errorRich('Không thể kết nối đến máy chủ.');
                              break;
                        case 401:
                              toast.warningRich('Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại.');
                              router.navigate(['/login']);
                              break;
                        case 403:
                              toast.errorRich('Bạn không có quyền thực hiện hành động này.');
                              break;
                        case 500:
                              toast.errorRich('Lỗi hệ thống, vui lòng thử lại sau.');
                              break;
                  }

                  // In ra console trong môi trường dev
                  if (!environment.production) {
                        console.error('HTTP Error:', error);
                  }

                  // Ném lại lỗi để component có thể xử lý nghiệp vụ cụ thể
                  return throwError(() => error);
            })
      );
};