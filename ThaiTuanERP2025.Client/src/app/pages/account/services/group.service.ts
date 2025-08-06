import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { CreateGroupDto, GroupDto } from "../dtos/group.dto";
import { catchError, map, Observable, throwError } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";

@Injectable({ providedIn: 'root' })
export class GroupService {
      private readonly API_URL = `${environment.apiUrl}/group`;

      constructor(private http: HttpClient) {}

      createGroup(dto: CreateGroupDto): Observable<ApiResponse<GroupDto>> {
            return this.http.post<ApiResponse<GroupDto>>(this.API_URL, dto);
      }

      getAllGroups(): Observable<ApiResponse<GroupDto[]>> {
            return this.http.get<ApiResponse<GroupDto[]>>(`${this.API_URL}`);
      }

      deleteGroup(id: string, requestorId: string): Observable<ApiResponse<void>> {
            return this.http.delete<ApiResponse<void>>(`${this.API_URL}/${id}?requestorId=${requestorId}`);
      }

      addUserToGroup(groupId: string, userId: string): Observable<ApiResponse<void>> {
            return this.http.put<ApiResponse<void>>(`${this.API_URL}/${groupId}/add-user`, { userId });
      }

      removeUserFromGroup(groupId: string, data: { userId: string, requestorId: string}):Observable<ApiResponse<void>> {
            return this.http.put<ApiResponse<void>>(`${this.API_URL}/${groupId}/remove-user`, data);
      }

      updateGroup(groupId: string, data: { name: string, description: string, requestorId: string}): Observable<ApiResponse<void>> {
            return this.http.put<ApiResponse<void>>(`${this.API_URL}/${groupId}`, data);
      }

      changeAdmin(groupId: string, data: { newAdminId: string, requestorId: string }): Observable<ApiResponse<void>> {
            return this.http.put<ApiResponse<void>>(`${this.API_URL}/${groupId}/change-admin`, data);
      }
}