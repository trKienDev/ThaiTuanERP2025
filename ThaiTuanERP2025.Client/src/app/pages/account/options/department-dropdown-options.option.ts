import { inject, Injectable } from "@angular/core";
import { DepartmentFacade } from "../facades/department.facade";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { shareReplay } from "rxjs";

@Injectable({ providedIn: 'root' })
export class DepartmentOptionStore {
      private readonly facade = inject(DepartmentFacade);

      readonly option$ = this.facade.departments$.pipe(
            toDropdownOptions({
                  id: 'id',
                  label: 'name',
            }),
            shareReplay({ bufferSize: 1, refCount: false })
      )
}