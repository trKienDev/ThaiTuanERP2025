import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../../environments/environment";
import { Observable } from "rxjs";
import { NotificationDto } from "../models/notification.model";
import { HttpClient, HttpParams } from "@angular/common/http";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class NotificationsApiService {
      private readonly http = inject(HttpClient);
      private readonly baseUrl = `${environment.baseUrl}/api/notification`;

      getList(opts?: { unreadOnly?: boolean; page?: number; pageSize?: number }): Observable<NotificationDto[]> {
            const params = new HttpParams()
                  .set('unreadOnly', String(opts?.unreadOnly ?? false)) 
                  .set('page', String(opts?.page ?? 1))
                  .set('pageSize', String(opts?.pageSize ?? 30));

            return this.http.get<{ isSuccess: boolean; data: NotificationDto[] }>(this.baseUrl, { params })
                  .pipe(handleApiResponse$<NotificationDto[]>());
      }

      getUnreadCount(): Observable<number> {
            return this.http.get<{ isSuccess: boolean; data: number }>(`${this.baseUrl}/unread-count`)
                  .pipe(handleApiResponse$<number>());
      }

      markRead(id: string): Observable<void> {
            return this.http.post<{ isSuccess: boolean }>(`${this.baseUrl}/${id}/read`, {})
                  .pipe(handleApiResponse$<void>());
      }

      markAllRead(): Observable<void> {
            return this.http.post<{ isSuccess: boolean }>(`${this.baseUrl}/read-all`, {})
                  .pipe(handleApiResponse$<void>());
      }
}