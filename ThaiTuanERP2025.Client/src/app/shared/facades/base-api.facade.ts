import { BehaviorSubject, Observable, shareReplay, switchMap, tap } from "rxjs"

export abstract class BaseApiFacade<Dto, Request, Update = Request, UpdateResponse = Dto> {
      protected readonly reload$ = new BehaviorSubject<void>(undefined);

      /** 
       * Danh sách được cache, luôn chạy getAll() ít nhất 1 lần khi facade khởi tạo.
       * Đồng thời sẽ chạy lại mỗi khi gọi refresh().
       */
      readonly list$: Observable<Dto[]>;

      protected constructor(protected service: {
            getAll: () => Observable<Dto[]>;
            create: (req: Request) => Observable<void>;
            update: (id: string, req: Update) => Observable<UpdateResponse>;
            delete: (id: string) => Observable<boolean>;
            toggleActive: (id: string) => Observable<boolean>;
      }) {
            this.list$ = this.reload$.pipe(
                  switchMap(() => this.service.getAll()),
                  shareReplay({ bufferSize: 1, refCount: true })   // <--- auto keep subscription
            );

            /** 
             * AUTO SUBSCRIBE -> đảm bảo list$ luôn hoạt động.
             * Không cần UI subscribe vẫn load.
             */
            this.list$.subscribe();
      }

      refresh() { this.reload$.next(); }

      create(req: Request) {
            return this.service.create(req).pipe(tap(() => this.refresh()));
      }
      update(id: string, req: Update) {
            return this.service.update(id, req).pipe(tap(() => this.refresh()));
      }
      delete(id: string) {
            return this.service.delete(id).pipe(tap(() => this.refresh()));
      }
      toggleActive(id: string) {
            return this.service.toggleActive(id).pipe(
                  tap(() => this.refresh())
            );
      }
}