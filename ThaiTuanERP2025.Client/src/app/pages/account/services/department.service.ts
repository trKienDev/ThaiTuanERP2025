import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { CreateDepartmentRequest, DepartmentDto, SetDepartmentManagerRequest, UpdateDepartmentRequest } from "../models/department.model";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' }) 
export class DepartmentService extends BaseCrudService<DepartmentDto, CreateDepartmentRequest, UpdateDepartmentRequest> {
      constructor(http: HttpClient) {
            super(http,`${environment.apiUrl}/department`);
      }

      setManager(id: string, request: SetDepartmentManagerRequest): Observable<string> {
            return this.http.put<ApiResponse<string>>(`${this.endpoint}/${id}/manager`, request)
                  .pipe(handleApiResponse$<string>());
      }
}