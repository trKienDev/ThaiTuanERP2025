import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { TaskReminderDto } from '../models/task-reminder.model';
import { handleApiResponse$ } from '../../../../shared/operators/handle-api-response.operator';
import { ApiResponse } from '../../../../shared/models/api-response.model';

@Injectable({ providedIn: 'root' })
export class TaskReminderApiService {
      private readonly http = inject(HttpClient);
      private readonly baseUrl = `${environment.baseUrl}/api/reminder`;

      getMyActive(): Observable<TaskReminderDto[]> {
            return this.http.get<ApiResponse<TaskReminderDto[]>>(this.baseUrl)
                  .pipe(handleApiResponse$<TaskReminderDto[]>());
      }

      dismiss(id: string): Observable<void> {
            return this.http
                  .post<ApiResponse<void>>(`${this.baseUrl}/${id}/dismiss`, {})
                  .pipe(handleApiResponse$<void>());
      }
}
