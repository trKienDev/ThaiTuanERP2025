import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { AssignPermissionToRoleRequest, PermissionDto, PermissionRequest } from "../models/permission.model";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";
import { ApiResponse } from "../../../shared/models/api-response.model";

@Injectable({ providedIn: 'root'})
export class PermissionService extends BaseCrudService<PermissionDto, PermissionRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/permission`);
      }

      getByRoleId(roleId: string): Observable<PermissionDto[]> {
            return this.http.get<ApiResponse<PermissionDto[]>>(`${this.endpoint}/by-role-id/${roleId}`)
                  .pipe(handleApiResponse$<PermissionDto[]>());
      }

      assignPermissionsToRole(roleId: string, payload: string[]): Observable<void> {
            return this.http.post<ApiResponse<void>>(`${this.endpoint}/${roleId}/assign-permissions`, payload)
                  .pipe(handleApiResponse$<void>());
      }
}