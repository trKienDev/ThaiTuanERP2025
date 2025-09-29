import { BehaviorSubject, Observable, shareReplay, switchMap, tap } from "rxjs"

export abstract class BaseCrudFacade<Dto, Request> {
      protected readonly reload$ = new BehaviorSubject<void>(undefined);

      protected constructor(protected service: {
            getAll: () => Observable<Dto[]>;
            create: (req: Request) => Observable<Dto>;
            update: (id: string, req: Request) => Observable<Dto>;
            delete: (id: string) => Observable<boolean>;
      }) {}

      /** Stream danh sách chung cho subclass alias lại  */
      protected readonly list$: Observable<Dto[]> = this.reload$.pipe(
            switchMap(() => this.service.getAll()),
            shareReplay({ bufferSize: 1, refCount: false })
      );

      refresh() { this.reload$.next(); }

      create(req: Request) {
            return this.service.create(req).pipe(tap(() => this.refresh()));
      }
      upadte(id: string, req: Request) {
            return this.service.update(id, req).pipe(tap(() => this.refresh()));
      }
      delete(id: string) {
            return this.service.delete(id).pipe(tap(() => this.refresh()));
      }
}