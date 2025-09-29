import { inject, Injectable } from "@angular/core";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { DepartmentDto, DepartmentRequest, SetDepartmentManagerRequest } from "../models/department.model";
import { DepartmentService } from "../services/department.service";
import { Observable, tap } from "rxjs";

@Injectable({ providedIn: 'root' })
export class DepartmentFacade extends BaseCrudFacade<DepartmentDto, DepartmentRequest> {
      private readonly departmentService = inject(DepartmentService);
      constructor() {
            super(inject(DepartmentService));
      }
      readonly department$: Observable<DepartmentDto[]> = this.list$;

      setManager(id: string, req: SetDepartmentManagerRequest): Observable<string> {
            return this.departmentService.setManager(id, req).pipe(
                  tap(() => this.refresh())
            )
      }
}