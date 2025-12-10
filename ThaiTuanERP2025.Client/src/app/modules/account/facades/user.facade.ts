import { inject, Injectable } from "@angular/core";
import { UserBriefAvatarDto, UserDto,  UserRequest } from "../models/user.model";
import { Observable, shareReplay, startWith, Subject, switchMap, tap } from "rxjs";
import { UserApiService } from "../services/api/user-api.service";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";

@Injectable({ providedIn: 'root' })
export class UserFacade extends BaseApiFacade<UserDto, UserRequest>{
      private readonly userService = inject(UserApiService);
      readonly users$: Observable<UserDto[]> = this.list$;

      constructor() {
            super(inject(UserApiService));
      }
      
      // ========= CURRENT USER =========
      private readonly currentUserReload$ = new Subject<void>();
      readonly currentUser$: Observable<UserDto> = this.currentUserReload$.pipe(
            // phát 1 giá trị ngay từ đầu để load lần đầu
            startWith(void 0),
            switchMap(() => this.userService.getCurrentuser()),
            shareReplay({ bufferSize: 1, refCount: true })
      );
      refreshCurrentUser(): void {
            this.currentUserReload$.next();
      }

      // ========= MANAGER IDS CACHE =========
      private readonly managerIdsCache = new Map<string, Observable<string[]>>();
      getManagerIds$(userId: string): Observable<string[]> {
            let req$ = this.managerIdsCache.get(userId);

            if (!req$) {
                  req$ = this.userService.getManagerIds(userId).pipe(
                        tap(() => {
                              // console.log(`Fetched manager IDs for user ${userId}`);
                        }),
                        // share kết quả + phát ngay giá trị mới nhất cho subscriber đến sau
                        shareReplay({ bufferSize: 1, refCount: true })
                  );
                  this.managerIdsCache.set(userId, req$);
            }

            return req$; 
      };
      invalidateManagerIds(userId: string): void {
            this.managerIdsCache.delete(userId);
      }
      getMyManagers$(): Observable<UserBriefAvatarDto[]> {
            return this.userService.getMyManagers().pipe(shareReplay({ bufferSize: 1, refCount: true }));
      }
}