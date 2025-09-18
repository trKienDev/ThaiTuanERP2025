import { inject, Injectable } from "@angular/core";
import { UserService } from "../../pages/account/services/user.service";
import { environment } from "../../../environments/environment";
import { BehaviorSubject, catchError, of, shareReplay, switchMap } from "rxjs";
import { toDropdownOptions } from "../components/kit-dropdown/kit-dropdown-to-options.operator";
import { resolveAvatarUrl } from "../utils/avatar.utils";
import { KitDropdownOption } from "../components/kit-dropdown/kit-dropdown.component";

@Injectable({ providedIn: 'root' })
export class UserOptionsStore {
  private readonly userService = inject(UserService);
  private readonly baseUrl = environment.baseUrl;
  private readonly refresh$ = new BehaviorSubject<void>(undefined);

  readonly options$ = this.refresh$.pipe(
    switchMap(() =>
      this.userService.getAll().pipe(
        toDropdownOptions<any>({
          id: 'id',
          label: 'fullName',
          imgUrl: u => resolveAvatarUrl(this.baseUrl, u),
        }),
        catchError(() => of([] as KitDropdownOption[]))
      )
    ),
    shareReplay({ bufferSize: 1, refCount: false })
  );

  refresh(): void { this.refresh$.next(); }
}