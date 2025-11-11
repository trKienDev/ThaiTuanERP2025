import { Injectable } from "@angular/core";
import { ToastService } from "../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { ConfirmService } from "../../shared/components/confirm-dialog/confirm.service";
import { HttpErrorResponse } from "@angular/common/http";
import { handleHttpError } from "../../shared/utils/handle-http-errors.util";

@Injectable({ providedIn: 'root' })
export class HttpErrorHandlerService {
      constructor(
            private readonly toast: ToastService,
            private readonly confirm: ConfirmService
      ) {}

      /**
       * Xử lý lỗi nghiệp vụ cho từng hành động cụ thể.
       * @param error Lỗi HTTP
       * @param contextMessage Thông điệp nghiệp vụ (ví dụ: 'tạo mã ngân sách', 'cập nhật kỳ ngân sách')
       */
      handle(error: unknown, contextMessage: string): void {
            // Tránh trùng với các lỗi kỹ thuật đã xử lý ở interceptor
            if (!(error instanceof HttpErrorResponse)) {
                  console.error('Unknown error:', error);
                  this.toast.errorRich('Lỗi không xác định.');
                  return;
            }

            // Xử lý lỗi nghiệp vụ
            switch (error.status) {
                  case 400: {
                        this.toast.errorRich(error.error?.message || 'Dữ liệu không hợp lệ.');
                        const messages = handleHttpError(error).join('\n');
                        this.confirm.error$(messages);
                        break;
                  }
                  case 404: {
                        this.toast.errorRich(`${contextMessage}.`);
                        const messages = handleHttpError(error).join('\n');
                        this.confirm.error$(messages);
                        break;
                  }
                  default: {
                        const messages = handleHttpError(error).join('\n');
                        this.confirm.error$(messages);
                        this.toast.errorRich(`${this.capitalize(contextMessage)} thất bại.`);
                        break;
                  }
            }
      }

      private capitalize(text: string): string {
            return text.charAt(0).toUpperCase() + text.slice(1);
      }
}