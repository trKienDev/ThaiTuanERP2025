import { inject, Injectable } from "@angular/core";
import { BudgetApproverFacade } from "../facades/budget-approver.facade";
import { resolveAvatarUrl } from "../../../shared/utils/avatar.utils";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { shareReplay } from "rxjs";

@Injectable({ providedIn: 'root' })
export class BudgetApproverOptionStore {
      private readonly facade = inject(BudgetApproverFacade);

      readonly options$ = this.facade.budgetApprovers$.pipe(
            toDropdownOptions({
                  id: (b) => b.approverUser.id,
                  label: (b) => b.approverUser.fullName,
                  imgUrl: (b) => resolveAvatarUrl(b.approverUser)
            }),
            shareReplay({ bufferSize: 1, refCount: false })
      );
}