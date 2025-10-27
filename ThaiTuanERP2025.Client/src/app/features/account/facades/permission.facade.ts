import { Injectable, inject } from "@angular/core";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { PermissionDto, PermissionRequest } from "../models/permission.model";
import { PermissionService } from "../services/permission.service";

@Injectable({ providedIn: 'root' })
export class PermissionFacade extends BaseCrudFacade<PermissionDto, PermissionRequest> {
      constructor() {
            super(inject(PermissionService));
      }     

      readonly permissions$ = this.list$;
}