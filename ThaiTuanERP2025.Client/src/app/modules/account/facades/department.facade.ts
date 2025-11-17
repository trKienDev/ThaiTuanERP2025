import { inject, Injectable } from "@angular/core";
import { DepartmentDto, DepartmentRequest, SetDepartmentManagerRequest } from "../models/department.model";
import { Observable, tap } from "rxjs";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { DepartmentApiService } from "../services/api/department-api.service";

@Injectable({ providedIn: 'root' })
export class DepartmentFacade extends BaseApiFacade<DepartmentDto, DepartmentRequest> {
      private readonly departmentApi = inject(DepartmentApiService);
      constructor() {
            super(inject(DepartmentApiService));
      }
      readonly departments$: Observable<DepartmentDto[]> = this.list$;

      setManager(id: string, req: SetDepartmentManagerRequest): Observable<string> {
            return this.departmentApi.setManager(id, req).pipe(
                  tap(() => this.refresh())
            )
      }

      setParent(id: string, parentId: string): Observable<void> {
            return this.departmentApi.setParent(id, parentId).pipe(
                  tap(() => this.refresh())
            )
      }
}