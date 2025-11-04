import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { RoleDto, RoleRequest } from "../models/role.model";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root'})
export class RoleService extends BaseCrudService<RoleDto, RoleRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/role`);
      }

      assignPermissions(roleId: string, payload: string[]): Observable<void> {
            return this.http.post<ApiResponse<void>>(`${this.endpoint}/${roleId}/permissions`, payload)
                  .pipe(handleApiResponse$<void>());
      }
}