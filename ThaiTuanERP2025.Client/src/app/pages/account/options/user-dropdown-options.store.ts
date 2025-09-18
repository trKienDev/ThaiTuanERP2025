import { inject, Injectable } from "@angular/core";
import { UserFacade } from "../facades/user.facade";
import { environment } from "../../../../environments/environment";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { resolveAvatarUrl } from "../../../shared/utils/avatar.utils";
import { shareReplay } from "rxjs";

@Injectable({ providedIn: 'root' })
export class UserOptionStore {
      private readonly facade = inject(UserFacade);
      private readonly baseUrl = environment.baseUrl;

      readonly option$ = this.facade.users$.pipe(
            toDropdownOptions({ 
                  id: 'id',
                  label: 'fullName',
                  imgUrl: u => resolveAvatarUrl(this.baseUrl, u)
            }),
            shareReplay({ bufferSize: 1, refCount: false })
      );
}