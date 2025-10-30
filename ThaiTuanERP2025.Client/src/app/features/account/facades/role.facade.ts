import { inject, Injectable } from "@angular/core";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { RoleDto, RoleRequest } from "../models/role.model";
import { RoleService } from "../services/role.service";

@Injectable({ providedIn: 'root' })
export class RoleFacade extends BaseCrudFacade<RoleDto, RoleRequest> {
      constructor() {
            super(inject(RoleService));
      }     

      readonly roles$ = this.list$;
}