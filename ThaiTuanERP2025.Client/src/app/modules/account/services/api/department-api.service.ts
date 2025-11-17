import { Injectable } from "@angular/core";
import { DepartmentDto, DepartmentRequest, SetDepartmentManagerRequest} from "../../models/department.model";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../../shared/operators/handle-api-response.operator";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' }) 
export class DepartmentApiService extends BaseApiService<DepartmentDto, DepartmentRequest> {
      constructor(http: HttpClient) {
            super(http,`${environment.apiUrl}/department`);
      }

      setManager(id: string, request: SetDepartmentManagerRequest): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.endpoint}/${id}/managers`, request)
                  .pipe(handleApiResponse$<string>());
      }

      setParent(id: string, parentId: string): Observable<void> {
            return this.http.put<ApiResponse<void>>(`${this.endpoint}/${id}/parent/${parentId}`, {})
                  .pipe(handleApiResponse$<void>());
      }

      getParentDepartment(id: string): Observable<DepartmentDto | null> {
            return this.http.get<ApiResponse<DepartmentDto | null>>(`${this.endpoint}/${id}/parent`)
                  .pipe(handleApiResponse$<DepartmentDto | null>());
      }
}