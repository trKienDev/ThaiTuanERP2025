import { HttpErrorResponse } from "@angular/common/http";
export function handleHttpError(
      error: any,
      fallbackMessage = 'Không thể kết nối tới máy chủ'
): string[] {
      if(error instanceof HttpErrorResponse) {
            if(error.status === 0) {
                  return ['Không thể kết nối đến máy chủ. Vui lòng kiểm tra mạng'];
            }
            
            if(error.status >= 500) {
                  return ['Lỗi máy chủ. Vui lòng thử lại sau'];
            }

            // Ưu tiên mảng errors nếu có phần tử
            const errsCamel = Array.isArray(error.error?.errors) ? error.error.errors as string[] : [];
            const errsPascal = Array.isArray(error.error?.Errors) ? error.error.Errors as string[] : [];
            const errs = (errsCamel.length ? errsCamel : errsPascal);
            if (errs.length > 0) return errs;

            // Nếu không có errors, lấy message
            const msg = (typeof error.error?.message === 'string' && error.error.message) ||
                  (typeof error.error?.Message === 'string' && error.error.Message);
            if (msg) return [msg];

            // Fallback cuối
            return [error.message || fallbackMessage];
      }

      // Không phải HttpErrorResponse (ví dụ Error do handleApiResponse$ ném)
      if (error?.message && typeof error.message === 'string') {
            return [error.message];
      }

      return [fallbackMessage];
}