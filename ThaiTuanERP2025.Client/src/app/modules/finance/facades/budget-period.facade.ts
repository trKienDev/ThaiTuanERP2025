import { inject, Injectable } from "@angular/core";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { BudgetPeriodDto, BudgetPeriodPayload, UpdateBudgetPeriodPayload } from "../models/budget-period.model";
import { BudgetPeriodApiService } from "../services/api/budget-period-api.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class BudgetPeriodFacade extends BaseApiFacade<BudgetPeriodDto, BudgetPeriodPayload, void, UpdateBudgetPeriodPayload, void> {
      private readonly budgetPeriodApiService = inject(BudgetPeriodApiService);
      constructor() {
            super(inject(BudgetPeriodApiService));
      }
      
      readonly budgetPeriods$: Observable<BudgetPeriodDto[]> = this.list$;
}