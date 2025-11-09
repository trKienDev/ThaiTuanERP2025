import { inject, Injectable } from "@angular/core";
import { BudgetApproverFacade } from "../facades/budget-approver.facade";
import { environment } from "../../../../environments/environment";
import { resolveAvatarUrl } from "../../../shared/utils/avatar.utils";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { shareReplay } from "rxjs";

@Injectable({ providedIn: 'root' })
export class BudgetApproverOptionStore {
      private readonly facade = inject(BudgetApproverFacade);
      private readonly baseUrl = environment.baseUrl;

      readonly options$ = this.facade.budgetApprovers$.pipe(
            toDropdownOptions({
                  id: 'id',
                  label: (b) => b.approverUser.fullName,
                  imgUrl: (b) => resolveAvatarUrl(this.baseUrl, b.approverUser)
            }),
            shareReplay({ bufferSize: 1, refCount: false })
      );
}