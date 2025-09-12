import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/api-response.model';

export function handleApiResponse$<T>() {
      return (source$: Observable<ApiResponse<T>>): Observable<T> =>
            source$.pipe(
                  map((response) => {
                        if (response.isSuccess && response.data !== null && response.data !== undefined) {
                              return response.data;
                        }

                  const errors = response.errors ?? [response.message ?? 'Đã xảy ra lỗi không xác định'];
                  throw new Error(errors[0]); // hoặc tạo class CustomApiError để chi tiết hơn
            })
      );
}     
