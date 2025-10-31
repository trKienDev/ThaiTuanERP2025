import { HttpInterceptorFn, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { tap } from 'rxjs/operators';

/**
 * 🎨 Hiển thị log màu theo HTTP method
 */
function getColorByMethod(method: string): string {
      switch (method.toUpperCase()) {
            case 'GET': return 'color: #00bfff';    // xanh dương
            case 'POST': return 'color: #ffb700';   // vàng
            case 'PUT': return 'color: #009688';    // xanh ngọc
            case 'DELETE': return 'color: #ff5252'; // đỏ
            default: return 'color: gray';
      }
}

/**
 * 🧩 Interceptor hiển thị log HTTP request/response ra console
 */
export const httpLoggerInterceptor: HttpInterceptorFn = (req, next) => {
      const correlationId = req.headers.get('X-Correlation-ID') ?? '(none)';
      const started = performance.now(); // <-- vẫn hoạt động trong browser hiện đại

      console.log(
            `%c➡️ ${req.method}`,
            getColorByMethod(req.method),
            `${req.url}`,
            `Corr=${correlationId}`
      );

      return next(req).pipe(
            tap({
                  next: event => {
                        if (event instanceof HttpResponse) {
                              const elapsed = performance.now() - started;
                              const status = event.status;
                              const color = status >= 200 && status < 300 ? 'color: #4CAF50' : 'color: orange';

                              console.log(
                                    `%c⬅️ ${req.method} ${status}`,
                                    color,
                                    `${req.url} (${elapsed.toFixed(0)} ms)`,
                                    `Corr=${event.headers.get('X-Correlation-ID') ?? correlationId}`
                              );
                        }
                  },
                  error: (error: HttpErrorResponse) => {
                        const elapsed = performance.now() - started;
                        console.log(
                        `%c💥 ${req.method} ${error.status}`,
                        'color: red',
                        `${req.url} (${elapsed.toFixed(0)} ms)`,
                        `Corr=${error.headers?.get('X-Correlation-ID') ?? correlationId}`,
                        '\n', error.message
                        );
                  }
            })
      );
};
