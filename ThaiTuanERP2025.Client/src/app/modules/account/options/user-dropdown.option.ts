import { inject, Injectable } from "@angular/core";
import { UserFacade } from "../facades/user.facade";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { resolveAvatarUrl } from "../../../shared/utils/avatar.utils";
import { shareReplay } from "rxjs";
import { KitDropdownOption } from "../../../shared/components/kit-dropdown/kit-dropdown.component";

@Injectable({ providedIn: 'root' })
export class UserOptionStore {
      private readonly facade = inject(UserFacade);
      private _snapshot: KitDropdownOption[] = [];

      readonly option$ = this.facade.users$.pipe(
            toDropdownOptions({ 
                  id: 'id',
                  label: 'fullName',
                  imgUrl: u => resolveAvatarUrl(u)
            }),
            shareReplay({ bufferSize: 1, refCount: false })
      );

      constructor() {
            // keep snapshot updated
            this.option$.subscribe(options => {
                  this._snapshot = options;
            });
      }

      // public getter
      get snapshot(): KitDropdownOption[] {
            return this._snapshot;
      }
}