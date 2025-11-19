import { inject, Injectable } from "@angular/core";
import { BudgetCodeFacade } from "../facades/budget-code.facade";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { shareReplay } from "rxjs";

@Injectable({ providedIn: 'root' })
export class BudgetCodeOptionStore {
      private readonly facade = inject(BudgetCodeFacade);

      readonly options$ = this.facade.budgetCodes$.pipe(
            toDropdownOptions({
                  id: 'id',
                  label: (b) => `${b.code} - ${b.name}`,
            }),
            shareReplay({ bufferSize: 1, refCount: false })
      );
}