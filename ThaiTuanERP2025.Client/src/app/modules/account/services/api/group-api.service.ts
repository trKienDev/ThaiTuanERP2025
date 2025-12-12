import { Injectable } from "@angular/core";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable} from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { CreateGroupModel, GroupModel } from "../../models/group.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class GroupApiService {
      private readonly API_URL = `${environment.server.apiUrl}/group`;

      constructor(private readonly http: HttpClient) {}

      create(dto: CreateGroupModel): Observable<GroupModel> {
            return this.http.post<ApiResponse<GroupModel>>(this.API_URL, dto)
                  .pipe(handleApiResponse$<GroupModel>());
      }

      getAll(): Observable<GroupModel[]> {
            return this.http.get<ApiResponse<GroupModel[]>>(`${this.API_URL}`)
                  .pipe(handleApiResponse$<GroupModel[]>());
      }

      deleteGroup(id: string, requestorId: string): Observable<string> {
            return this.http.delete<ApiResponse<string>>(`${this.API_URL}/${id}?requestorId=${requestorId}`)
                  .pipe(handleApiResponse$<string>());
      }

      addUserToGroup(groupId: string, userId: string): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.API_URL}/${groupId}/add-user`, { userId })
                  .pipe(handleApiResponse$<string>());
      }

      removeUserFromGroup(groupId: string, data: { userId: string, requestorId: string}):Observable<ApiResponse<void>> {
            return this.http.put<ApiResponse<void>>(`${this.API_URL}/${groupId}/remove-user`, data);
      }

      updateGroup(groupId: string, data: { name: string, description: string, requestorId: string}): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.API_URL}/${groupId}`, data)
                  .pipe(handleApiResponse$<string>());
      }

      changeAdmin(groupId: string, data: { newAdminId: string, requestorId: string }): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.API_URL}/${groupId}/change-admin`, data)
                  .pipe(handleApiResponse$<string>());
      }
}