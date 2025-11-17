import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({ providedIn: 'root' })
export class BudgetPlanEventsService {
      private readonly _updated$ = new Subject<void>();
      updated$ = this._updated$.asObservable();

      notifyUpdated() {
            this._updated$.next();
      }
}
