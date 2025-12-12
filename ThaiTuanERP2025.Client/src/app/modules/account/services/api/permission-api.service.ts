import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { PermissionDto, PermissionRequest } from "../../models/permission.model";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root'})
export class PermissionApiService extends BaseApiService<PermissionDto, PermissionRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.server.apiUrl}/permission`);
      }

      getByRoleId(roleId: string): Observable<PermissionDto[]> {
            return this.http.get<ApiResponse<PermissionDto[]>>(`${this.endpoint}/role/${roleId}`)
                  .pipe(handleApiResponse$<PermissionDto[]>());
      }
}