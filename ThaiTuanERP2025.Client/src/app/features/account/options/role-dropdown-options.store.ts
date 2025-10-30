import { inject, Injectable } from "@angular/core";
import { RoleFacade } from "../facades/role.facade";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { shareReplay } from "rxjs";

@Injectable({ providedIn: 'root' })
export class RoleDropdownOptionsStore {
      private readonly facade = inject(RoleFacade);

      readonly option$ = this.facade.roles$.pipe(
            toDropdownOptions({
                  id: 'id',
                  label: 'name',
            }),
            shareReplay({ bufferSize: 1, refCount: false })
      )
}