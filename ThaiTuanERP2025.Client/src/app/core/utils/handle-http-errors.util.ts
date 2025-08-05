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

            if(error.error?.errors && Array.isArray(error.error.errors)) {
                  return error.error.errors;
            } 

            if(typeof error.error?.message === 'string') {
                  return [error.error.message];
            }

            return [error.message || fallbackMessage];
      }

      return [fallbackMessage];
}