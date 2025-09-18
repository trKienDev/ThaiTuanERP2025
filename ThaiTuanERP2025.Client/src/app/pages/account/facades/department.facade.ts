import { inject, Injectable } from "@angular/core";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { CreateDepartmentRequest, DepartmentDto, UpdateDepartmentRequest } from "../models/department.model";
import { DepartmentService } from "../services/department.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class DepartmentFacade extends BaseCrudFacade<DepartmentDto, CreateDepartmentRequest, UpdateDepartmentRequest> {
      constructor() {
            super(inject(DepartmentService));
      }
      readonly department$: Observable<DepartmentDto[]> = this.list$;
}