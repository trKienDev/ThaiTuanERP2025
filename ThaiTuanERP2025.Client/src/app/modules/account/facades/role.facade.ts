import { inject, Injectable } from "@angular/core";
import { RoleDto, RoleRequest } from "../models/role.model";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { RoleApiService } from "../services/api/role-api.service";

@Injectable({ providedIn: 'root' })
export class RoleFacade extends BaseApiFacade<RoleDto, RoleRequest> {
      constructor() {
            super(inject(RoleApiService));
      }     

      readonly roles$ = this.list$;
}