// 📁 src/app/core/interceptors/correlation-id.interceptor.ts
import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { tap } from 'rxjs';


export const CorrelationIdInterceptor: HttpInterceptorFn = (req, next) => {
      let correlationId = req.headers.get('X-Correlation-ID') ?? crypto.randomUUID();

      const cloned = req.clone({ setHeaders: { 'X-Correlation-ID': correlationId } });

      return next(cloned).pipe(
            tap({
                  next: (event) => {
                        if (event instanceof HttpResponse) {
                              const returned = event.headers.get('X-Correlation-ID');
                              if (returned && returned !== correlationId) {
                                    console.warn(`⚠️ Correlation ID mismatch: sent=${correlationId}, got=${returned}`);
                              }
                        }
                  }
            })
      );
};