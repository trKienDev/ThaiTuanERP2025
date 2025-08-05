import { ApiResponse } from "../models/api-response.model";

export function handleApiResponse<T> (
      response: ApiResponse<T>,
      onSuccess: (data: T) => void,
      onError?: (errors: string[]) => void
):void {
      if(response.isSuccess && response.data !== null && response.data !== undefined) {
            onSuccess(response.data);
      } else {
            const errors = response.errors ?? [response.message ?? 'Đã xảy ra lỗi không xác định'];
            if(onError) {
                  onError(errors);
            } else {
                  alert('API error');
                  console.log('API error: ', errors);
            }
      }
}