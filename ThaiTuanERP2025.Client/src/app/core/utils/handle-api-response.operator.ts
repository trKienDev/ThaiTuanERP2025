import { map } from 'rxjs/operators';
import { ApiResponse } from '../models/api-response.model';
import { Observable, throwError } from 'rxjs';

export function handleApiResponse$<T>() {
      return (source$: Observable<ApiResponse<T>>): Observable<T> =>
            source$.pipe(
                  map((response) => {
                        console.log('response: ', response);
                        if (response.isSuccess && response.data !== null && response.data !== undefined) {
                              return response.data;
                        }

                  const errors = response.errors ?? [response.message ?? 'Đã xảy ra lỗi không xác định'];
                  throw new Error(errors[0]); // hoặc tạo class CustomApiError để chi tiết hơn
            })
      );
}
