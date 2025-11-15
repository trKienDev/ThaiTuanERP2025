import { inject, Injectable } from "@angular/core";
import { UserFacade } from "../facades/user.facade";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { resolveAvatarUrl } from "../../../shared/utils/avatar.utils";
import { Observable, shareReplay } from "rxjs";
import { UserDto } from "../models/user.model";

@Injectable({ providedIn: 'root' })
export class ManagerOptionStore {
      private readonly facade = inject(UserFacade);

      /** Lấy dropdown option managers theo userId */
      getManagerOptions$(userId: string): Observable<{ id: string; label: string; imgUrl?: string }[]> {
            return this.facade.getManagers$(userId).pipe(
                  toDropdownOptions<UserDto>({
                        id: "id",
                        label: "fullName",
                        imgUrl: u => resolveAvatarUrl(u)
                  }),
                  shareReplay({ bufferSize: 1, refCount: false })
            );
      }
}
