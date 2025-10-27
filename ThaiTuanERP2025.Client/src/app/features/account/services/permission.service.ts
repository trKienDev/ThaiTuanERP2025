import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { PermissionDto, PermissionRequest } from "../models/permission.model";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root'})
export class PermissionService extends BaseCrudService<PermissionDto, PermissionRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/permission`);
      }
}