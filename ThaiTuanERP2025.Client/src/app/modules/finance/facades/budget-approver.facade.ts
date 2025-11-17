import { Injectable, inject } from "@angular/core";
import { Observable} from "rxjs";
import { BudgetApproverDto, BudgetApproversRequest } from "../models/budget-approvers.model";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { BudgetApproverApiService } from "../services/api/budget-approver-api.service";

@Injectable({ providedIn: 'root' })
export class BudgetApproverFacade extends BaseApiFacade<BudgetApproverDto, BudgetApproversRequest> {
      private readonly budgetApproverSerivce = inject(BudgetApproverApiService);
      constructor() {
            super(inject(BudgetApproverApiService));
      }
      readonly budgetApprovers$: Observable<BudgetApproverDto[]> = this.list$;
}