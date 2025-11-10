import { Injectable, inject } from "@angular/core";
import { Observable, tap } from "rxjs";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { BudgetApproverDto, BudgetApproversRequest } from "../models/budget-approvers.model";
import { BudgetApproverService } from "../services/budget-approver.service";

@Injectable({ providedIn: 'root' })
export class BudgetApproverFacade extends BaseCrudFacade<BudgetApproverDto, BudgetApproversRequest> {
      private readonly budgetApproverSerivce = inject(BudgetApproverService);
      constructor() {
            super(inject(BudgetApproverService));
      }
      readonly budgetApprovers$: Observable<BudgetApproverDto[]> = this.list$;
}