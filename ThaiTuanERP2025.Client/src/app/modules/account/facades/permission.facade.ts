import { Injectable, inject } from "@angular/core";
import { PermissionDto, PermissionRequest } from "../models/permission.model";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { PermissionApiService } from "../services/api/permission-api.service";

@Injectable({ providedIn: 'root' })
export class PermissionFacade extends BaseApiFacade<PermissionDto, PermissionRequest> {
      constructor() {
            super(inject(PermissionApiService));
      }     

      readonly permissions$ = this.list$;
}