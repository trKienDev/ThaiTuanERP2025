import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { RoleDto, RoleRequest } from "../models/role.model";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root'})
export class RoleService extends BaseCrudService<RoleDto, RoleRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/roles`);
      }
}