import { RoleDto, RoleRequest } from "../../models/role.model";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { Observable } from "rxjs";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root'})
export class RoleApiService extends BaseApiService<RoleDto, RoleRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.server.apiUrl}/role`);
      }

      assignPermissions(roleId: string, payload: string[]): Observable<void> {
            return this.http.post<ApiResponse<void>>(`${this.endpoint}/${roleId}/permissions`, payload)
                  .pipe(handleApiResponse$<void>());
      }
}